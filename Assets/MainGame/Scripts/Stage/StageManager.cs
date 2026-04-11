using UnityEngine;

/// <summary>
/// ステージの管理を行うクラス
/// </summary>
public class StageManager : MonoBehaviour
{
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