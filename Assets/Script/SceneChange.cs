using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    private Pouse pouse;
   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        pouse = GetComponent<Pouse>();
    }

    // Update is called once per frame
    void Update()
    {
        SceneLoop();

        //Mainシーンの時だけポーズ処理
        if (SceneManager.GetActiveScene().name == "Main")
        {
            pouse.Pause();
        }
    }

    private void SceneLoop()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            if (SceneManager.GetActiveScene().name == "Title")
            {
                SceneManager.LoadScene("Main");
            }
            else if (SceneManager.GetActiveScene().name == "Main")
            {
                SceneManager.LoadScene("Result");
            }
            else if (SceneManager.GetActiveScene().name == "Result")
            {
                SceneManager.LoadScene("Title");
            }
        }
    }
}
