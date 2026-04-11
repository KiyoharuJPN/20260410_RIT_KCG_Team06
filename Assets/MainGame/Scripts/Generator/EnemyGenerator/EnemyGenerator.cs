using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    [System.Serializable]
    struct EnemyGenInfo
    {
        [Tooltip("敵のID")]
        public int enemyID;
        [Tooltip("敵の出現位置")]
        public Vector2 position;
    }

    [SerializeField,Tooltip("敵の出現情報")]
    EnemyGenInfo[] enemyGenInfos;
    // 生成したエニミーを管理するリスト
    List<GameObject> DynamicEnemies = new List<GameObject>();
    // エニミーを生成する親オブジェクト
    Transform enemyRoot;

    // エニミーを生成する
    public void ResetEnemy(GameObject[] enemyPrefabs)
    {
        // エニミーの出現情報がない場合は何もしない
        if (enemyGenInfos.Length == 0) return;
        // エニミーを生成する場所があるかどうかをチェックする
        CheckEnemyRoot();

        // 既に生成されているエニミーを削除する
        if (DynamicEnemies.Count > 0) {
            foreach (GameObject enemy in DynamicEnemies) {
                Destroy(enemy);
            }
            DynamicEnemies.Clear();
        }

        // エニミーを生成する
        foreach (EnemyGenInfo info in enemyGenInfos) {
            GameObject enemy = Instantiate(enemyPrefabs[(info.enemyID-1)], enemyRoot);
            enemy.transform.localPosition = info.position;
            DynamicEnemies.Add(enemy);
        }
    }

    // エニミーを生成する場所があるかどうかをチェックする
    void CheckEnemyRoot()
    {
        // 子オブジェクトを探す
        Transform found = transform.Find("Enemys");

        if (found == null)
        {
            // なかったら作る
            GameObject obj = new GameObject("Enemys");
            obj.transform.SetParent(transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;

            enemyRoot = obj.transform;
        }
        else
        {
            // あったらそれを使う
            enemyRoot = found;
        }
    }
}
