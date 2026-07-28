using UnityEngine;

public class GameStart : MonoBehaviour
{
    [SerializeField]
    private int startEnemyCount = 10;

    void Start()
    {
        EnemySpawner[] spawners =
            FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);

        for (int i = 0; i < startEnemyCount; i++)
        {
            int index = Random.Range(0, spawners.Length);
            spawners[index].SpawnEnemy();
        }
    }
}