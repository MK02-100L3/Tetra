using UnityEngine;

/// <summary>
/// スコアを管理するクラス
/// </summary>
public class ScoreManager : MonoBehaviour
{
    // 現在のスコア
    private int score = 0;

    /// <summary>
    /// 現在のスコアを取得する
    /// </summary>
    public int Score => score;

    /// <summary>
    /// スコアを加算する
    /// </summary>
    /// <param name="point">加算する点数</param>
    public void AddScore(int point)
    {
        score += point;

        // コンソールに現在のスコアを表示
        Debug.Log("現在のスコア：" + score);
    }
}