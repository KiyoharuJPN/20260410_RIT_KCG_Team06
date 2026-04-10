using DG.Tweening;
using UnityEngine;

public class RepeatScaleUI : MonoBehaviour
{
    /// <summary>
    /// 最小スケール
    /// </summary>
    [SerializeField]
    private float minScale = 1.0f;
    /// <summary>
    /// 最大スケール
    /// </summary>
    [SerializeField]
    private float maxScale = 1.5f;
    /// <summary>
    /// Tweenの時間
    /// </summary>
    [SerializeField]
    private float tweenTime= 1.0f;
    /// <summary>
    /// Tweenの仕方
    /// </summary>
    [SerializeField]
    private Ease tweenEase = Ease.InOutSine;

    /// <summary>
    /// rectTransformのキャッシュ
    /// </summary>
    private RectTransform rectTransform;
    /// <summary>
    /// 現在保存中のTween
    /// </summary>
    private Tween tween;

    private void Awake()
    {
        rectTransform = transform as RectTransform;
        ApplyMinScale();
    }

    private void OnEnable()
    {
        StartScaleTween();
    }

    private void OnDisable()
    {
        KillTween();
    }

    private void OnDestroy()
    {
        KillTween();
    }

    private void StartScaleTween()
    {
        KillTween();

        if (rectTransform == null)
        {
            rectTransform = transform as RectTransform;
        }

        ApplyMinScale();

        tween = rectTransform
            .DOScale(maxScale, tweenTime)
            .SetEase(tweenEase)
            .SetLoops(-1, LoopType.Yoyo);
    }

    private void ApplyMinScale()
    {
        if (rectTransform == null)
        {
            return;
        }

        rectTransform.localScale = Vector3.one * minScale;
    }

    private void KillTween()
    {
        if (tween != null && tween.IsActive())
        {
            tween.Kill();
        }

        tween = null;
    }
}
