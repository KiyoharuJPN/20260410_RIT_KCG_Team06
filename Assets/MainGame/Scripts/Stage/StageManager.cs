using UnityEngine;

/// <summary>
/// ステージの管理を行うクラス
/// </summary>
public class StageManager : Singleton<StageManager>
{
    // ステージ用
    [SerializeField]
    GameMasterData masterData;
    public GameMasterData MasterData => masterData;
    [SerializeField]
    GameObject StaticEnvironment;
    [SerializeField]
    GameObject StaticEnvironmentPrefab;
    [SerializeField]
    GameObject DynamicEnvironment;

    public void ResetStaticEnvironment()
    {
        
    }

















    // 消したらエラーを出すので、残しておいた
    /// <summary>
    /// ボーナスステージかどうかを示すプロパティ
    /// </summary>
    public bool IsBonusStage { get; private set; }

    /// <summary>
    /// ボーナスステージかどうかを設定する
    /// </summary>
    public void SetBonus(bool value)
    {
        IsBonusStage = value;
    }
}