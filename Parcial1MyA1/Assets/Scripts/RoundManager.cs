using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour, IObserver
{
    public Enemy enemyPrefab; 

    public Transform spawnPoints;

    Transform[] _spawnPositions;

    Transform _target;

    int _totalEnemies;

    int _actualRound;
    
    void Start()
    {
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_EnemyDestroyed, EnemyDead);

        _target = FindObjectOfType<Player>().transform; //target que le voy a asignar al enemigo

        _spawnPositions = spawnPoints.GetComponentsInChildren<Transform>(); //Los puntos de spawn para los enemigos
        
        StartCoroutine(SpawnEnemies());
    }

    public void EnemyDead()
    {
        _totalEnemies--; //Murio un enemy

        if (_totalEnemies <= 0) //Si no hay mas enemies
        {
            StartCoroutine(SpawnEnemies()); //Nueva wave
        }
    }

    //Devuelve cuantos enemigos crear por ronda
    int CalculateEnemiesToSpawn(int round)
    {
        return round * 2;
    }

    IEnumerator SpawnEnemies()
    {
        _actualRound++; //Nueva ronda

        _totalEnemies = CalculateEnemiesToSpawn(_actualRound); //Total de enemigos a spawnear

        int enemiesToSpawn = _totalEnemies;

        int enemiesCont = 0;
        
        while (enemiesCont < enemiesToSpawn)
        {
            int posToSpawn = Random.Range(0, _spawnPositions.Length); //Posicion en la que va a spawnear

            Enemy e = EnemySpawner.Instance.pool.GetObject();
            e.transform.position = _spawnPositions[posToSpawn].position;
            e.transform.rotation = transform.rotation;
            // var e = Instantiate(enemyPrefab, _spawnPositions[posToSpawn].position, Quaternion.identity);  //Instancio enemy
            e.setTarget(_target);
            e.setMananger(this);
            //e.manager = this; //Le paso el manager para que al morir le avise que reduza uno en _totalEnemies
            //e.target = _target; //Le paso el target

            e.Subscribe(this);

            enemiesCont++;

            yield return new WaitForSeconds(0.5f);
        }
        
    }

    public void Notify(string action)
    {
        if (action == "EnemyDestroyed")
        {
            EnemyDead();
        }
    }
}
