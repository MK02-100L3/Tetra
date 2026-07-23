using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// 制限時間を管理するクラス
/// </summary>
public class TimerManager : MonoBehaviour
{
    // 制限時間
    [SerializeField]
    private float timeLimit = 60f;

    // タイマー表示
    [SerializeField]
    private TMP_Text timerText;

    // 現在時間
    private float currentTime;

    // スコア管理
    private ScoreManager scoreManager;

    // タイムアップしたか
    public bool IsTimeUp { get; private set; }

    void Start()
    {
        currentTime = timeLimit;
        UpdateTimerText();

        // Scene内のScoreManagerを取得
        scoreManager = FindFirstObjectByType<ScoreManager>();
    }

    void Update()
    {
        // タイムアップ後は何もしない
        if (IsTimeUp)
            return;

        // 時間を減らす
        currentTime -= Time.deltaTime;

        // 0秒になったら
        if (currentTime <= 0)
        {
            currentTime = 0;
            IsTimeUp = true;

            Debug.Log("TIME UP!");

            // リザルト画面へ移動する処理を開始
            StartCoroutine(GameEnd());
        }

        UpdateTimerText();
    }

    /// <summary>
    /// タイマー表示を更新
    /// </summary>
    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        timerText.text = "Time : " + Mathf.CeilToInt(currentTime);
    }

    /// <summary>
    /// タイムアップ後の処理
    /// </summary>
    private IEnumerator GameEnd()
    {
        // 最終スコアを保存
        GameData.Score = scoreManager.Score;

        Debug.Log("2秒待機開始");
        // 2秒待つ
        yield return new WaitForSeconds(2f);

        Debug.Log("Resultへ移動します");
        // Resultシーンへ移動
        SceneManager.LoadScene("Result");
    }
}