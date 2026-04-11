using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // プレハブ格納用
    [SerializeField,Tooltip("マップのプレハブ")] 
    GameObject[] mapPrefabs;
    [SerializeField,Tooltip("敵のプレハブ")]
    GameObject[] enemyPrefabs;
    // 生成位置管理用
    [SerializeField,Tooltip("生成間隔")]
    int interval = 16;
    [SerializeField,Tooltip("生成開始高さ")]    
    int startHeight = 24;
    [SerializeField,Tooltip("座標ずらし")]
    int genOffset = 10;

    List<GameObject> DynamicMaps = new List<GameObject>();




    public void ResetMap()
    {
        foreach(GameObject map in DynamicMaps) {
            map.GetComponent<EnemyGenerator>().ResetEnemy(enemyPrefabs);
        }
    }
}
