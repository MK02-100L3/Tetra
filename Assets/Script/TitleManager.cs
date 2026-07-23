using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
/// <summary>
/// タイトル画面を管理するクラス
/// </summary>
public class TitleManager : MonoBehaviour
{
    // 入力取得
    private PlayerInputActions input;

    void Start()
    {
        // Input Systemを生成
        input = new PlayerInputActions();

        // 入力を有効化
        input.Enable();
    }

    void Update()
    {
        // SpaceキーやAボタンなど、Submit入力でゲーム開始
        if (input.Player.Swing.WasPressedThisFrame())
        {
            SceneManager.LoadScene("Main");
        }

        // スペースキーが押された瞬間
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("Main");
        }
    }

    /// <summary>
    /// シーン終了時に入力を無効化
    /// </summary>
    private void OnDisable()
    {
        if (input != null)
        {
            input.Disable();
        }
    }
}