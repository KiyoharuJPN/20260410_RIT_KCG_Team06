using UnityEngine;

/// <summary>
/// 汎用的シングルトンパターン基底クラス
/// </summary>
/// <typeparam name="T">クラス型を設定</typeparam>
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    /// <summary>
    /// クラスインスタンスの保存用
    /// </summary>
    private static T _instance;

    /// <summary>
    /// クラスインスタンスを受け取るプロパティ
    /// </summary>
    public static T Instance
    {
        // インスタンスの設定
        get
        {
            // インスタンスが既に設定されているなら
            if (_instance) return _instance;

            // インスタンスに同じ型のオブジェクトを探してきて代入
            _instance = FindFirstObjectByType<T>();

            // 設定できたらそのまま返す
            if (_instance) return _instance;

            // インスタンスが設定できなかったらエラー処理
            Debug.LogError(typeof(T) + "型のオブジェクトは見つかりませんでした");
            return null;
        }
    }

    private void Awake()
    {
        // インスタンスが既にあり、自分じゃなかったら
        if (_instance && _instance != this)
        {
            // 削除する
            Destroy(gameObject);
        }
        else
        {
            // 一応キャストする
            _instance = this as T;
        }
    }

    private void OnDestroy()
    {
        _instance = null;
    }
}