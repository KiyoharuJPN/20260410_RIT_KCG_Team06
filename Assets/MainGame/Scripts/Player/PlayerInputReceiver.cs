using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// プレイヤーの入力を受け取るクラス。
/// </summary>
public class PlayerInputReceiver : MonoBehaviour
{
    /// <summary>
    /// このフレームでキーが押されたか
    /// </summary>
    public bool IsPressed { get; private set; }

    /// <summary>
    /// このフレームでキーが離されたか
    /// </summary>
    public bool IsReleased { get; private set; }

    /// <summary>
    /// 現在キーが押し続けられているか（フレームをまたいで維持される）
    /// </summary>
    public bool IsHolding { get; private set; }

    /// <summary>
    /// PlayerInput コンポーネントの参照
    /// </summary>
    private PlayerInput playerInput;

    /// <summary>
    /// ジャンプ（Punch）アクションの参照
    /// </summary>
    private InputAction punchAction;

    private void Awake()
    {
        // PlayerInput コンポーネントを取得
        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        // ActionMapからPunchアクションを取得してコールバックを登録
        punchAction = playerInput.actions["Punch"];
        punchAction.started  += OnPunchStarted;
        punchAction.canceled += OnPunchCanceled;

        Debug.Log("PlayerInputReceiver: Punchアクションのコールバックを登録しました!");
    }

    private void OnDisable()
    {
        // オブジェクト無効化時にコールバックを解除
        if (punchAction != null)
        {
            punchAction.started  -= OnPunchStarted;
            punchAction.canceled -= OnPunchCanceled;
        }
    }

    /// <summary>
    /// キーが押されたときに呼ばれるコールバック
    /// </summary>
    private void OnPunchStarted(InputAction.CallbackContext context)
    {
        Debug.Log("Punch: Pressed");
        IsPressed = true;
        IsHolding = true; // 押し始めを記録
    }

    /// <summary>
    /// キーが離されたときに呼ばれるコールバック
    /// </summary>
    private void OnPunchCanceled(InputAction.CallbackContext context)
    {
        Debug.Log("Punch: Released");
        IsReleased = true;
        IsHolding = false; // 押し終わりを記録
    }

    /// <summary>
    /// フレームの最後に両フラグをリセットする
    /// </summary>
    private void LateUpdate()
    {
        // 次フレームのためにリセット
        IsPressed = false;
        IsReleased = false;
    }
}
