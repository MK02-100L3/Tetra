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
        if (Keyboard.current.enterKey.wasPressedThisFrame)
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
