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
        // 音楽の再生(今までの音楽をストップ)
        AudioManager.Instance.StopBGM(BGMName.MAIN_GAME_BGM2_NAME);
        AudioManager.Instance.StopBGM(BGMName.MAIN_GAME_BGM_NAME);

        AudioManager.Instance.PlayLoopBGM(BGMName.RESULT_BGM_NAME);
        

    }

    // Result内容の加算と取得
    // コイン
    public void AddCoin()
    {
        coinCount++;
        CalcScore();
        Debug.Log($"Coin added! Current coin count: {coinCount}");
    }
    public void AddCoin(int amount)
    {
        coinCount += amount;
        CalcScore();
        Debug.Log($"Coins added {amount} ! Current coin count: {coinCount}");
    }

    // キルカウント
    public void AddKill()
    {
        killCount++;
        CalcScore();
        Debug.Log($"Kill added! Current kill count: {killCount}");
    }
    public void AddKill(int amount)
    {
        killCount += amount;
        CalcScore();
        Debug.Log($"Kills added {amount} ! Current kill count: {killCount}");
    }

    // スコア
    void CalcScore()
    {
        score = killCount * masterData.ScorePerEnemyDefeat + coinCount * masterData.ScorePerCoin;
        Debug.Log($"Score calculated: {score} ");
    }
}
