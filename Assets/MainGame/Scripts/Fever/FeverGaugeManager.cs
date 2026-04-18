using System.Collections;
using UnityEngine;

public partial class FeverGaugeManager : Singleton<FeverGaugeManager>
{
    /// <summary>
    /// マスターデータ
    /// </summary>
    [SerializeField]
    private GameMasterData gameMasterData;
    /// <summary>
    /// フィーバー開始演出中に表示するパネルUI
    /// </summary>
    [SerializeField]
    private GameObject feverStartPanelUI;
    /// <summary>
    /// フィーバー開始演出の停止時間（秒）
    /// </summary>
    [SerializeField]
    private float feverStartFreezeDuration = 1.0f;
    /// <summary>
    /// フィーバーエフェクト点滅間隔（秒）
    /// </summary>
    [SerializeField]
    private float feverEffectBlinkInterval = 0.08f;

    private float feverGauge;
    /// <summary>
    /// フィーバーゲージの現在値
    /// </summary>
    public float FeverGauge
    {
        get => feverGauge;
        set
        {
            if (IsFever || isFeverStarting) return;

            feverGauge = Mathf.Max(0.0f, value);
            feverGauge = Mathf.Min(gameMasterData.FeverGaugeAmount, feverGauge);

            if (CanStartFever())
            {
                StartFever();
            }
        }
    }

    /// <summary>
    /// フィーバー中かどうか
    /// </summary>
    public bool IsFever { get; private set; } = false;

    /// <summary>
    /// フィーバー開始演出中かどうか
    /// </summary>
    private bool isFeverStarting = false;

    /// <summary>
    /// フィーバー開始演出コルーチン
    /// </summary>
    private Coroutine feverStartRoutine;

    /// <summary>
    /// 一時停止前のTimeScale
    /// </summary>
    private float cachedTimeScale = 1.0f;

    // 何回も通知を送るので、プレイヤーコントローラーの参照を保持しておく
    private PlayerController playerController;

    private void Awake()
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<PlayerController>();
        }
    }

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
        if (isFeverStarting || IsFever)
        {
            return;
        }

        if (feverStartRoutine != null)
        {
            StopCoroutine(feverStartRoutine);
        }

        feverStartRoutine = StartCoroutine(FeverStartSequence());
    }

    /// <summary>
    /// フィーバー開始演出
    /// </summary>
    private IEnumerator FeverStartSequence()
    {
        isFeverStarting = true;

        if (playerController == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerController = player.GetComponent<PlayerController>();
            }
        }

        if (feverStartPanelUI != null)
        {
            feverStartPanelUI.SetActive(true);
        }

        cachedTimeScale = Time.timeScale;
        Time.timeScale = 0.0f;

        var duration = Mathf.Max(0.0f, feverStartFreezeDuration);
        var interval = Mathf.Max(0.02f, feverEffectBlinkInterval);

        var elapsed = 0.0f;
        var visible = false;
        if (playerController != null)
        {
            playerController.SetFeverEffectVisible(false);
        }

        while (elapsed < duration)
        {
            visible = !visible;
            if (playerController != null)
            {
                playerController.SetFeverEffectVisible(visible);
            }

            var wait = Mathf.Min(interval, duration - elapsed);
            yield return new WaitForSecondsRealtime(wait);
            elapsed += wait;
        }

        if (playerController != null)
        {
            playerController.SetFeverEffectVisible(true);
        }

        Time.timeScale = cachedTimeScale;

        if (feverStartPanelUI != null)
        {
            feverStartPanelUI.SetActive(false);
        }

        IsFever = true;
        isFeverStarting = false;
        feverStartRoutine = null;

        if (playerController != null)
        {
            playerController.StartFever();
        }
    }

    private void EndFever()
    {
        if (!IsFever)
        {
            return;
        }

        IsFever = false;
        feverGauge = 0.0f; // Player死亡によるリセットがあるのでこれで戻す

        if (playerController != null)
        {
            playerController.EndFever();
        }
    }

    private void OnDisable()
    {
        if (feverStartRoutine != null)
        {
            StopCoroutine(feverStartRoutine);
            feverStartRoutine = null;
        }

        isFeverStarting = false;
        Time.timeScale = 1.0f;

        if (feverStartPanelUI != null)
        {
            feverStartPanelUI.SetActive(false);
        }

        if (IsFever)
        {
            EndFever();
        }
    }
}
