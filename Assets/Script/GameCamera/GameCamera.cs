using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class GameCamera : MonoBehaviour
{
    public float RotSpeed = 0.5f;
    public float RotUpLimit = 40.0f;
    public float RotDownLimit = -20.0f;
    public float CameraRange = 3.0f;
    public float CameraY_Up = 1.5f;


    // Inspectorからプレイヤーを設定する
    [SerializeField]
    private Transform player;

    private float m_nowX_Rot = 0.0f;

    // Input Systemを扱うクラス
    private PlayerInputActions input;


    void Start()
    {
        // 初期X軸の回転量を保存
        m_nowX_Rot = transform.localEulerAngles.x;

        // Input Systemの準備
        input = new PlayerInputActions();
        input.Enable();
    }


    void Update()
    {
        // Lookアクションの入力を取得する
        // x = 左右
        // y = 上下
        Vector2 lookInput = input.Player.Look.ReadValue<Vector2>();


        // 入力値に回転速度を掛ける
        float Up_rot = lookInput.y * RotSpeed;

        // 現在の上下角度を更新
        m_nowX_Rot += Up_rot;

        // カメラの上下角度を制限する
        if (m_nowX_Rot > RotUpLimit || m_nowX_Rot < RotDownLimit)
        {
            m_nowX_Rot = Mathf.Clamp(m_nowX_Rot, RotDownLimit, RotUpLimit);
            Up_rot = 0.0f;
        }

        // プレイヤーを中心に上下へ回転
        transform.RotateAround(
            player.transform.position,
            transform.right,
            Up_rot);


        // 左右入力
        // マイナスを付けることで教材と同じ回転方向にする
        float Left_rot = -lookInput.x * RotSpeed;

        // プレイヤーを中心に左右へ回転
        transform.RotateAround(
            player.transform.position,
            Vector3.up,
            Left_rot);

        // カメラの前方向を基準に一定距離離す
        Vector3 cameraMove = transform.forward * -CameraRange;

        // 少し上に持ち上げる
        cameraMove.y += CameraY_Up;

        // プレイヤーの位置に反映
        transform.position = player.transform.position + cameraMove;
    }
}