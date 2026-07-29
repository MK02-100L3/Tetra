using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            EnemySpawner[] spawners =
                FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);

            int index = Random.Range(0, spawners.Length);

            spawners[index].SpawnEnemy();
        }
    }

    // 今まで通り即スポーン
    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }

    // 遅れてスポーン
    public void SpawnEnemy(float delay)
    {
        StartCoroutine(SpawnDelay(delay));
    }

    private IEnumerator SpawnDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}