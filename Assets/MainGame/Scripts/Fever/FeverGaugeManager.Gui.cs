using UnityEngine;

public partial class FeverGaugeManager
{
    private void OnGUI()
    {
        const float x = 20.0f;
        const float y = 20.0f;
        const float width = 220.0f;
        const float height = 40.0f;

        if (GUI.Button(new Rect(x, y, width, height), "FeverGauge を最大にする"))
        {
            FeverGauge = gameMasterData.FeverGaugeAmount;
        }
    }
}
