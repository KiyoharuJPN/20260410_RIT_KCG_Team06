using TMPro;
using UnityEngine;

public class TimerUI : MonoBehaviour
{
    /// <summary>
    /// 表示用テキスト
    /// </summary>
    private TextMeshProUGUI text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();

        text.text = "00:00";
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"{(int)(TimeManager.Instance.TimeLimit / 60):00}:{(int)(TimeManager.Instance.TimeLimit % 60):00}";
    }
}
