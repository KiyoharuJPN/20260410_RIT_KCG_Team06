using UnityEngine;

public class DebugManager : MonoBehaviour
{
    /// <summary>
    /// ランタイム変更対象のマスターデータ
    /// </summary>
    [SerializeField]
    private GameMasterData data;

    /// <summary>小ジャンプ力入力</summary>
    private string smallJumpPowerText;
    /// <summary>大ジャンプ力入力</summary>
    private string largeJumpPowerText;
    /// <summary>ジャンプ待ち時間入力</summary>
    private string jumpCooldownText;
    /// <summary>上昇重力入力</summary>
    private string jumpGravityScaleText;
    /// <summary>落下重力入力</summary>
    private string fallGravityScaleText;
    /// <summary>コインスコア入力</summary>
    private string scorePerCoinText;
    /// <summary>敵撃破スコア入力</summary>
    private string scorePerEnemyDefeatText;
    /// <summary>制限時間入力</summary>
    private string timeLimitText;
    /// <summary>フィーバー必要量入力</summary>
    private string feverGaugeAmountText;
    /// <summary>コインフィーバー増加入力</summary>
    private string feverPerCoinText;
    /// <summary>敵撃破フィーバー増加入力</summary>
    private string feverPerEnemyDefeatText;
    /// <summary>フィーバー減少量入力</summary>
    private string feverDecreasePerSecondText;

    /// <summary>
    /// 入力欄初期化済みかどうか
    /// </summary>
    private bool isInitialized;
    /// <summary>
    /// 縦スクロール位置
    /// </summary>
    private Vector2 scrollPosition;

    private void Awake()
    {
        InitializeInputFields();
    }

    private void OnGUI()
    {
        if (!ShouldShowDebugUI())
        {
            return;
        }

        if (data == null)
        {
            GUI.Label(new Rect(16, 16, 300, 24), "GameMasterData が未設定です。");
            return;
        }

        if (!isInitialized)
        {
            InitializeInputFields();
        }

        var safe = Screen.safeArea;
        var margin = Mathf.Min(safe.width, safe.height) * 0.03f;
        var areaX = safe.x + margin;
        var areaY = safe.y + margin;
        var areaWidth = Mathf.Min(safe.width - (margin * 2.0f), 520.0f);
        var areaHeight = safe.height - (margin * 2.0f);

        GUILayout.BeginArea(new Rect(areaX, areaY, areaWidth, areaHeight), GUI.skin.box);
        GUILayout.Label("Debug GameMasterData");

        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

        smallJumpPowerText = DrawField("SmallJumpPower", smallJumpPowerText, areaWidth);
        largeJumpPowerText = DrawField("LargeJumpPower", largeJumpPowerText, areaWidth);
        jumpCooldownText = DrawField("JumpCooldown", jumpCooldownText, areaWidth);
        jumpGravityScaleText = DrawField("JumpGravityScale", jumpGravityScaleText, areaWidth);
        fallGravityScaleText = DrawField("FallGravityScale", fallGravityScaleText, areaWidth);

        scorePerCoinText = DrawField("ScorePerCoin", scorePerCoinText, areaWidth);
        scorePerEnemyDefeatText = DrawField("ScorePerEnemyDefeat", scorePerEnemyDefeatText, areaWidth);

        timeLimitText = DrawField("TimeLimit", timeLimitText, areaWidth);

        feverGaugeAmountText = DrawField("FeverGaugeAmount", feverGaugeAmountText, areaWidth);
        feverPerCoinText = DrawField("FeverPerCoin", feverPerCoinText, areaWidth);
        feverPerEnemyDefeatText = DrawField("FeverPerEnemyDefeat", feverPerEnemyDefeatText, areaWidth);
        feverDecreasePerSecondText = DrawField("FeverDecreasePerSecond", feverDecreasePerSecondText, areaWidth);

        GUILayout.Space(8.0f);

        if (GUILayout.Button("適用", GUILayout.Height(36)))
        {
            ApplyToMasterData();
        }

        if (GUILayout.Button("現在値を再読込", GUILayout.Height(36)))
        {
            InitializeInputFields();
        }

        GUILayout.EndScrollView();
        GUILayout.EndArea();
    }

