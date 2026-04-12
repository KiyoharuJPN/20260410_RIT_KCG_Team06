using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorTest : MonoBehaviour
{
    [SerializeField, Tooltip("マップのプレハブ")]
    GameObject[] mapPrefabs;

    [SerializeField, Tooltip("敵のプレハブ")]
    public GameObject[] enemyPrefabs;

    [SerializeField, Tooltip("生成間隔")]
    int interval = 16;

    [SerializeField, Tooltip("生成開始高さ")]
    int startHeight = 32;

    [SerializeField, Tooltip("座標ずらし")]
    int genOffset = 20;

    List<GameObject> DynamicMaps = new List<GameObject>();

    GameObject player;
    Vector2 playerStartPos;

    int LastGenHeight = 0;

    // ─── 追加 ───────────────────────────────────────────
    // 今回の走行で生成したフロアの記録（死亡時に RunMemory へ渡す）
    List<MapRecord> thisRunPath = new List<MapRecord>();

    // 重放時に「次に再生するインデックス」
    int replayIndex = 0;
    // ────────────────────────────────────────────────────

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerStartPos = player.transform.position;

        LastGenHeight = startHeight;

        // ─── 追加：重放開始インデックスをリセット ───
        replayIndex = 0;
        thisRunPath.Clear();
        // ─────────────────────────────────────────────
    }

    void Update()
    {
        if (player.transform.position.y > LastGenHeight - genOffset)
        {
            LastGenHeight += interval;
            GenMap();
        }
    }

    void GenMap()
    {
        Vector3 spawnPos = new Vector3(
            0,
            startHeight + interval * DynamicMaps.Count,
            0
        );

        MapRecord record;
        var memory = RunMemory.Instance;

        // ─── 変更：重放 or ランダム分岐 ─────────────────
        if (memory != null
            && memory.IsReplayMode
            && replayIndex < memory.RecordedPath.Count)
        {
            // 記憶通りに再生（位置は今の spawnPos で再計算、Prefab のみ復元）
            record = memory.RecordedPath[replayIndex];
            record.position = spawnPos;   // 高さ計算は常に最新値を使う
            replayIndex++;

            if (replayIndex >= memory.RecordedPath.Count)
            {
                memory.ExitReplayMode();
                Debug.Log("[MapGenerator] 記憶ルート再生完了 → ランダム生成へ");
            }
        }
        else
        {
            // ランダム生成 → 今回のルートに記録する
            record = new MapRecord
            {
                prefabIndex = Random.Range(0, mapPrefabs.Length),
                position = spawnPos
            };
            thisRunPath.Add(record);
        }
        // ─────────────────────────────────────────────────

        // 生成・敵リセットは変わらず
        GameObject map = Instantiate(
            mapPrefabs[record.prefabIndex],
            record.position,
            Quaternion.identity,
            transform
        );
        map.GetComponent<EnemyGenerator>().ResetEnemy(enemyPrefabs);
        DynamicMaps.Add(map);
    }

    public void ResetMap()
    {
        foreach (GameObject map in DynamicMaps)
            map.GetComponent<EnemyGenerator>().ResetEnemy(enemyPrefabs);
    }

    // ─── 変更：死亡時に記憶を保存してからリセット ──────
    public void PlayerDead()
    {
        // 今回走った新規ルートを RunMemory に渡す
        // （記憶済み部分 + 今回の新規部分 = 次回の完全ルート）
        if (RunMemory.Instance != null)
        {
            // 記憶済み分 ＋ 今回の新規分を結合して保存
            var fullPath = new List<MapRecord>();
            if (RunMemory.Instance.IsReplayMode == false)
                fullPath.AddRange(RunMemory.Instance.RecordedPath); // 前回分
            fullPath.AddRange(thisRunPath);                          // 今回の新規分
            RunMemory.Instance.SaveRun(fullPath);
        }

        // プレイヤーをスタート位置に戻す
        player.transform.position = playerStartPos;

        // フロアリセット
        ResetMap();

        // 次走行の準備
        thisRunPath.Clear();
        replayIndex = 0;
    }
    // ──────────────────────────────────────────────────────
}