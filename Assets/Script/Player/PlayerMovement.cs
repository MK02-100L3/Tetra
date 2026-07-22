using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Input Actionsを扱うクラス
    // Move、Look、Swingなどの入力を取得するために使う
    private PlayerInputActions input;

    // CharacterControllerコンポーネント
    // プレイヤーを移動させるために使用する
    private CharacterController controller;

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

    void Start()
    {
        // PlayerInputActionsクラスを生成する
        // これでInputActionsで設定したMoveやLookなどを使えるようになる
        input = new PlayerInputActions();

        // Playerに付いているCharacterControllerを取得する
        controller = GetComponent<CharacterController>();

        // Input Systemを有効化する
        // Enable()しないと入力を受け取れない
        input.Enable();
    }

    void Update()
    {
        // Swingアクションが押された瞬間を取得する
        if (input.Player.Swing.WasPressedThisFrame())
        {
            Attack();
        }

        // Moveアクションの入力を取得する
        // x = 左右、y = 前後
        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();

        // カメラの前方向と右方向を取得する
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        // 上下方向の移動をなくす
        // カメラが少し下を向いていても地面に沿って移動できるようにする
        forward.y = 0;
        right.y = 0;

        // ベクトルの長さを1にそろえる
        forward.Normalize();
        right.Normalize();

        // 入力方向をカメラ基準の移動方向へ変換する
        Vector3 moveDirection =
            forward * moveInput.y +
            right * moveInput.x;

        // 移動速度を掛ける
        moveDirection *= moveSpeed;

        // フレームレートに依存しない速度にする
        moveDirection *= Time.deltaTime;

        // 移動しているときだけプレイヤーの向きを変える
        if (moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection.normalized;
        }

        // CharacterControllerでプレイヤーを移動させる
        controller.Move(moveDirection);
    }

    /// <summary>
    /// 攻撃処理
    /// </summary>
    void Attack()
    {
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
                    enemy.KnockBack(transform.position);
                }
            }
        }
    }
}