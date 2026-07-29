using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private float spawnInterval = 1.0f;

    [SerializeField]
    private bool isManager = false;

    private float timer;

    private ScoreManager scoreManager;

    void Start()
    {
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    void Update()
    {
        if (!isManager)
            return;

        timer += Time.deltaTime;

        if (timer < spawnInterval)
            return;

        timer = 0;

        if (scoreManager.Score >= 60000)
        {
            spawnInterval = 0.1f;
        }
        else if (scoreManager.Score >= 30000)
        {
            spawnInterval = 0.2f;
        }
        else if (scoreManager.Score >= 25000)
        {
            spawnInterval = 0.4f;
        }
        else if (scoreManager.Score >= 10000)
        {
            spawnInterval = 0.6f;
        }
        else
        {
            spawnInterval = 0.8f;
        }

        int enemyCount =
            FindObjectsByType<EnemyController>(FindObjectsSortMode.None).Length;

        int maxEnemy = scoreManager.GetMaxEnemyCount();

        if (enemyCount < maxEnemy)
        {
            EnemySpawner[] spawners =
                FindObjectsByType<EnemySpawner>(FindObjectsSortMode.None);

            int index = Random.Range(0, spawners.Length);
            spawners[index].SpawnEnemy();
        }
    }

    public void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, transform.rotation);
    }
}