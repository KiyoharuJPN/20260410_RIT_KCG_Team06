using UnityEngine;

/// <summary>1フロア分の生成記録</summary>
[System.Serializable]
public class MapRecord
{
    public int prefabIndex;   // mapPrefabs の添字
    public Vector3 position;      // 生成ワールド座標
}