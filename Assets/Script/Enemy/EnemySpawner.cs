using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // 生成する敵のPrefab（Inspectorから設定）
    [SerializeField] private GameObject enemyPrefab;

    // 敵を生成する間隔（秒）
    [SerializeField] private float spawnInterval = 1f;

    // 経過時間を測るタイマー
    private float timer;

    void Update()
    {
        // 毎フレーム経過時間を加算する
        timer += Time.deltaTime;

        // 一定時間経過したら敵を生成する
        if (timer >= spawnInterval)
        {
            SpawnEnemy();

            // タイマーをリセットして次の生成まで待つ
            timer = 0f;
        }
    }

    /// <summary>
    /// 敵をスポナーの位置に生成する
    /// </summary>
    void SpawnEnemy()
    {
        // enemyPrefabを、このオブジェクトの位置に、回転なしで生成する
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
    }
}