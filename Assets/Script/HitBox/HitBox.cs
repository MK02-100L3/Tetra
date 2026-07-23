using UnityEngine;

public class HitBox : MonoBehaviour
{
    //private void OnTriggerEnter(Collider other)//OnTriggerEnterは自動で呼ばれるのでUpdateじゃなくていい
    //{
    //    // Enemyタグが付いたオブジェクトなら
    //    if (other.CompareTag("Enemy"))
    //    {
    //        Debug.Log("Hit!");
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("何か当たった : " + other.name);

        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Hit!");
        }
    }
}
