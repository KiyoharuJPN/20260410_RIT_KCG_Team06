using UnityEngine;
using System.Collections.Generic;

public class AudioManager : Singleton<AudioManager>
{
    /// <summary>
    /// 再生中のBGMを格納する辞書
    /// </summary>
    private Dictionary<string, AudioSource> bgmSources = new();
    /// <summary>
    /// ロードしたBGMを格納する辞書
    /// </summary>
    private Dictionary<string, AudioClip> bgmClips = new();
    /// <summary>
    /// 再生中のSEを格納する辞書
    /// </summary>
    private Dictionary<string, AudioSource> seSources = new();
    /// <summary>
    /// ロードしたSEを格納する辞書
    /// </summary>
    private Dictionary<string, AudioClip> seClips = new();

    /// <summary>
    /// 現在のBGMの音量
    /// </summary>
    public float NowBGMVolume { get; private set; } = 1.0f;
    /// <summary>
    /// 現在のSEの音量
    /// </summary>
    public float NowSEVolume { get; private set; } = 1.0f;
    /// <summary>
    /// 解放時のために保持するローダー
    /// </summary>
    public AudioLoader Loader { get; set; }

    /// <summary>
    /// シーンに置かずに生成させる
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void OnGameLoad()
    {
        var gameObject = new GameObject("AudioManager");
        DontDestroyOnLoad(gameObject);

        var manager = gameObject.AddComponent<AudioManager>();
        manager.Loader = gameObject.AddComponent<AudioLoader>();
    }

    private void OnDestroy()
    {
        Loader.Final();
    }

    /// <summary>
    /// BGMをリソースフォルダからロードする
    /// </summary>
    /// <param name="clipName">オーディオファイルの名前</param>
    /// <param name="clipPath">オーディオファイルのパス(拡張子なし)</param>
    public void LoadBGM(string clipName, string clipPath)
    {
        // 指定のファイルを既にロードしているならリターン
        if (bgmClips.ContainsKey(clipName)) return;

        AudioClip clip = Resources.Load<AudioClip>(clipPath);

        if (clip)
        {
            bgmClips[clipName] = clip;
        }
        else
        {
            Debug.LogWarning($"{clipName}: ファイルが見つかりませんでした");
        }
    }

