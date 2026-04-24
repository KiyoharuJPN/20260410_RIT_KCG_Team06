using System;

/// <summary>
/// UI用にイベントを用意
/// </summary>
public abstract class JumpGaugeUIEvent
{
    public static Action<float> OnChargeUpdate;
    public static Action<bool> OnChargeReady;
    public static Action OnChargeCanceled;
}
