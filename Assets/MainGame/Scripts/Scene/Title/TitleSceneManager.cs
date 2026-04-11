using UnityEngine;

public class TitleSceneManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        AudioManager.Instance.PlayBGM(AudioName.BGMName.TITLE_BGM_NAME);
    }
}
