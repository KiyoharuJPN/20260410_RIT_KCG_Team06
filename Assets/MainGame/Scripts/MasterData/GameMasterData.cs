using UnityEngine;

[CreateAssetMenu(fileName = "GameMasterData", menuName = "Scriptable Objects/GameMasterData")]
public class GameMasterData : ScriptableObject
{
    [Header("Jump")]
    [SerializeField]
    private float smallJumpPower = 8.0f;
    [SerializeField]
    private float largeJumpPower = 12.0f;
    [SerializeField]
    private float jumpCooldown = 0.2f;
    [SerializeField]
    [Tooltip("上昇中の重力スケール（小さいほど一気に上昇する）")]
    private float jumpGravityScale = 1.0f;
    [SerializeField]
    [Tooltip("落下中の重力スケール（大きいほど素早く落ちる）")]
    private float fallGravityScale = 3.0f;

    [Header("Score")]
    [SerializeField]
    private int scorePerCoin = 100;
    [SerializeField]
    private int scorePerEnemyDefeat = 300;

    [Header("Time")]
    [SerializeField]
    private float timeLimit = 60.0f;

    [Header("Fever")]
    [SerializeField]
    private float feverGaugeAmount = 100.0f;
    [SerializeField]
    private float feverPerCoin = 5.0f;
    [SerializeField]
    private float feverPerEnemyDefeat = 10.0f;
    [SerializeField]
    private float feverDecreasePerSecondDuringFever = 15.0f;

    /// <summary>
    /// 小ジャンプ力
    /// </summary>
    public float SmallJumpPower => smallJumpPower;
    /// <summary>
    /// 大ジャンプ力
    /// </summary>
    public float LargeJumpPower => largeJumpPower;
    /// <summary>
    /// ジャンプ待ち時間
    /// </summary>
    public float JumpCooldown => jumpCooldown;
    /// <summary>
    /// 上昇中の重力スケール
    /// </summary>
    public float JumpGravityScale => jumpGravityScale;
    /// <summary>
    /// 落下中の重力スケール
    /// </summary>
    public float FallGravityScale => fallGravityScale;

    /// <summary>
    /// コインで増えるスコア
    /// </summary>
    public int ScorePerCoin => scorePerCoin;
    /// <summary>
    /// 敵を倒して増えるスコア
    /// </summary>
    public int ScorePerEnemyDefeat => scorePerEnemyDefeat;

    /// <summary>
    /// 制限時間
    /// </summary>
    public float TimeLimit => timeLimit;

    /// <summary>
    /// フィーバーゲージの量
    /// </summary>
    public float FeverGaugeAmount => feverGaugeAmount;
    /// <summary>
    /// コインで増えるフィーバーの量
    /// </summary>
    public float FeverPerCoin => feverPerCoin;
    /// <summary>
    /// 敵を倒して増えるフィーバーの量
    /// </summary>
    public float FeverPerEnemyDefeat => feverPerEnemyDefeat;
    /// <summary>
    /// フィーバー中に減るフィーバーの量
    /// </summary>
    public float FeverDecreasePerSecondDuringFever => feverDecreasePerSecondDuringFever;

    /// <summary>
    /// デバッグ用にランタイム値を一括反映する
    /// </summary>
    public void ApplyRuntimeValues(
        float newSmallJumpPower,
        float newLargeJumpPower,
        float newJumpCooldown,
        float newJumpGravityScale,
        float newFallGravityScale,
        int newScorePerCoin,
        int newScorePerEnemyDefeat,
        float newTimeLimit,
        float newFeverGaugeAmount,
        float newFeverPerCoin,
        float newFeverPerEnemyDefeat,
        float newFeverDecreasePerSecondDuringFever)
    {
        smallJumpPower = Mathf.Max(0.0f, newSmallJumpPower);
        largeJumpPower = Mathf.Max(0.0f, newLargeJumpPower);
        jumpCooldown = Mathf.Max(0.0f, newJumpCooldown);
        jumpGravityScale = Mathf.Max(0.0f, newJumpGravityScale);
        fallGravityScale = Mathf.Max(0.0f, newFallGravityScale);

        scorePerCoin = Mathf.Max(0, newScorePerCoin);
        scorePerEnemyDefeat = Mathf.Max(0, newScorePerEnemyDefeat);

        timeLimit = Mathf.Max(0.0f, newTimeLimit);

        feverGaugeAmount = Mathf.Max(0.0001f, newFeverGaugeAmount);
        feverPerCoin = Mathf.Max(0.0f, newFeverPerCoin);
        feverPerEnemyDefeat = Mathf.Max(0.0f, newFeverPerEnemyDefeat);
        feverDecreasePerSecondDuringFever = Mathf.Max(0.0f, newFeverDecreasePerSecondDuringFever);
    }
}
