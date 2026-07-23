using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// リザルト画面を管理するクラス
/// </summary>
public class ResultManager : MonoBehaviour
{
    // スコア表示用テキスト
    [SerializeField]
    private TMP_Text scoreText;

    void Start()
    {
        // 保存していたスコアを表示
        scoreText.text = "Score : " + GameData.Score;
    }

    void Update()
    {
        // スペースキーが押されたらタイトル画面へ戻る
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Title");
        }
    }
}