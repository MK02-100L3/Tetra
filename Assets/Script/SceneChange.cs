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
            if (SceneManager.GetActiveScene().name == "Title")
            {
                SceneManager.LoadScene("Game");
            }
            else if (SceneManager.GetActiveScene().name == "Game")
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
