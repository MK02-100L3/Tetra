using TMPro;
using UnityEngine;

/// <summary>
/// スコアを管理するクラス
/// </summary>
public class ScoreManager : MonoBehaviour
{
    // 現在のスコア
    private int score = 0;

    // 現在のスコアを取得する
    public int Score => score;

    // スコア表示用のText
    [SerializeField]
    private TMP_Text scoreText;

    void Start()
    {
        // ゲーム開始時に表示を更新
        UpdateScoreText();
    }

    /// <summary>
    /// スコアを加算する
    /// </summary>
    /// <param name="point">加算する点数</param>
    public void AddScore(int point)
    {
        // スコアを加算
        score += point;

        // UIを更新
        UpdateScoreText();
    }

    /// <summary>
    /// スコア表示を更新する
    /// </summary>
    private void UpdateScoreText()
    {
        // Textが設定されていない場合は何もしない
        if (scoreText == null)
            return;

        // スコアを表示
        scoreText.text = "Score : " + score;
    }

    /// <summary>
    /// 現在のスコアに応じた敵の最大数を返す
    /// </summary>
    public int GetMaxEnemyCount()
    {
        if (score >= 5000)
        {
            return 30;
        }

        if (score >= 3000)
        {
            return 25;
        }

        if (score >= 1500)
        {
            return 20;
        }

        if (score >= 500)
        {
            return 15;
        }

        return 10;
    }
}