    /// <summary>
    /// BGMを一度だけ再生する
    /// </summary>
    /// <param name="clipName">オーディオファイルの名前</param>
    public void PlayBGM(string clipName)
    {
        if (bgmClips.TryGetValue(clipName, out AudioClip clip))
        {
            // 既存のAudioSourceを再利用するか、新規作成
            if (!bgmSources.TryGetValue(clipName, out AudioSource audioSource))
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = clip;
                bgmSources[clipName] = audioSource;
            }

            // 新しく入ってきたBGMの音量も変更
            ChangeVolumeAllBGM(NowBGMVolume);

            audioSource.PlayOneShot(audioSource.clip);
        }
        else
        {
            Debug.LogWarning($"{clipName}: ファイルがロードされていないため再生できません");
        }
    }

    /// <summary>
    /// BGMをループ再生する
    /// </summary>
    /// <param name="clipName">オーディオファイルの名前</param>
    public void PlayLoopBGM(string clipName)
    {
        if (bgmClips.TryGetValue(clipName, out AudioClip clip))
        {
            // 既存のAudioSourceを再利用するか、無いなら新規作成
            if (!bgmSources.TryGetValue(clipName, out AudioSource audioSource))
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = clip;
                bgmSources[clipName] = audioSource;
            }

            // ループ再生にする
            audioSource.loop = true;

            // 新しく入ってきたBGMの音量も変更
            ChangeVolumeAllBGM(NowBGMVolume);

            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"{clipName}: ファイルがロードされていないため再生できません");
        }
    }

    /// <summary>
    /// BGMを停止し、AudioSourceを破棄する
    /// </summary>
    /// <param name="clipName">オーディオファイルの名前</param>
    public void StopBGM(string clipName)
    {
        if (bgmSources.TryGetValue(clipName, out AudioSource audioSource))
        {
            audioSource.Stop();

            bgmSources.Remove(clipName);
            Destroy(audioSource);
        }
        else
        {
            Debug.LogWarning($"{clipName}: 再生中のオーディオが見つかりません");
        }
    }

    /// <summary>
    /// BGMをアンロードしてメモリを解放
    /// </summary>
    /// <param name="clipName">オーディオファイルの名前</param>
    public void UnloadBGM(string clipName)
    {
        if (bgmClips.ContainsKey(clipName))
        {
            // もし再生中なら止める
            if (bgmSources.ContainsKey(clipName))
            {
                StopBGM(clipName);
            }

            Resources.UnloadAsset(bgmClips[clipName]);
            bgmClips.Remove(clipName);
        }
        else
        {
            Debug.LogWarning($"{clipName}: ファイルはロードされていません");
        }
    }

    /// <summary>
    /// 全てのBGMの音量を変更する
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeVolumeAllBGM(float volume)
    {
        // 更新 
        NowBGMVolume = volume;
        if (bgmSources.Count == 0) return;

        // 全体に変更をかける
        foreach (var bgm in bgmSources.Values)
        {
            bgm.volume = volume;
        }
    }

    /// <summary>
    /// SEをリソースフォルダからロードする
    /// </summary>
    /// <param name="clipName">オーディオファイルの名前</param>
    /// <param name="clipPath">オーディオファイルのパス(拡張子なし)</param>
    public void LoadSE(string clipName, string clipPath)
    {
        // 指定のファイルを既にロードしているならリターン
        if (seClips.ContainsKey(clipName)) return;

        AudioClip clip = Resources.Load<AudioClip>(clipPath);

        if (clip)
        {
            seClips[clipName] = clip;
        }
        else
        {
            Debug.LogWarning($"{clipName}: ファイルが見つかりませんでした");
        }
    }

    /// <summary>
    /// SEを一度だけ再生する
    /// </summary>
    /// <param name="clipName">オーディオファイルの名前</param>
    public void PlaySE(string clipName)
    {
        if (seClips.TryGetValue(clipName, out AudioClip clip))
        {
            // 既存のAudioSourceを再利用するか、新規作成
            if (!seSources.TryGetValue(clipName, out AudioSource audioSource))
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = clip;
                seSources[clipName] = audioSource;
            }

            // 新しく入ってきたSEの音量も変更
            ChangeVolumeAllSE(NowSEVolume);

            audioSource.PlayOneShot(audioSource.clip);
        }
        else
        {
            Debug.LogWarning($"{clipName}: ファイルがロードされていないため再生できません");
        }
    }

    /// <summary>
    /// SEをループ再生する
    /// </summary>
    /// <param name="clipName">オーディオファイルの名前</param>
    public void PlayLoopSE(string clipName)
    {
        if (seClips.TryGetValue(clipName, out AudioClip clip))
        {
            // 既存のAudioSourceを再利用するか、無いなら新規作成
            if (!seSources.TryGetValue(clipName, out AudioSource audioSource))
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.clip = clip;
                seSources[clipName] = audioSource;
            }

            // ループ再生にする
            audioSource.loop = true;

            // 新しく入ってきたSEの音量も変更
            ChangeVolumeAllSE(NowSEVolume);

            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"{clipName}: ファイルがロードされていないため再生できません");
        }
    }

    /// <summary>
    /// SEを停止し、AudioSourceを破棄する
    /// </summary>
    /// <param name="clipName">オーディオファイルの名前</param>
    public void StopSE(string clipName)
    {
        if (seSources.TryGetValue(clipName, out AudioSource audioSource))
        {
            audioSource.Stop();

            seSources.Remove(clipName);
            Destroy(audioSource);
        }
        else
        {
            Debug.LogWarning($"{clipName}: 再生中のオーディオが見つかりません");
        }
    }

    /// <summary>
    /// SEをアンロードしてメモリを解放
    /// </summary>
    /// <param name="clipName">オーディオファイルの名前</param>
    public void UnloadSE(string clipName)
    {
        if (seClips.ContainsKey(clipName))
        {
            // もし再生中なら止める
            if (seSources.ContainsKey(clipName))
            {
                StopBGM(clipName);
            }

            Resources.UnloadAsset(seClips[clipName]);
            seClips.Remove(clipName);
        }
        else
        {
            Debug.LogWarning($"{clipName}: ファイルはロードされていません");
        }
    }

    /// <summary>
    /// 全てのSEの音量を変更する
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeVolumeAllSE(float volume)
    {
        // 更新 
        NowSEVolume = volume;
        if (seSources.Count == 0) return;

        // 全体に変更をかける
        foreach (var se in seSources.Values)
        {
            se.volume = volume;
        }
    }

    /// <summary>
    /// 指定したBGMが現在再生中かどうか
    /// </summary>
    /// <param name="clipName">オーディオファイルの名前</param>
    /// <returns>再生中なら true、そうでなければ false</returns>
    public bool IsPlaying(string clipName)
    {
        if (bgmSources.TryGetValue(clipName, out AudioSource bgm))
        {
            return bgm.isPlaying;
        }
        if (seSources.TryGetValue(clipName, out AudioSource se))
        {
            return se.isPlaying;
        }

        return false;
    }
}