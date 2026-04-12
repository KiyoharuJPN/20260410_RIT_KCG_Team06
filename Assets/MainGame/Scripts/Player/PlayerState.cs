/// <summary>
/// プレイヤーの状態を表す列挙型
/// </summary>
public enum PlayerState
{
    Idle,
    Charging,     // 地面でエンター長押し中：チャージ蓄積
    ChargeReady,  // エンターを離した後：2秒以内に再押下でジャンプ
    Jumping,
    Falling
}