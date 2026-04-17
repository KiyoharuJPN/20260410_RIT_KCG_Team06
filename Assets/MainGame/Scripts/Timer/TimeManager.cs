using DG.Tweening;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    /// <summary>
    /// 時間制限
    /// </summary>
    [SerializeField]
    private GameMasterData gameMasterData;

    /// <summary>
    /// 残り時間
    /// </summary>
    public float TimeLimit { get; private set; }

    /// <summary>
    /// ゲームオーバー遷移済みかどうか
    /// </summary>
    private bool isGameOver;

    private void Start()
    {
        InitializeGameRuntimeState();
        TimeLimit = gameMasterData.TimeLimit;
        isGameOver = false;
    }

    private void Update()
    {
        if (isGameOver)
        {
            return;
        }

        TimeLimit -= Time.deltaTime;
        if (TimeLimit <= 0.0f)
        {
            TimeLimit = 0.0f;
            GameOver();
        }
    }

    /// <summary>
    /// ゲーム開始時にランタイム値を初期化する
    /// </summary>
    private void InitializeGameRuntimeState()
    {
        ResultManager.score = 0;
        ResultManager.killCount = 0;
        ResultManager.coinCount = 0;

        var feverGaugeManager = FeverGaugeManager.Instance;
        if (feverGaugeManager != null)
        {
            feverGaugeManager.FeverGauge = 0.0f;
        }
    }

    private void GameOver()
    {
        if (isGameOver)
        {
            return;
        }

        isGameOver = true;

        SceneChangeManager.Instance
            .SetFadeDuration(0.4f, 0.3f)
            .SetFadeEase(Ease.OutCubic, Ease.InCubic)
            .SetFadeColor(Color.black)
            .ChangeScene("ResultScene");
    }
}
