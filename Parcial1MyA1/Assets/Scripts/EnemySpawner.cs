﻿using System.Collections;
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

    public void Awake()
    {
        _instance = this;

        pool = new ObjectPool<Enemy>(EnemyFactory, Enemy.TurnOn, Enemy.TurnOff, enemyStock, true);

    }
    private void Start()
    {
        
    }
    public Enemy EnemyFactory()
    {
        Enemy e = Instantiate(enemyPrefab);
        e.transform.parent = this.transform;
        return e;
    }

    public void ReturnEnemy(Enemy b)
    {
        pool.ReturnObject(b);
    }


}
