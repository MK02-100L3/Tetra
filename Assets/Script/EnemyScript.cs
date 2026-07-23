using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public Transform player;       //プレイヤーオブジェクトのTransform
    public float moveSpeed = 5.0f; //敵の移動速度

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            //プレイヤーの位置を向く
            transform.LookAt(player);

            //プレイヤーに向かって移動
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }
}
