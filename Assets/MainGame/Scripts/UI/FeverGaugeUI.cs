using UnityEngine;

public class FeverGaugeUI : MonoBehaviour
{
    /// <summary>
    /// マスターデータ
    /// </summary>
    [SerializeField]
    private GameMasterData gameMasterData;
    /// <summary>
    /// シェーダー側の進捗プロパティ名
    /// </summary>
    [SerializeField]
    private string shaderPropertyName = "_FeverProgress";

    /// <summary>
    /// 正規化に使う最大ゲージ値
    /// </summary>
    private float feverGaugeMax;
    /// <summary>
    /// フィーバーゲージ管理
    /// </summary>
    private FeverGaugeManager feverGaugeManager;
    /// <summary>
    /// シェーダーに値を送る対象マテリアル
    /// </summary>
    private Material targetMaterial;

    private void Awake()
    {
        feverGaugeManager = FeverGaugeManager.Instance;
        feverGaugeMax = gameMasterData.FeverGaugeAmount;
        targetMaterial = GetComponent<Renderer>()?.material;

        SetParameter();
    }

    private void Update()
    {
        SetParameter();
    }

    private void SetParameter()
    {
        if (targetMaterial == null)
        {
            return;
        }

        var maxValue = Mathf.Max(0.0001f, feverGaugeMax);
        var normalized = Mathf.Clamp01(feverGaugeManager.FeverGauge / maxValue);

        targetMaterial.SetFloat(shaderPropertyName, normalized);
    }
}
