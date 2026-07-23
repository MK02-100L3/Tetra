using TMPro;
using UnityEngine;

/// <summary>
/// 制限時間を管理するクラス
/// </summary>
public class TimerManager : MonoBehaviour
{
    // 制限時間
    [SerializeField]
    private float timeLimit = 60f;

    // タイマー表示用
    [SerializeField]
    private TMP_Text timerText;

    // 現在の残り時間
    private float currentTime;

    // タイムアップしたか
    public bool IsTimeUp { get; private set; }

    void Start()
    {
        // 初期時間を設定
        currentTime = timeLimit;

        // 表示更新
        UpdateTimerText();
    }

    void Update()
    {
        // タイムアップ後は何もしない
        if (IsTimeUp)
            return;

        // 時間を減らす
        currentTime -= Time.deltaTime;

        // 0未満にならないようにする
        if (currentTime <= 0)
        {
            currentTime = 0;
            IsTimeUp = true;

            Debug.Log("TIME UP!");
        }

        // UI更新
        UpdateTimerText();
    }

    /// <summary>
    /// タイマー表示を更新
    /// </summary>
    private void UpdateTimerText()
    {
        if (timerText == null)
            return;

        // 小数点を表示せず整数だけ表示
        timerText.text = "Time : " + Mathf.CeilToInt(currentTime);
    }
}