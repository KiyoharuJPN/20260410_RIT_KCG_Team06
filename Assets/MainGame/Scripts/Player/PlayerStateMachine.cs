using UnityEngine;

/// <summary>
/// プレイヤーの状態を管理するクラス
/// </summary>
public class PlayerStateMachine : MonoBehaviour
{
    /// <summary>
    /// プレイヤーの状態を表すプロパティ
    /// </summary>
    public PlayerState CurrentState { get; private set; }

    /// <summary>
    /// プレイヤーの状態を変更するメソッド
    /// </summary>
    public void ChangeState(PlayerState newState)
    {
        // 状態が変わらない場合は何もしない
        if (CurrentState == newState)
        {
            return;
        }

        // 状態が変わる場合は新しい状態に更新する
        CurrentState = newState;
    }
}
