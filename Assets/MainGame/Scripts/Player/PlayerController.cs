using UnityEngine;

/// <summary>
/// プレイヤーの操作を管理するクラス
/// </summary>
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("StageManagerをアタッチしてください")]
    private StageManager stageManager;

    /// <summary>
    /// PlayerInputReceiverクラスのインスタンスを保持する変数
    /// </summary>
    private PlayerInputReceiver inputReceiver;

    /// <summary>
    /// PlayerJumpHandlerクラスのインスタンスを保持する変数
    /// </summary>
    private PlayerJumpHandler playerJumpHandler;

    /// <summary>
    /// PlayerJumpExecutorクラスのインスタンスを保持する変数
    /// </summary>
    private PlayerJumpExecutor playerJumpExecutor;

    /// <summary>
    /// PlayerPunchExecutorクラスのインスタンスを保持する変数
    /// </summary>
    private PlayerPunchExecutor playerPunchExecutor;

    /// <summary>
    /// PlayerStateMachineクラスのインスタンスを保持する変数
    /// </summary>
    private PlayerStateMachine playerStateMachine;

    /// <summary>
    /// GroundCheckerクラスのインスタンスを保持する変数
    /// </summary>
    private GroundChecker groundChecker;

    /// <summary>
    /// Rigidbody2Dコンポーネントを保持する変数
    /// </summary>
    private Rigidbody2D rb;

    [SerializeField]
    [Tooltip("マスターデータ")]
    private GameMasterData masterData;

    /// <summary>
    /// チャージキャンセルまでの時間（秒）
    /// </summary>
    private const float CHARGE_CANCEL_DURATION = 2.0f;

    /// <summary>
    /// ChargeReady状態に入ってからの経過時間
    /// </summary>
    private float m_chargeReadyTimer = 0f;

    void Awake()
    {
        // 各コンポーネントを取得して変数に格納
        inputReceiver      = GetComponent<PlayerInputReceiver>();
        playerJumpHandler  = GetComponent<PlayerJumpHandler>();
        playerJumpExecutor = GetComponent<PlayerJumpExecutor>();
        playerPunchExecutor = GetComponent<PlayerPunchExecutor>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
        groundChecker      = GetComponent<GroundChecker>();
        rb                 = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 現在の状態に応じた処理を実行
        switch (playerStateMachine.CurrentState)
        {
            case PlayerState.Idle:
                // Idle状態の処理
                HandleIdle();
                break;
            case PlayerState.Charging:
                // Charging状態の処理
                HandleCharging();
                break;
            case PlayerState.ChargeReady:
                // ChargeReady状態の処理
                HandleChargeReady();
                break;
            case PlayerState.Jumping:
                // Jumping状態の処理
                HandleJump();
                break;
            case PlayerState.Falling:
                // Falling状態の処理
                HandleFall();
                break;
        }
    }

    /// <summary>
    /// 待機状態の処理を行うメソッド
    /// </summary>
    private void HandleIdle()
    {
        // 地面にいてキーが押されたらチャージ開始
        if (inputReceiver.IsPressed && groundChecker.IsGrounded)
        {
            playerJumpHandler.StartHold();
            playerStateMachine.ChangeState(PlayerState.Charging);
        }
    }

    /// <summary>
    /// チャージ状態の処理を行うメソッド
    /// キーを押し続けている間チャージが蓄積される
    /// </summary>
    private void HandleCharging()
    {
        // キーを離したらChargeReady状態へ（チャージ量を確定）
        if (inputReceiver.IsReleased)
        {
            playerJumpHandler.EndHold();  // HoldDuration を確定
            m_chargeReadyTimer = 0f;      // タイマーをリセット
            playerStateMachine.ChangeState(PlayerState.ChargeReady);
        }
    }

    /// <summary>
    /// チャージ確定後の待機状態の処理を行うメソッド
    /// もう一度押すとジャンプ、2秒経過でキャンセル
    /// </summary>
    private void HandleChargeReady()
    {
        m_chargeReadyTimer += Time.deltaTime;

        // もう一度エンターキーを押したらジャンプ
        if (inputReceiver.IsPressed)
        {
            playerJumpExecutor.Jump(
                playerJumpHandler.HoldDuration,
                stageManager.IsBonusStage
            );
            playerStateMachine.ChangeState(PlayerState.Jumping);
            return;
        }

        // 2秒経過でチャージをキャンセルしてIdle状態へ
        if (m_chargeReadyTimer >= CHARGE_CANCEL_DURATION)
        {
            Debug.Log("チャージキャンセル");
            playerStateMachine.ChangeState(PlayerState.Idle);
        }
    }

    /// <summary>
    /// ジャンプ状態の処理を行うメソッド
    /// 空中でエンターキーを押すとパンチを実行
    /// </summary>
    private void HandleJump()
    {
        // 空中でキーが押されたらパンチ実行（クールタイムなし）
        if (inputReceiver.IsPressed)
        {
            playerPunchExecutor.ExecutePunch();
        }

        // 上昇が終わったら落下用の重力スケールに切り替えてFalling状態へ
        if (rb.linearVelocity.y <= 0)
        {
            // 落下中は重力を強くして早く落ちるように感覚にする
            rb.gravityScale = masterData.FallGravityScale;
            playerStateMachine.ChangeState(PlayerState.Falling);
        }
    }

    /// <summary>
    /// 落下状態の処理を行うメソッド
    /// 空中でエンターキーを押すとパンチを実行
    /// </summary>
    private void HandleFall()
    {
        // 空中でキーが押されたらパンチ実行（クールタイムなし、着地まで可能）
        if (inputReceiver.IsPressed)
        {
            playerPunchExecutor.ExecutePunch();
        }

        // 地面に着いたら重力スケールをデフォルトに戻してIdle状態へ
        if (groundChecker.IsGrounded)
        {
            // 重力を通常の1.0fに戻す
            rb.gravityScale = 1.0f;
            playerStateMachine.ChangeState(PlayerState.Idle);
        }
    }
}
