using AudioName;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    // score出力用
    [SerializeField]
    TMP_Text scoreText;
    // kill数出力用
    [SerializeField]
    TMP_Text killText;
    // coin数出力用
    [SerializeField]
    TMP_Text coinText;
    // 次のステージへ遷移する旨のテキスト(アニメーションがあったらここで作る)
    [SerializeField]
    TMP_Text nextStage;
    /// <summary>
    /// 最初の静止時間
    /// </summary>
    [SerializeField]
    private float freezeTime;

    /// <summary>
    /// 現在時間
    /// </summary>
    private float nowTime;


    private void Awake()
    {
        // 音楽の再生(今までの音楽をストップ)
        AudioManager.Instance.StopBGM(BGMName.MAIN_GAME_BGM2_NAME);
        AudioManager.Instance.StopBGM(BGMName.MAIN_GAME_BGM_NAME);

        AudioManager.Instance.PlayLoopBGM(BGMName.RESULT_BGM_NAME);

        // ResultManagerからスコア、キル数、コイン数を取得してUIに反映
        scoreText.text = ResultManager.score.ToString() == null ? "0" : ResultManager.score.ToString();
        killText.text = ResultManager.killCount.ToString() == null ? "0" : ResultManager.killCount.ToString();
        coinText.text = ResultManager.coinCount.ToString() == null ? "0" : ResultManager.coinCount.ToString();
        AudioManager.Instance.PlaySE(SEName.RESULT_SE_NAME);
    }


    void Update()
    {
        nowTime += Time.deltaTime;

        if (Keyboard.current.enterKey.wasPressedThisFrame && nowTime >= freezeTime)
        {
            SceneChangeManager.Instance
                .SetFadeDuration(0.4f, 0.3f)
                .SetFadeEase(Ease.OutCubic, Ease.InCubic)
                .SetFadeColor(Color.black)
                .ChangeScene("TitleScene");
            AudioManager.Instance.StopBGM(BGMName.RESULT_BGM_NAME);
        }
    }
}
