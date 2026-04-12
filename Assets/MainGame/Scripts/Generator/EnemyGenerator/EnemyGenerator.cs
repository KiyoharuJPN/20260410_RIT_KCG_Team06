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
        [Tooltip("敵の移動速度(敵1に対して設定は無効になります/0デフォルト→仕様書参照)")]
        public float moveSpeed;
        [Tooltip("敵の移動距離(敵1に対して設定は無効になります/0デフォルト→仕様書参照)")]
        public float moveDistance;
        [Tooltip("敵のHP(0デフォルト→仕様書参照)")]
        public float hp;
    }

    [SerializeField,Tooltip("敵の出現情報")]
    EnemyGenInfo[] enemyGenInfos;
    // 生成したエニミーを管理するリスト
    List<GameObject> DynamicEnemies = new List<GameObject>();
    // エニミーを生成する親オブジェクト
    Transform enemyRoot = null;

    // エニミーを生成する
    public void ResetEnemy(GameObject[] enemyPrefabs)
    {
        // エニミーの出現情報がない場合は何もしない
        if (enemyGenInfos.Length == 0) return;
        // エニミーを生成する場所があるかどうかをチェックする
        if (enemyRoot == null) CheckEnemyRoot();

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
            EnemyBase eb = enemy.GetComponent<EnemyBase>();
            if(info.moveSpeed > 0) eb.SetMoveSpeed(info.moveSpeed);
            if(info.moveDistance > 0) eb.SetDistance(info.moveDistance);
            if(info.hp > 0) eb.SetHP(info.hp);
            DynamicEnemies.Add(enemy);
        }
    }

    // エニミーを生成する場所があるかどうかをチェックする
    void CheckEnemyRoot()
    {
        // 子オブジェクトを探す
        Transform found = transform.Find("Enemy");

        if (found == null)
        {
            // なかったら作る
            GameObject obj = new GameObject("Enemy");
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
