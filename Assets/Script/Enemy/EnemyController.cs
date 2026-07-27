using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    // 飛んでいる敵を一時停止しているかどうか
    private bool isPaused = false;

    private Rigidbody rb;
    // 飛んでいるかどうか
    private bool isFlying = false;
    public bool IsFlying => isFlying;
    // 飛ぶ方向
    private Vector3 flyDirection;

    // 消えるまでの時間
    private float lifeTimer;

    // あと何回連鎖できるか
    private int chainLevel = 0;

    // プレイヤー
    private Transform player;

    // スコア管理クラス
    private ScoreManager scoreManager;

    // 敵の移動速度
    [SerializeField]
    private float moveSpeed = 3.0f;

    // 飛ぶ速度
    [SerializeField]
    private float flySpeed = 15f;

    // 消えるまでの時間
    [SerializeField]
    private float lifeTime = 2.0f;


    // Inspectorから敵ごとに点数を変更できる
    [SerializeField]
    private int scoreValue = 100;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip chainHitSE;   // 敵同士がぶつかった音

    [SerializeField]
    private float normalFlySpeed = 15f;

    [SerializeField]
    private float maxFlySpeed = 25f;

    [SerializeField]
    private float normalRotateSpeed = 900f;

    [SerializeField]
    private float maxRotateSpeed = 1800f;

    private float currentFlySpeed;
    private float currentRotateSpeed;

    //[SerializeField]
    //private AudioClip destroySE;    // 消滅音（後でもOK）


    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Scene内のScoreManagerを取得
        scoreManager = FindFirstObjectByType<ScoreManager>();

        // Playerタグのオブジェクトを探す
        player = GameObject.FindGameObjectWithTag("Player").transform;

        rb = GetComponent<Rigidbody>();

        // 重力を使わない
        rb.useGravity = false;

        // 物理演算では動かさない
        rb.isKinematic = true;
    }

    void Update()
    {
        if (!isFlying)
        {
            MoveToPlayer();
            return;
        }

        // 一時停止中なら飛ばない
        if (isPaused)
        {
            return;
        }

        // 一直線に飛ぶ
        transform.position += flyDirection * currentFlySpeed * Time.deltaTime;

        // 飛んでいる間は転がるように回転する
        transform.Rotate(
            currentRotateSpeed * Time.deltaTime,
            0.0f,
            currentRotateSpeed * Time.deltaTime);

        // タイマー
        lifeTimer -= Time.deltaTime;

        // 飛んでいる時間が終わったら撃破
        if (lifeTimer <= 0)
        {
            DestroyEnemy();

        }
    }

    /// <summary>
    /// 敵を吹っ飛ばす
    /// </summary>
    /// <param name="attackPosition">攻撃した位置</param>
    /// <param name="chainLevel">あと何回連鎖できるか</param>
    public void KnockBack(Vector3 attackPosition, int chainLevel, bool isMaxCharge)
    {
        // すでに飛んでいたら何もしない
        if (isFlying)
            return;

        // この敵の連鎖回数を保存
        this.chainLevel = chainLevel;

        if (isMaxCharge)
        {
            currentFlySpeed = maxFlySpeed;
            currentRotateSpeed = maxRotateSpeed;
        }
        else
        {
            currentFlySpeed = normalFlySpeed;
            currentRotateSpeed = normalRotateSpeed;
        }

        // 飛ぶ方向を計算
        flyDirection = (transform.position - attackPosition).normalized;
        flyDirection.y = 0;
        flyDirection.Normalize();

        // 飛行開始
        isFlying = true;
        lifeTimer = lifeTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isFlying)
            return;

        EnemyController enemy = other.GetComponent<EnemyController>();

        if (enemy != null && !enemy.IsFlying)
        {
            // 相手を吹っ飛ばす
            // まだ連鎖できるなら
            if (chainLevel > 0)
            {
                // 効果音
                audioSource.PlayOneShot(chainHitSE);

                // 一瞬停止
                StartCoroutine(HitPause());

                // 相手へ残り回数を渡す
                enemy.KnockBack(transform.position, chainLevel - 1, currentFlySpeed == maxFlySpeed);
            }
        }

        // 壁に当たったら撃破
        if (other.CompareTag("Wall"))
        {
            DestroyEnemy();
        }
    }

    /// <summary>
    /// プレイヤーへ向かって移動する
    /// </summary>
    private void MoveToPlayer()
    {
        // Playerが見つからなければ何もしない
        if (player == null)
            return;

        // プレイヤーへの方向を計算
        Vector3 direction = (player.position - transform.position).normalized;

        // 地面に沿って移動するためY方向を無視
        direction.y = 0;

        // プレイヤーの方を向く
        if (direction != Vector3.zero)
        {
            transform.forward = direction.normalized;
        }

        // プレイヤーへ向かって移動
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// スコアを加算して敵を削除する
    /// </summary>
    private void DestroyEnemy()
    {
        chainLevel = 0;
        // ScoreManagerが存在する場合のみ加算する
        if (scoreManager != null)
        {
            scoreManager.AddScore(scoreValue);
        }

        // 敵を削除
        Destroy(gameObject);
    }

    /// <summary>
    /// スコアを加算せずに敵を削除する
    /// タイムアップ時などで使用する
    /// </summary>
    public void DestroyWithoutScore()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// 飛んでいる敵を一定時間停止させる
    /// </summary>
    private IEnumerator HitPause()
    {
        // 飛ぶのを一時停止する
        isPaused = true;

        // 0.03秒停止する
        yield return new WaitForSeconds(0.2f);//yield returnはここで一旦処理を止めて、あとで続きを実行するという意味

        // 飛行を再開する
        isPaused = false;
    }
}

