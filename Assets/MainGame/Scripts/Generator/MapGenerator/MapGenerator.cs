using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // プレハブ格納用
    [SerializeField,Tooltip("マップのプレハブ")] 
    GameObject[] mapPrefabs;
    [SerializeField,Tooltip("敵のプレハブ")]
    public GameObject[] enemyPrefabs;
    // 生成位置管理用
    [SerializeField,Tooltip("生成間隔")]
    int interval = 16;
    [SerializeField,Tooltip("生成開始高さ")]    
    int startHeight = 32;
    [SerializeField, Tooltip("座標ずらし")]
    int genOffset = 20;
    // 生成したマップの管理用
    List<GameObject> DynamicMaps = new List<GameObject>();
    // プレイヤーの管理用
    GameObject player;
    Vector2 playerStartPos;

    // 生成管理用
    int LastGenHeight = 0;



    private void Start()
    {
        // プレイヤーに関する初期化
        player = GameObject.FindGameObjectWithTag("Player");
        playerStartPos = player.transform.position;
        // 生成関係
        LastGenHeight = startHeight;

    }


    private void Update()
    {
        if(player.transform.position.y > LastGenHeight - genOffset) {
            LastGenHeight += interval;
            GenMap();
        }
    }

    void GenMap()
    {
        // ランダムにマップを選択
        int index = Random.Range(0, mapPrefabs.Length);

        // 生成位置（Yだけ上に積む）
        Vector3 spawnPos = new Vector3(0, startHeight + interval * DynamicMaps.Count, 0);

        // 生成
        GameObject map = Instantiate(mapPrefabs[index], spawnPos, Quaternion.identity, transform);

        // 敵のリセット
        map.GetComponent<EnemyGenerator>().ResetEnemy(enemyPrefabs);

        // リストに追加
        DynamicMaps.Add(map);
    }

    public void ResetMap()
    {
        foreach(GameObject map in DynamicMaps) {
            map.GetComponent<EnemyGenerator>().ResetEnemy(enemyPrefabs);
        }
    }

    // プレイヤーが死んだときのこれを読んだら最初の一に戻してくれるさらにマップもリセットしてくれる
    public void PlayerDead()
    {
        player.transform.position = playerStartPos;
        ResetMap();
    }
}
