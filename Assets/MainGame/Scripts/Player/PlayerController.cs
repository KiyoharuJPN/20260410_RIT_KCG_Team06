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

    void Awake()
    {
        // 各コンポーネントを取得して変数に格納
        inputReceiver = GetComponent<PlayerInputReceiver>();
        playerJumpHandler = GetComponent<PlayerJumpHandler>();
        playerJumpExecutor = GetComponent<PlayerJumpExecutor>();
        playerStateMachine = GetComponent<PlayerStateMachine>();
        groundChecker = GetComponent<GroundChecker>();
        rb = GetComponent<Rigidbody2D>();

        // ===== デバッグ：各コンポーネントの取得状態を確認 =====
        Debug.Log($"inputReceiver:      {(inputReceiver != null ? "OK" : "NULL")}");
        Debug.Log($"playerJumpHandler:  {(playerJumpHandler != null ? "OK" : "NULL")}");
        Debug.Log($"playerJumpExecutor: {(playerJumpExecutor != null ? "OK" : "NULL")}");
        Debug.Log($"playerStateMachine: {(playerStateMachine != null ? "OK" : "NULL")}");
        Debug.Log($"groundChecker:      {(groundChecker != null ? "OK" : "NULL")}");
        Debug.Log($"rb:                 {(rb != null ? "OK" : "NULL")}");
        Debug.Log($"stageManager:       {(stageManager != null ? "OK" : "NULL")}");
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
        // ジャンプキーが押されたときのデバッグログ
        if (inputReceiver.IsPressed)
        {
            Debug.Log($"IsGrounded = {groundChecker.IsGrounded}");
        }

        // ジャンプキーが押されていて、かつ地面にいる場合
        if (inputReceiver.IsPressed && groundChecker.IsGrounded)
        {
            // チャージ開始を記録
            playerJumpHandler.StartHold();

            // キーが押されたと同時に離されれば単押し判定として小ジャンプを即実行
            if (inputReceiver.IsReleased)
            {
                Debug.Log("単押し検出");
                // ジャンプのチャージを終了し、ジャンプを実行して状態をJumpingに変更
                playerJumpHandler.EndHold();
                playerJumpExecutor.Jump(
                    playerJumpHandler.HoldDuration,
                    stageManager.IsBonusStage
                );
                playerStateMachine.ChangeState(PlayerState.Jumping);
            }
            else
            {
                Debug.Log("長押し検出");
                // 長押し中：Charging状態へ遷移してキー離しを待つ
                playerStateMachine.ChangeState(PlayerState.Charging);
            }
        }
    }

    /// <summary>
    /// チャージ状態の処理を行うメソッド
    /// </summary>
    private void HandleCharging()
    {
        // ジャンプキーを離したらジャンプ
        if (inputReceiver.IsReleased)
        {
            // ジャンプのチャージを終了し、ジャンプを実行して状態をJumpingに変更
            playerJumpHandler.EndHold();
            playerJumpExecutor.Jump(
                playerJumpHandler.HoldDuration,
                stageManager.IsBonusStage
            );
            playerStateMachine.ChangeState(PlayerState.Jumping);
        }
    }

    /// <summary>
    /// ジャンプ状態の処理を行うメソッド
    /// </summary>
    private void HandleJump()
    {
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
    /// </summary>
    private void HandleFall()
    {
        // 地面に着いたら重力スケールをデフォルトに戻してIdle状態へ
        if (groundChecker.IsGrounded)
        {
            // 重力を通常の1.0fに戻す
            rb.gravityScale = 1.0f;
            playerStateMachine.ChangeState(PlayerState.Idle);
        }
    }
}
