using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float moveSpeed = 5.0f;
  
    void Update()
    {
        Vector3 move = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            move.z += 1;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            move.z -= 1;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            move.x += 1;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            move.x -= 1;
        }

        transform.position += move * moveSpeed * Time.deltaTime;
    }
}