using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance
    {
        get { return _instance; }
    }

    static EnemySpawner _instance;

    public Enemy enemyPrefab;
    public int enemyStock = 5;


    public ObjectPool<Enemy> pool;

    void Start()
    {
        _instance = this;

        pool = new ObjectPool<Enemy>(EnemyFactory, Enemy.TurnOn, Enemy.TurnOff, enemyStock, true);
    }

    public Enemy EnemyFactory()
    {
        return Instantiate(enemyPrefab);
    }

    public void ReturnEnemy(Enemy b)
    {
        pool.ReturnObject(b);
    }
}
