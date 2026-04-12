using System.Collections.Generic;

/// <summary>
/// 死亡をまたいでルートを記憶するシングルトン。
/// </summary>
public class RunMemory : Singleton<RunMemory>
{
    /// <summary>記憶済みルート（死亡前に走ったフロアの列）</summary>
    public List<MapRecord> RecordedPath { get; private set; } = new();

    /// <summary>true のあいだは RecordedPath を再生する</summary>
    public bool IsReplayMode { get; private set; } = false;

    /// <summary>死亡時に MapGenerator から呼ぶ</summary>
    public void SaveRun(List<MapRecord> path)
    {
        // 前回ルートに今回走った新規分を追加（記憶が延伸していく）
        RecordedPath = new List<MapRecord>(path);
        IsReplayMode = (RecordedPath.Count > 0);
    }

    /// <summary>記憶ルートを再生しきったら MapGenerator から呼ぶ</summary>
    public void ExitReplayMode() => IsReplayMode = false;

    public void ClearMemory()
    {
        RecordedPath.Clear();
        IsReplayMode = false;
    }
}