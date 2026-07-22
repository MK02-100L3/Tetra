using UnityEngine;
public class EnemyController : MonoBehaviour
{
    private Rigidbody rb;
    // 飛んでいるかどうか
    private bool isFlying = false;
    public bool IsFlying => isFlying;
    // 飛ぶ方向
    private Vector3 flyDirection;

    // 消えるまでの時間
    private float lifeTimer;

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

    void Start()
    {
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


        // 一直線に飛ぶ
        transform.position += flyDirection * flySpeed * Time.deltaTime;

        // タイマー
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0)
        {
            scoreManager.AddScore(100);
            Destroy(gameObject);
        }
    }

    // プレイヤーから吹っ飛ばされる
    public void KnockBack(Vector3 attackPosition)
    {
        if (isFlying)
            return;

        // 飛ぶ方向
        flyDirection = (transform.position - attackPosition).normalized;
        flyDirection.y = 0;
        flyDirection.Normalize();

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
            enemy.KnockBack(transform.position);
        }

        if (other.CompareTag("Wall"))
        {
            scoreManager.AddScore(100);
            Destroy(gameObject);
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

        // プレイヤーへ向かって移動
        transform.position += direction * moveSpeed * Time.deltaTime;
    }
}
