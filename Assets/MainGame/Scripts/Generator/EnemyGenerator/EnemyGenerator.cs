using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    struct EnemyGenInfo
    {
        [Tooltip("敵のID")]
        public int enemyID;
        [Tooltip("敵の出現位置")]
        public Vector2 position;
    }

    [SerializeField,Tooltip("敵の出現情報")]
    EnemyGenInfo[] enemyGenInfos;

    List<GameObject> DynamicEnemies = new List<GameObject>();


    public void ResetEnemy(GameObject[] enemyPrefabs)
    {
        if (DynamicEnemies.Count > 0) {
            foreach (GameObject enemy in DynamicEnemies) {
                Destroy(enemy);
            }
            DynamicEnemies.Clear();
        }

        foreach (EnemyGenInfo info in enemyGenInfos) {
            GameObject enemy = Instantiate(enemyPrefabs[(info.enemyID-1)], transform);
            enemy.transform.localPosition = info.position;
            DynamicEnemies.Add(enemy);
        }
    }
}
