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
    }


    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SceneChangeManager.Instance
                .SetFadeDuration(0.4f, 0.3f)
                .SetFadeEase(Ease.OutCubic, Ease.InCubic)
                .SetFadeColor(Color.black)
                .ChangeScene("TitleScene");
        }
    }
}
