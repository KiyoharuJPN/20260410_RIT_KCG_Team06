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
        // ResultManagerからスコア、キル数、コイン数を取得してUIに反映
        scoreText.text = "50";// ResultManager.score.ToString();
        killText.text = "Kills :                            5";// + ResultManager.killCount.ToString();
        coinText.text = "Coins :                            5";// + ResultManager.coinCount.ToString();
    }


    void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            // 次のステージへ遷移
            SceneManager.LoadScene("TitleScene");
        }
    }
}
