using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    [SerializeField]
    TMP_Text scoreText;
    [SerializeField]
    TMP_Text killText;
    [SerializeField]
    TMP_Text coinText;
    [SerializeField]
    TMP_Text nextStage;


    private void Awake()
    {
        // ResultManagerからスコア、キル数、コイン数を取得してUIに反映
        scoreText.text = "50";// ResultManager.score.ToString();
        killText.text = "Kills :                            5";// + ResultManager.killCount.ToString();
        coinText.text = "Coins :                            5";// + ResultManager.coinCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            // 次のステージへ遷移
            SceneManager.LoadScene("TitleScene");
        }
    }
}
