using AudioName;
using UnityEngine;

public class ResultManager : Singleton<ResultManager>
{
    // 点数計算用
    [SerializeField]
    GameMasterData masterData;

    // Result内容：スコア、キル数、コイン数
    static public int score = 0;
    static public int killCount = 0;
    static public int coinCount = 0; 

    void Awake()
    {
        // 初期化
        score = 0;
        killCount = 0;
        coinCount = 0;

        // 音楽を再生
        AudioManager.Instance.StopBGM(BGMName.TITLE_BGM_NAME);
        AudioManager.Instance.PlayBGM(BGMName.MAIN_GAME_BGM_NAME);
    }

    // Result内容の加算と取得
    // コイン
    public void AddCoin()
    {
        coinCount++;
        CalcScore();
        FeverGaugeManager.Instance.FeverGauge += masterData.FeverPerCoin;
        Debug.Log($"Coin added! Current coin count: {coinCount}");
    }
    public void AddCoin(int amount)
    {
        coinCount += amount;
        CalcScore();
        FeverGaugeManager.Instance.FeverGauge += masterData.FeverPerCoin;
        Debug.Log($"Coins added {amount} ! Current coin count: {coinCount}");
    }

    // キルカウント
    public void AddKill()
    {
        killCount++;
        CalcScore();
        FeverGaugeManager.Instance.FeverGauge += masterData.FeverPerEnemyDefeat;
        Debug.Log($"Kill added! Current kill count: {killCount}");
    }
    public void AddKill(int amount)
    {
        killCount += amount;
        CalcScore();
        FeverGaugeManager.Instance.FeverGauge += masterData.FeverPerEnemyDefeat;
        Debug.Log($"Kills added {amount} ! Current kill count: {killCount}");
    }

    // スコア
    void CalcScore()
    {
        score = killCount * masterData.ScorePerEnemyDefeat + coinCount * masterData.ScorePerCoin;
        Debug.Log($"Score calculated: {score} ");
    }
}
