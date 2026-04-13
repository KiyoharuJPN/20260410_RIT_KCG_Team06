using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Video;

public class TitleSceneManager : MonoBehaviour
{
    /// <summary>
    /// タイトルロゴ本体
    /// </summary>
    [SerializeField]
    private GameObject titleLogoObject;
    /// <summary>
    /// 爆発演出用のVideoPlayer
    /// </summary>
    [SerializeField]
    private VideoPlayer startVideoPlayer;
    /// <summary>
    /// 遷移先シーン名
    /// </summary>
    [SerializeField]
    private string nextSceneName = "GameScene";
    /// <summary>
    /// ビデオを描画する
    /// </summary>
    [SerializeField]
    private RawImage videoImage;

    /// <summary>
    /// 開始処理を既に実行したかどうか
    /// </summary>
    private bool isStartTriggered = false;
    /// <summary>
    /// 動画のPrepare完了待ち中かどうか
    /// </summary>
    private bool isPreparingVideo = false;

    private void Start()
    {
        AudioManager.Instance.PlayLoopBGM(AudioName.BGMName.TITLE_BGM_NAME);

        if (startVideoPlayer != null)
        {
            videoImage.gameObject.SetActive(false);
            startVideoPlayer.loopPointReached += OnStartVideoFinished;
            startVideoPlayer.prepareCompleted += OnVideoPrepared;
            InitializeVideoPlayer();
        }
    }

    private void Update()
    {
        if (isStartTriggered)
        {
            return;
        }

        if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            isStartTriggered = true;
            PlayStartSequence();
        }
    }

    private void OnDestroy()
    {
        if (startVideoPlayer != null)
        {
            startVideoPlayer.loopPointReached -= OnStartVideoFinished;
            startVideoPlayer.prepareCompleted -= OnVideoPrepared;
        }
    }

    /// <summary>
    /// VideoPlayerを毎回初期化状態に戻す
    /// </summary>
    private void InitializeVideoPlayer()
    {
        if (startVideoPlayer == null)
        {
            return;
        }

        startVideoPlayer.isLooping = false;
        startVideoPlayer.skipOnDrop = false;
        startVideoPlayer.waitForFirstFrame = false;

        startVideoPlayer.Stop();
        startVideoPlayer.time = 0.0;
        startVideoPlayer.frame = 0;
        isPreparingVideo = false;
    }

    /// <summary>
    /// Enter押下時の開始演出を実行する
    /// </summary>
    private void PlayStartSequence()
    {
        if (titleLogoObject != null)
        {
            titleLogoObject.SetActive(false);
        }

        if (startVideoPlayer == null || startVideoPlayer.clip == null)
        {
            ChangeToNextScene();
            return;
        }

        if (!startVideoPlayer.gameObject.activeSelf)
        {
            startVideoPlayer.gameObject.SetActive(true);
        }

        InitializeVideoPlayer();

        AudioManager.Instance.PlaySE(AudioName.SEName.SYSOK_SE_NAME);
        isPreparingVideo = true;
        startVideoPlayer.Prepare();
    }

    /// <summary>
    /// 動画Prepare完了時に呼ばれる
    /// </summary>
    private void OnVideoPrepared(VideoPlayer player)
    {
        if (!isPreparingVideo)
        {
            return;
        }

        isPreparingVideo = false;
        player.Play();
        videoImage.gameObject.SetActive(true);
    }

    /// <summary>
    /// 動画の再生終了時に呼ばれる
    /// </summary>
    private void OnStartVideoFinished(VideoPlayer _)
    {
        ChangeToNextScene();
    }

    /// <summary>
    /// フェード付きで次シーンへ遷移する
    /// </summary>
    private void ChangeToNextScene()
    {
        SceneChangeManager.Instance
            .SetFadeDuration(0.4f, 0.3f)
            .SetFadeEase(Ease.OutCubic, Ease.InCubic)
            .SetFadeColor(Color.black)
            .ChangeScene(nextSceneName);
    }
}
