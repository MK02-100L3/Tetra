using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    // Input Actionsを扱うクラス
    // Move、Look、Swingなどの入力を取得するために使う
    private PlayerInputActions input;

    // CharacterControllerコンポーネント
    // プレイヤーを移動させるために使用する
    private CharacterController controller;
    // Animator
    private Animator animator;

    // プレイヤーの移動速度
    // SerializeFieldを付けるとInspectorから値を変更できる
    [SerializeField]
    private float moveSpeed = 5.0f;

    // プレイヤーを動かす基準となるカメラ
    // InspectorからMain Cameraを設定する
    [SerializeField]
    private Transform cameraTransform;

    [SerializeField]
    private Transform hitBox;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip swingSE;

    [SerializeField]
    private AudioClip hitSE;

    // ヒット時のエフェクト
    [SerializeField]
    private GameObject hitEffect;

    // 最大チャージエフェクト
    [SerializeField]
    private GameObject maxChargeEffect;

    // 最大チャージ演出を出したか
    private bool maxChargeEffectPlayed = false;

    // 攻撃中かどうか
    private bool isAttacking = false;

    // チャージ中かどうか
    private bool isCharging = false;

    // 現在のチャージ時間
    private float chargeTime = 0.0f;

    // 最大チャージ時間
    [SerializeField]
    private float maxChargeTime = 1.0f;

    // 無敵中か
    private bool isInvincible = false;

    // ノックバック中か
    private bool isKnockBack = false;

    // ノックバック速度
    private Vector3 knockBackVelocity;

    [SerializeField]
    private float knockBackPower = 8f;

    [SerializeField]
    private float knockBackTime = 0.2f;

    [SerializeField]
    private float invincibleTime = 1.0f;

    // 通常攻撃の最大連鎖回数
    private const int NormalChainLevel = 1;

    // 最大チャージ時の最大連鎖回数
    private const int MaxChainLevel = 999;

    void Start()
    {
        // Playerに付いているAnimatorを取得
        animator = GetComponent<Animator>();
        // PlayerInputActionsクラスを生成する
        // これでInputActionsで設定したMoveやLookなどを使えるようになる
        input = new PlayerInputActions();

        audioSource = GetComponent<AudioSource>();

        // Playerに付いているCharacterControllerを取得する
        controller = GetComponent<CharacterController>();

        // Input Systemを有効化する
        // Enable()しないと入力を受け取れない
        input.Enable();
    }

    void Update()
    {
        if (isKnockBack)
        {
            controller.Move(knockBackVelocity * Time.deltaTime);
            return;
        }

        bool attackInput = input.Player.Charge.IsPressed();

        // チャージ開始
        if (attackInput && !isCharging && !isAttacking)
        {
            isCharging = true;
            chargeTime = 0.0f;

            maxChargeEffectPlayed = false;

            animator.SetBool("Charging", true);

            Debug.Log("チャージ開始");
        }

        // 攻撃中はチャージしない
        if (isCharging && !isAttacking)
        {
            // 時間を加算
            chargeTime += Time.deltaTime;

            // 最大時間までしか溜めない
            chargeTime = Mathf.Clamp(chargeTime, 0.0f, maxChargeTime);

            if (chargeTime >= maxChargeTime && !maxChargeEffectPlayed)
            {
                maxChargeEffectPlayed = true;

                Instantiate(
                    maxChargeEffect,
                    transform.position + Vector3.up * 1.0f,
                    Quaternion.identity,
                    transform);
            }
        }

        // スティックを離したら攻撃
        if (!attackInput && isCharging)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("MaxChargeEffect"))
                {
                    Destroy(child.gameObject);
                }
            }
            isCharging = false;
            animator.SetBool("Charging", false);
            Debug.Log("チャージ終了");
            Debug.Log(chargeTime);

            StartCoroutine(AttackRoutine());
        }

        // Moveアクションの入力を取得する
        // x = 左右、y = 前後
        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();

        // 入力があるかどうか
        bool isMoving = moveInput.sqrMagnitude > 0.01f;

        // Animatorへ渡す
        animator.SetBool("Run", isMoving);

        //Debug.Log(isMoving);

        // カメラの前方向と右方向を取得
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // 上下方向は無視して地面に沿って移動させる
        forward.y = 0;
        right.y = 0;

        // 長さを1にそろえる
        forward.Normalize();
        right.Normalize();

        // カメラ基準で移動方向を作る
        Vector3 moveDirection =
            forward * moveInput.y +
            right * moveInput.x;

        // 斜め移動が速くならないようにする
        if (moveDirection.sqrMagnitude > 1.0f)
        {
            moveDirection.Normalize();
        }

        // 移動速度を掛ける
        moveDirection *= moveSpeed;

        // フレームレートに依存しない速度にする
        moveDirection *= Time.deltaTime;

        // 移動しているときだけプレイヤーの向きを変える
        if (moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection.normalized;
        }

        //攻撃中は移動できない
        if (!isAttacking && !isCharging)
        {
            controller.Move(moveDirection);
        }

        if (!isAttacking && moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection.normalized;
        }
    }

    /// <summary>

    /// オブジェクトが無効化・破棄されるときに入力を停止する
    /// </summary>
    private void OnDisable()
    {
        // inputが存在するときだけ入力を無効化
        if (input != null)
        {
            input.Disable();
        }
    }
    /// 攻撃処理
    /// </summary>
    //private void Swing()
    //{
    //    Debug.Log("スイング！");
    //}

    public void Attack()
    {
        Debug.Log("Attack()が呼ばれた");

        // チャージ量によって連鎖回数を決める
        bool isMaxCharge = chargeTime >= maxChargeTime;

        int chainLevel;

        if (isMaxCharge)
        {
            chainLevel = MaxChainLevel;
        }
        else
        {
            chainLevel = NormalChainLevel;
        }
        // HitBoxの半分の大きさ
        Vector3 halfExtents = hitBox.localScale / 2.0f;

        // HitBoxの範囲にあるColliderを取得
        Collider[] hits = Physics.OverlapBox(
            hitBox.position,
            halfExtents,
            hitBox.rotation);

        foreach (Collider hit in hits)//foreach配列やリストの中身を1つずつ取り出す
        {
            if (hit.CompareTag("Enemy"))
            {
                EnemyController enemy = hit.GetComponent<EnemyController>();

                if (enemy != null)
                {
                    enemy.KnockBack(transform.position,chainLevel,isMaxCharge);

                    GameCamera cameraShake = cameraTransform.GetComponent<GameCamera>();

                    if (cameraShake != null)
                    {
                        cameraShake.StartShake(0.1f, 0.25f);
                    }

                    audioSource.PlayOneShot(hitSE);

                    // ヒット位置にエフェクトを生成
                    // エフェクトを敵の少し上（1.5m上）に生成する
                    Instantiate(
                        hitEffect,
                        hit.transform.position + Vector3.up * 1.5f,
                        Quaternion.identity);

                    // 0.05秒ヒットストップ
                    HitStopManager.Instance.StartHitStop(0.05f);
                }
            }

        }
    }

    private System.Collections.IEnumerator AttackRoutine()
    {
        Debug.Log("Routine開始");
        // 攻撃開始
        isAttacking = true;

        // アニメーション再生
        animator.SetTrigger("Strike");

        // 当たる瞬間まで待つ
        yield return new WaitForSeconds(0.2f);

        // 攻撃判定
        Attack();

        audioSource.PlayOneShot(swingSE);

        // アニメーション終了まで待つ
        yield return new WaitForSeconds(0.3f);

        // 攻撃終了
        isAttacking = false;
    }

    private void OnDrawGizmos()
    {
        if (hitBox == null)
            return;

        // 攻撃範囲を赤色で表示
        Gizmos.color = Color.red;

        // HitBoxと同じ位置・回転で描画
        Gizmos.matrix = Matrix4x4.TRS(
            hitBox.position,
            hitBox.rotation,
            Vector3.one);

        // OverlapBoxと同じサイズ
        Gizmos.DrawWireCube(
            Vector3.zero,
            hitBox.localScale);
    }

    public void Damage(Vector3 enemyPos)
    {
        if (isInvincible)
            return;

        StartCoroutine(DamageRoutine(enemyPos));
    }

    private IEnumerator DamageRoutine(Vector3 enemyPos)
    {
        isInvincible = true;
        isKnockBack = true;
        maxChargeEffectPlayed = false;

        // チャージ解除
        isCharging = false;
        animator.SetBool("Charging", false);

        // 最大チャージエフェクト削除
        foreach (Transform child in transform)
        {
            if (child.CompareTag("MaxChargeEffect"))
                Destroy(child.gameObject);
        }

        // ノックバック方向
        Vector3 dir = (transform.position - enemyPos).normalized;
        dir.y = 0;

        knockBackVelocity = dir * knockBackPower;

        yield return new WaitForSeconds(knockBackTime);

        knockBackVelocity = Vector3.zero;
        isKnockBack = false;

        StartCoroutine(BlinkRoutine());

        yield return new WaitForSeconds(invincibleTime);

        isInvincible = false;
    }

    private IEnumerator BlinkRoutine()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        // 元の表示状態を保存
        bool[] originalState = new bool[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalState[i] = renderers[i].enabled;
        }

        float timer = 0f;

        while (timer < invincibleTime)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                if (originalState[i])
                    renderers[i].enabled = false;
            }

            yield return new WaitForSeconds(0.1f);

            for (int i = 0; i < renderers.Length; i++)
            {
                if (originalState[i])
                    renderers[i].enabled = true;
            }

            yield return new WaitForSeconds(0.1f);

            timer += 0.2f;
        }

        // 元の状態に戻す
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].enabled = originalState[i];
        }
    }
}