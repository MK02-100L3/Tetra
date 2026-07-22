using UnityEngine;

public class EnemyController : MonoBehaviour
{
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

    void Update()
    {
        if (!isFlying)
            return;

        // 一直線に移動
        transform.position += flyDirection * flySpeed * Time.deltaTime;

        // タイマーを減らす
        lifeTimer -= Time.deltaTime;

        // 一定時間経ったら消える
        if (lifeTimer <= 0)
        {
            Destroy(gameObject);
        }
    }

    // プレイヤーから吹っ飛ばされる
    public void KnockBack(Vector3 attackPosition)
    {
        // 攻撃地点から敵への方向を求める
        flyDirection = (transform.position - attackPosition).normalized;

        // 地面に沿って飛ばす
        flyDirection.y = 0;

        // 飛行開始
        isFlying = true;

        // タイマーをリセット
        lifeTimer = lifeTime;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 自分が飛んでいなければ何もしない
        if (!isFlying)
            return;

        // 相手がEnemyなら
        EnemyController enemy =
            collision.gameObject.GetComponent<EnemyController>();

        if (enemy != null)
        {
            Debug.Log("連鎖！");
        }
    }
}
