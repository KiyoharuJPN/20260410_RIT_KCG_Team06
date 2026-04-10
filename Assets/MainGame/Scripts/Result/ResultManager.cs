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
    }

    // Result内容の加算と取得
    // コイン
    public void AddCoin()
    {
        coinCount++;
        CalcScore();
    }
    public void AddCoin(int amount)
    {
        coinCount += amount;
        CalcScore();
    }

    // キルカウント
    public void AddKill()
    {
        killCount++;
        CalcScore();
    }
    public void AddKill(int amount)
    {
        killCount += amount;
        CalcScore();
    }

    // スコア
    void CalcScore()
    {
        score = killCount * masterData.ScorePerEnemyDefeat + coinCount * masterData.ScorePerCoin;
    }
}
