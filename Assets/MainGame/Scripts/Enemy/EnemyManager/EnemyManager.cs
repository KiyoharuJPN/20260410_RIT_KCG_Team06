using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>
{
    List<EnemyBase> liveEnemy = new List<EnemyBase>();

    public List<EnemyBase> GetLiveEnemy()=>liveEnemy;
    public void RemoveEnemy(EnemyBase enemy)=> liveEnemy.Remove(enemy);
    public void AddEnemy(EnemyBase enemy)=>liveEnemy.Add(enemy);
}