    /// <summary>
    /// エディタまたはデバッグビルド時のみUI表示する
    /// </summary>
    private bool ShouldShowDebugUI()
    {
        return Application.isEditor || Debug.isDebugBuild;
    }

    /// <summary>
    /// 入力欄を現在のマスターデータ値で初期化する
    /// </summary>
    private void InitializeInputFields()
    {
        if (data == null)
        {
            return;
        }

        smallJumpPowerText = data.SmallJumpPower.ToString();
        largeJumpPowerText = data.LargeJumpPower.ToString();
        jumpCooldownText = data.JumpCooldown.ToString();
        jumpGravityScaleText = data.JumpGravityScale.ToString();
        fallGravityScaleText = data.FallGravityScale.ToString();

        scorePerCoinText = data.ScorePerCoin.ToString();
        scorePerEnemyDefeatText = data.ScorePerEnemyDefeat.ToString();

        timeLimitText = data.TimeLimit.ToString();

        feverGaugeAmountText = data.FeverGaugeAmount.ToString();
        feverPerCoinText = data.FeverPerCoin.ToString();
        feverPerEnemyDefeatText = data.FeverPerEnemyDefeat.ToString();
        feverDecreasePerSecondText = data.FeverDecreasePerSecondDuringFever.ToString();

        isInitialized = true;
    }

    /// <summary>
    /// ラベル + 入力欄を描画する
    /// </summary>
    private string DrawField(string label, string value, float areaWidth)
    {
        var labelWidth = Mathf.Clamp(areaWidth * 0.50f, 140.0f, 260.0f);

        GUILayout.BeginHorizontal();
        GUILayout.Label(label, GUILayout.Width(labelWidth));
        value = GUILayout.TextField(value, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();

        return value;
    }

    /// <summary>
    /// 入力値をGameMasterDataへ反映する
    /// </summary>
    private void ApplyToMasterData()
    {
        var smallJumpPower = ParseFloat(smallJumpPowerText, data.SmallJumpPower);
        var largeJumpPower = ParseFloat(largeJumpPowerText, data.LargeJumpPower);
        var jumpCooldown = ParseFloat(jumpCooldownText, data.JumpCooldown);
        var jumpGravityScale = ParseFloat(jumpGravityScaleText, data.JumpGravityScale);
        var fallGravityScale = ParseFloat(fallGravityScaleText, data.FallGravityScale);

        var scorePerCoin = ParseInt(scorePerCoinText, data.ScorePerCoin);
        var scorePerEnemyDefeat = ParseInt(scorePerEnemyDefeatText, data.ScorePerEnemyDefeat);

        var timeLimit = ParseFloat(timeLimitText, data.TimeLimit);

        var feverGaugeAmount = ParseFloat(feverGaugeAmountText, data.FeverGaugeAmount);
        var feverPerCoin = ParseFloat(feverPerCoinText, data.FeverPerCoin);
        var feverPerEnemyDefeat = ParseFloat(feverPerEnemyDefeatText, data.FeverPerEnemyDefeat);
        var feverDecreasePerSecond = ParseFloat(feverDecreasePerSecondText, data.FeverDecreasePerSecondDuringFever);

        data.ApplyRuntimeValues(
            smallJumpPower,
            largeJumpPower,
            jumpCooldown,
            jumpGravityScale,
            fallGravityScale,
            scorePerCoin,
            scorePerEnemyDefeat,
            timeLimit,
            feverGaugeAmount,
            feverPerCoin,
            feverPerEnemyDefeat,
            feverDecreasePerSecond);
    }

    /// <summary>
    /// float文字列を安全にパースする
    /// </summary>
    private float ParseFloat(string text, float fallback)
    {
        return float.TryParse(text, out var value) ? value : fallback;
    }

    /// <summary>
    /// int文字列を安全にパースする
    /// </summary>
    private int ParseInt(string text, int fallback)
    {
        return int.TryParse(text, out var value) ? value : fallback;
    }
}
