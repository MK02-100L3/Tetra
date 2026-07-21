using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Input Actionsを扱うクラス
    // Move、Look、Swingなどの入力を取得するために使う
    private PlayerInputActions input;

    // CharacterControllerコンポーネント
    // プレイヤーを移動させるために使用する
    private CharacterController controller;

    // プレイヤーの移動速度
    // SerializeFieldを付けるとInspectorから値を変更できる
    [SerializeField]
    private float moveSpeed = 5.0f;

    void Start()
    {
        // PlayerInputActionsクラスを生成する
        // これでInputActionsで設定したMoveやLookなどを使えるようになる
        input = new PlayerInputActions();

        // Playerに付いているCharacterControllerを取得する
        controller = GetComponent<CharacterController>();

        // Input Systemを有効化する
        // Enable()しないと入力を受け取れない
        input.Enable();
    }

    void Update()
    {
        // Moveアクションの入力を取得する
        // x = 左右、y = 前後
        Vector2 moveInput = input.Player.Move.ReadValue<Vector2>();

        // Vector2をVector3へ変換する
        Vector3 moveDirection = new Vector3(
            moveInput.x,
            0,
            moveInput.y
        );

        // 移動速度を掛ける
        moveDirection *= moveSpeed;

        // フレームレートに依存しない速度にする
        moveDirection *= Time.deltaTime;

        // CharacterControllerでプレイヤーを移動させる
        controller.Move(moveDirection);
    }
}