using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Pouse : MonoBehaviour
{
    //ポーズ画像全体
    [SerializeField]
    private GameObject pauseScreen;
    //ポーズ画面を開いたとき、最初に選択するボタン
    [SerializeField]
    private Button firstButton;
    //現在ポーズ画面を開いているか。
    private bool isPaused = false;

    void Start()
    {
        // Pause ScreenがInspectorで設定されているか確認
        //pauseScreen.SetActive(false);
        if (pauseScreen == null)
        {
            Debug.LogError("Pause Screenが設定されていません！");
            // このスクリプトの動作を停止する
            enabled = false;
            return;
        }
        //ゲーム開始時はポーズ画面を停止する。
        pauseScreen.SetActive(false);
    }

    void Update()
    {
        // キーボードが認識されていて、
        // Escキーがこのフレームで押された場合
        if (Keyboard.current != null &&
            Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            // trueとfalseを反転させる
            isPaused = !isPaused;
            // isPausedがtrueなら表示、falseなら非表示
            pauseScreen.SetActive(isPaused);

            if (isPaused)
            {
                //時を停止させる
                Time.timeScale = 0f;
                // ポーズ画面を開いたとき
                StartCoroutine(SelectFirstButton());
            }
            else
            {
                //時を加速させる
                Time.timeScale = 1f;
                // ポーズ画面を閉じたとき、選択状態を解除する
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }

    private IEnumerator SelectFirstButton()
    {
        // ポーズ画面が完全に表示されるまで1フレーム待つ
        yield return null;

        if (EventSystem.current == null)
        {
            // EventSystemが存在するか確認
            Debug.LogError("EventSystemが見つかりません！");
            yield break;
        }

        if (firstButton == null)
        {
            // First Buttonが設定されているか確認
            Debug.LogError("First Buttonが設定されていません！");
            yield break;
        }
        // 以前の選択状態を解除
        EventSystem.current.SetSelectedGameObject(null);
        // 最初のボタンを選択状態にする
        firstButton.Select();
    }
}
