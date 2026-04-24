using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ジャンプゲージUI
/// </summary>
public class JumpGaugeUI : MonoBehaviour
{
    [SerializeField]
    [Tooltip("ジャンプゲージUI")]
    private Image gauge;

    [SerializeField]
    [Tooltip("ジャンプゲージ満タンエフェクト")]
    private GameObject readyEffect;

    /// <summary>
    /// 各ジャンプチャージイベント登録
    /// </summary>
    private void OnEnable()
    {
        JumpGaugeUIEvent.OnChargeUpdate += UpdateGauge;
        JumpGaugeUIEvent.OnChargeReady += SetReady;
        JumpGaugeUIEvent.OnChargeCanceled += ResetUI;
    }

    /// <summary>
    /// 各ジャンプチャージイベント解除
    /// </summary>
    private void OnDisable()
    {
        JumpGaugeUIEvent.OnChargeUpdate -= UpdateGauge;
        JumpGaugeUIEvent.OnChargeReady -= SetReady;
        JumpGaugeUIEvent.OnChargeCanceled -= ResetUI;
    }

    /// <summary>
    /// ジャンプチャージUIの更新処理
    /// </summary>
    private void UpdateGauge(float value)
    {
        gauge.fillAmount = value;
    }

    /// <summary>
    /// ジャンプゲージ満タン時の処理
    /// </summary>
    private void SetReady(bool isReady)
    {
        readyEffect.SetActive(isReady);
    }

    /// <summary>
    /// ジャンプゲージリセット時の処理
    /// </summary>
    private void ResetUI()
    {
        gauge.fillAmount = 0f;
        readyEffect.SetActive(false);
    }
}
