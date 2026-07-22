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

    // 飛ぶ速度
    [SerializeField]
    private float flySpeed = 15f;

    // 消えるまでの時間
    [SerializeField]
    private float lifeTime = 2.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // 重力を使わない
        rb.useGravity = false;

        // 物理演算では動かさない
        rb.isKinematic = true;
    }

    void Update()
    {
        if (!isFlying)
            return;

        // 一直線に飛ぶ
        transform.position += flyDirection * flySpeed * Time.deltaTime;

        // タイマー
        lifeTimer -= Time.deltaTime;

        if (lifeTimer <= 0)
        {
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
            Destroy(gameObject);
        }
    }
}
