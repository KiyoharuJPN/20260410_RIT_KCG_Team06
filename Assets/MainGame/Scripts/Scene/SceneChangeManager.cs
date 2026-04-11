using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChangeManager : Singleton<SceneChangeManager>
{
    /// <summary>
    /// フェードアウト（0.0 -> 1.0）にかける時間（秒）
    /// </summary>
    private float fadeOutDuration = 0.35f;
    /// <summary>
    /// フェードイン（1.0 -> 0.0）にかける時間（秒）
    /// </summary>
    private float fadeInDuration = 0.35f;
    /// <summary>
    /// フェードアウト時のEase
    /// </summary>
    private Ease fadeOutEase = Ease.InOutSine;
    /// <summary>
    /// フェードイン時のEase
    /// </summary>
    private Ease fadeInEase = Ease.InOutSine;
    /// <summary>
    /// フェードパネルの色
    /// </summary>
    private Color fadeColor = Color.black;
    /// <summary>
    /// フェード用Canvasのソート順
    /// </summary>
    private int sortingOrder = 9999;
    /// <summary>
    /// フェード中にRaycastをブロックするか
    /// </summary>
    private bool blockRaycasts = true;

    /// <summary>
    /// フェードパネルのCanvasGroup
    /// </summary>
    private CanvasGroup fadeCanvasGroup;
    /// <summary>
    /// フェードパネルのImage
    /// </summary>
    private Image fadeImage;
    /// <summary>
    /// 現在再生中のフェードTween
    /// </summary>
    private Tween fadeTween;
    /// <summary>
    /// シーン遷移処理中かどうか
    /// </summary>
    private bool isChanging;

    /// <summary>
    /// シーンに置かずに生成させる
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnGameLoad()
    {
        var existing = FindFirstObjectByType<SceneChangeManager>();
        if (existing != null)
        {
            return;
        }

        var gameObject = new GameObject("SceneChangeManager");
        DontDestroyOnLoad(gameObject);
        gameObject.AddComponent<SceneChangeManager>();
    }

    /// <summary>
    /// 初期化時にフェードUIを生成する
    /// </summary>
    private void Start()
    {
        CreateFadeCanvasIfNeeded();
        SetFadeAlpha(0.0f);
        ApplyVisualSettings();
    }

    /// <summary>
    /// フェード時間を設定する
    /// </summary>
    public SceneChangeManager SetFadeDuration(float outDuration, float inDuration)
    {
        fadeOutDuration = Mathf.Max(0.0f, outDuration);
        fadeInDuration = Mathf.Max(0.0f, inDuration);
        return this;
    }

    /// <summary>
    /// フェードEaseを設定する
    /// </summary>
    public SceneChangeManager SetFadeEase(Ease outEase, Ease inEase)
    {
        fadeOutEase = outEase;
        fadeInEase = inEase;
        return this;
    }

    /// <summary>
    /// フェード色を設定する
    /// </summary>
    public SceneChangeManager SetFadeColor(Color color)
    {
        fadeColor = color;
        ApplyVisualSettings();
        return this;
    }

    /// <summary>
    /// フェード付きでシーン変更する
    /// </summary>
    public void ChangeScene(string sceneName)
    {
        if (isChanging)
        {
            return;
        }

        StartCoroutine(ChangeSceneRoutine(sceneName));
    }

    private IEnumerator ChangeSceneRoutine(string sceneName)
    {
        isChanging = true;
        CreateFadeCanvasIfNeeded();
        ApplyVisualSettings();

        yield return FadeTo(1.0f, fadeOutDuration, fadeOutEase);

        var loadOp = SceneManager.LoadSceneAsync(sceneName);
        while (!loadOp.isDone)
        {
            yield return null;
        }

        yield return FadeTo(0.0f, fadeInDuration, fadeInEase);

        isChanging = false;
    }

    private IEnumerator FadeTo(float targetAlpha, float duration, Ease ease)
    {
        KillFadeTween();

        if (duration <= 0.0f)
        {
            SetFadeAlpha(targetAlpha);
            yield break;
        }

        fadeTween = fadeCanvasGroup.DOFade(targetAlpha, duration).SetEase(ease);
        yield return fadeTween.WaitForCompletion();
    }

    private void CreateFadeCanvasIfNeeded()
    {
        if (fadeCanvasGroup != null)
        {
            return;
        }

        var canvasObject = new GameObject("SceneFadeCanvas");
        canvasObject.transform.SetParent(transform, false);

        var canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = sortingOrder;

        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        var panelObject = new GameObject("FadePanel");
        panelObject.transform.SetParent(canvasObject.transform, false);

        var rect = panelObject.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.offsetMin = Vector2.zero;
        rect.offsetMax = Vector2.zero;

        fadeImage = panelObject.AddComponent<Image>();
        fadeImage.color = fadeColor;

        fadeCanvasGroup = panelObject.AddComponent<CanvasGroup>();
        fadeCanvasGroup.blocksRaycasts = blockRaycasts;
        fadeCanvasGroup.interactable = false;
    }

    private void ApplyVisualSettings()
    {
        if (fadeImage != null)
        {
            fadeImage.color = fadeColor;
        }

        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.blocksRaycasts = blockRaycasts;
        }

        if (fadeCanvasGroup != null)
        {
            var canvas = fadeCanvasGroup.GetComponentInParent<Canvas>();
            if (canvas != null)
            {
                canvas.sortingOrder = sortingOrder;
            }
        }
    }

    private void SetFadeAlpha(float alpha)
    {
        if (fadeCanvasGroup == null)
        {
            return;
        }

        fadeCanvasGroup.alpha = Mathf.Clamp01(alpha);
    }

    private void KillFadeTween()
    {
        if (fadeTween != null && fadeTween.IsActive())
        {
            fadeTween.Kill();
        }

        fadeTween = null;
    }

    private void OnDisable()
    {
        KillFadeTween();
    }
}
