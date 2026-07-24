using UnityEngine;

/// <summary>
/// 敵を一定時間ごとにスポーンするクラス
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    // 生成する敵Prefab
    [SerializeField]
    private GameObject enemyPrefab;

    // 生成間隔
    [SerializeField]
    private float spawnInterval = 1f;

    // タイマー
    private float timer;

    // タイマー管理クラス
    private TimerManager timerManager;

    void Start()
    {
        // Scene内のTimerManagerを取得
        timerManager = FindFirstObjectByType<TimerManager>();
    }

    void Update()
    {
        // タイムアップしたら敵を生成しない
        if (timerManager != null && timerManager.IsTimeUp)
            return;

        // 時間を加算
        timer += Time.deltaTime;

        // 一定時間経過したら敵を生成
        if (timer >= spawnInterval)
        {
            SpawnEnemy();
            timer = 0f;
        }
    }

    /// <summary>
    /// 敵を生成する
    /// </summary>
    private void SpawnEnemy()
    {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}