using AudioName;
using System.Collections.Generic;
using TMPro;
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

    /// <summary>
    /// hp関連
    /// </summary>
    private PlayerLives lives;
    private GameObject feverEffect;

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

        lives = GetComponent<PlayerLives>();
        feverEffect = transform.Find("FeverEffect").gameObject;
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
            case PlayerState.Fevering:
                // Fevering状態の処理
                HandleFevering();
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
            AudioManager.Instance.PlaySE(SEName.JUMP_SE_NAME);
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
        if (groundChecker.IsGrounded && Mathf.Abs(rb.linearVelocity.y) <= 0.01f)
        {
            // 重力を通常の1.0fに戻す
            rb.gravityScale = 1.0f;
            playerStateMachine.ChangeState(PlayerState.Idle);
        }
    }

    /// <summary>
    /// fevering状態の処理を行うメソッド
    /// 移動して空中でエンターキーを押すとパンチを実行
    /// </summary>
    void HandleFevering()
    {
        FeveringMovement();

        // 空中でキーが押されたらパンチ実行（クールタイムなし）
        if (inputReceiver.IsPressed)
        {
            playerPunchExecutor.ExecutePunch();
        }
    }

    /// <summary>
    /// フィーバー中移動用のメソッド
    /// </summary>
    void FeveringMovement()
    {
        List<EnemyBase> enemies = EnemyManager.Instance.GetLiveEnemy();

        // 追尾対象（自分より上で最も近い敵）を探す
        EnemyBase closestEnemy = null;
        float closestDistance = float.MaxValue;

        foreach (EnemyBase enemy in enemies)
        {
            float dy = enemy.transform.position.y - transform.position.y;
            // 誤差許容
            if (dy < 0.5f)
            {
                continue;
            }

            if (dy < closestDistance)
            {
                closestDistance = dy;
                closestEnemy = enemy;
            }
        }

        // 目標方向を決める
        Vector2 moveDir;
        float speed;

        if (closestEnemy != null)
        {
            Vector2 targetPosition = new Vector2(
                closestEnemy.transform.position.x,
                closestEnemy.transform.position.y + masterData.FeverChaseOffset);

            Vector2 toTarget = targetPosition - (Vector2)transform.position;

            // 方向のみ使う（速度は常に一定）
            moveDir = toTarget.sqrMagnitude > 0.0001f ? toTarget.normalized : Vector2.up;
            speed = masterData.FeverChaseSpeed;
        }
        else
        {
            moveDir = Vector2.up;
            speed = masterData.FeverFlySpeed;
        }

        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
    }

    /// <summary>
    /// フィーバーエフェクトの表示状態を切り替える
    /// </summary>
    public void SetFeverEffectVisible(bool isVisible)
    {
        if (feverEffect != null)
        {
            feverEffect.SetActive(isVisible);
        }
    }

    /// フィーバー状態を開始するメソッド
    public void StartFever()
    {
        lives.SetInvincible(true);
        AudioManager.Instance.StopBGM(BGMName.MAIN_GAME_BGM_NAME);
        AudioManager.Instance.PlayLoopBGM(BGMName.MAIN_GAME_BGM2_NAME);
        SetFeverEffectVisible(true);
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;
        playerStateMachine.ChangeState(PlayerState.Fevering);
    }

    /// フィーバー状態を終了するメソッド
    public void EndFever()
    {
        if (lives != null)
            lives.SetInvincible(false);
        AudioManager.Instance.StopBGM(BGMName.MAIN_GAME_BGM2_NAME);
        AudioManager.Instance.PlayLoopBGM(BGMName.MAIN_GAME_BGM_NAME);
        SetFeverEffectVisible(false);
        playerStateMachine.ChangeState(PlayerState.Jumping);
        rb.gravityScale = masterData.JumpGravityScale;
    }
}
