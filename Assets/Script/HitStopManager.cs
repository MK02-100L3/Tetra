using System.Collections;
using UnityEngine;

/// <summary>
/// ヒットストップを管理するクラス
/// </summary>
public class HitStopManager : MonoBehaviour
{
    // Singleton
    public static HitStopManager Instance;

    private void Awake()
    {
        // シーン内で1つだけ存在させる
        Instance = this;
    }

    /// <summary>
    /// ヒットストップを開始する
    /// </summary>
    /// <param name="stopTime">止める時間</param>
    public void StartHitStop(float stopTime)
    {
        StartCoroutine(HitStop(stopTime));
    }

    /// <summary>
    /// 時間を止める処理
    /// </summary>
    private IEnumerator HitStop(float stopTime)
    {
        // ゲームを停止
        Time.timeScale = 0f;

        // リアル時間で待機
        yield return new WaitForSecondsRealtime(stopTime);

        // ゲーム再開
        Time.timeScale = 1f;
    }
}