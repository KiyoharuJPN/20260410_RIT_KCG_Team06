using DG.Tweening;
using UnityEngine;

public class TimeManager : Singleton<TimeManager>
{
    /// <summary>
    /// 時間制限
    /// </summary>
    [SerializeField]
    private float timeLimit = 120;
    public float TimeLimit => timeLimit;

    private void Update()
    {
        timeLimit -= Time.deltaTime;
        if (timeLimit <= 0)
        {
            timeLimit = 0;
            GameOver();
        }
    }

    private void GameOver()
    {
        SceneChangeManager.Instance
            .SetFadeDuration(0.4f, 0.3f)
            .SetFadeEase(Ease.OutCubic, Ease.InCubic)
            .SetFadeColor(Color.black)
            .ChangeScene("ResultScene");
    }
}
