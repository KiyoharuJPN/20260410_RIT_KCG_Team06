using UnityEngine;
using AudioName;

public class AudioLoader : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        AudioManager.Instance.LoadBGM(BGMName.MAIN_GAME_BGM_NAME, "Audio/BGM/ORARA_BGM_Stage_Atype");
        AudioManager.Instance.LoadBGM(BGMName.MAIN_GAME_BGM2_NAME, "Audio/BGM/ORARA_BGM_Stage_Btype");
        AudioManager.Instance.LoadBGM(BGMName.TITLE_BGM_NAME, "Audio/BGM/ORARA_BGM_Title");
        AudioManager.Instance.LoadBGM(BGMName.RESULT_BGM_NAME, "Audio/BGM/ORARA_BGM_Result");

        AudioManager.Instance.LoadSE(SEName.COIN_SE_NAME, "Audio/SE/SE_Coin");
        AudioManager.Instance.LoadSE(SEName.DAMAGE_SE_NAME, "Audio/SE/SE_Damage");
        AudioManager.Instance.LoadSE(SEName.JUMP_SE_NAME, "Audio/SE/SE_Jump");
        AudioManager.Instance.LoadSE(SEName.PUNCH_SE_NAME, "Audio/SE/SE_Punch");
        AudioManager.Instance.LoadSE(SEName.RESULT_SE_NAME, "Audio/SE/SE_Result");
        AudioManager.Instance.LoadSE(SEName.SYSOK_SE_NAME, "Audio/SE/SE_SysOK");
    }

    public void Final()
    {
        AudioManager.Instance.UnloadBGM(BGMName.MAIN_GAME_BGM_NAME);
        AudioManager.Instance.UnloadBGM(BGMName.MAIN_GAME_BGM2_NAME);
        AudioManager.Instance.UnloadBGM(BGMName.TITLE_BGM_NAME);
        AudioManager.Instance.UnloadBGM(BGMName.RESULT_BGM_NAME);

        AudioManager.Instance.UnloadSE(SEName.COIN_SE_NAME);
        AudioManager.Instance.UnloadSE(SEName.DAMAGE_SE_NAME);
        AudioManager.Instance.UnloadSE(SEName.JUMP_SE_NAME);
        AudioManager.Instance.UnloadSE(SEName.PUNCH_SE_NAME);
        AudioManager.Instance.UnloadSE(SEName.RESULT_SE_NAME);
        AudioManager.Instance.UnloadSE(SEName.SYSOK_SE_NAME);
    }
}

namespace AudioName
{
    public static class BGMName
    {
        public static string MAIN_GAME_BGM_NAME = "MainGame";
        public static string MAIN_GAME_BGM2_NAME = "MainGame2";
        public static string TITLE_BGM_NAME = "Title";
        public static string RESULT_BGM_NAME = "Result";
    }

    public static class SEName
    {
        public static string COIN_SE_NAME = "Coin";
        public static string DAMAGE_SE_NAME = "Damage";
        public static string JUMP_SE_NAME = "Jump";
        public static string PUNCH_SE_NAME = "Punch";
        public static string RESULT_SE_NAME = "Result";
        public static string SYSOK_SE_NAME = "SysOk";
    }
}