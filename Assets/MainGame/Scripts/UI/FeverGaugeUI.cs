using UnityEngine;
using UnityEngine.UI;

public class FeverGaugeUI : MonoBehaviour
{
    /// <summary>
    /// マスターデータ
    /// </summary>
    [SerializeField]
    private GameMasterData gameMasterData;
    /// <summary>
    /// 分割ゲージ用スプライト配列
    /// </summary>
    [SerializeField]
    private Sprite[] gaugeSprites;
    /// <summary>
    /// 表示先のImage
    /// </summary>
    [SerializeField]
    private Image targetImage;

    /// <summary>
    /// 正規化に使う最大ゲージ値
    /// </summary>
    private float feverGaugeMax;
    /// <summary>
    /// フィーバーゲージ管理
    /// </summary>
    private FeverGaugeManager feverGaugeManager;

    private void Awake()
    {
        feverGaugeManager = FeverGaugeManager.Instance;
        feverGaugeMax = gameMasterData != null ? gameMasterData.FeverGaugeAmount : 1.0f;

        if (targetImage == null)
        {
            targetImage = GetComponent<Image>();
        }

        SetGaugeSprite();
    }

    private void Update()
    {
        SetGaugeSprite();
    }

    /// <summary>
    /// フィーバーゲージ値から段階スプライトを設定する
    /// </summary>
    private void SetGaugeSprite()
    {
        if (targetImage == null || gaugeSprites == null || gaugeSprites.Length == 0 || feverGaugeManager == null)
        {
            return;
        }

        var maxValue = Mathf.Max(0.0001f, feverGaugeMax);
        var normalized = Mathf.Clamp01(feverGaugeManager.FeverGauge / maxValue);

        var segmentCount = gaugeSprites.Length;
        var filledSegment = Mathf.CeilToInt(normalized * segmentCount);

        if (filledSegment <= 0)
        {
            targetImage.sprite = gaugeSprites[0];
            return;
        }

        var index = Mathf.Clamp(filledSegment - 1, 0, segmentCount - 1);
        targetImage.sprite = gaugeSprites[index];
    }
}
