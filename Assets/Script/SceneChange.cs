using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool enter =
            Keyboard.current != null &&
            Keyboard.current.enterKey.wasPressedThisFrame;

        bool pad =
            Gamepad.current != null &&
            Gamepad.current.buttonSouth.wasPressedThisFrame;

        if (enter || pad)
        {
            NextScene();
        }
    }

    public void NextScene()
    {
        string current = SceneManager.GetActiveScene().name;

       //タイトル→メイン
       if (current == "Title")
        {
            SceneManager.LoadScene("Main");
        }
        //リザルト→タイトル
       else if (current == "Result")
        {
            SceneManager.LoadScene("Title");
        }
    }
}
