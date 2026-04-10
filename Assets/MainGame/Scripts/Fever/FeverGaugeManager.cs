using UnityEngine;

public partial class FeverGaugeManager : Singleton<FeverGaugeManager>
{
    /// <summary>
    /// マスターデータ
    /// </summary>
    [SerializeField]
    private GameMasterData gameMasterData;

    private float feverGauge;
    /// <summary>
    /// フィーバーゲージの現在値
    /// </summary>
    public float FeverGauge
    {
        get => feverGauge;
        set
        {
            feverGauge = Mathf.Max(0.0f, value);

            if (!IsFever && CanStartFever())
            {
                StartFever();
            }
        }
    }
    /// <summary>
    /// フィーバー中かどうか
    /// </summary>
    public bool IsFever { get; private set; } = false;

    private void Update()
    {
        if (!IsFever || gameMasterData == null)
        {
            return;
        }

        feverGauge -= gameMasterData.FeverDecreasePerSecondDuringFever * Time.deltaTime;
        if (feverGauge <= 0.0f)
        {
            feverGauge = 0.0f;
            EndFever();
        }
    }

    private bool CanStartFever()
    {
        return gameMasterData != null && feverGauge >= gameMasterData.FeverGaugeAmount;
    }

    private void StartFever()
    {
        IsFever = true;
    }

    private void EndFever()
    {
        IsFever = false;
    }

    private void OnDisable()
    {
        EndFever();
    }
}
