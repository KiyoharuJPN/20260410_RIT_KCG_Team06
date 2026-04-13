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
            if (IsFever) return;
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

    // 何回も通知を送るので、プレイヤーコントローラーの参照を保持しておく
    PlayerController playerController;
    private void Awake()=> playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

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
        playerController.StartFever();
    }

    private void EndFever()
    {
        IsFever = false;
        feverGauge = 0.0f; // Player死亡によるリセットがあるのでこれで戻す
        playerController.EndFever();
    }

    private void OnDisable()
    {
        EndFever();
    }
}
