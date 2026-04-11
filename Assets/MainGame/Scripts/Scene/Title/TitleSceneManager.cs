using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleSceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlayLoopBGM(AudioName.BGMName.TITLE_BGM_NAME);
    }

    private void Update()
    {
        if (Keyboard.current.enterKey.wasPressedThisFrame)
        {
            SceneChangeManager.Instance
                .SetFadeDuration(0.4f, 0.3f)
                .SetFadeEase(Ease.OutCubic, Ease.InCubic)
                .SetFadeColor(Color.black)
                .ChangeScene("GameScene");
        }
    }
}
