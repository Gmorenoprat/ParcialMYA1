using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundManager : MonoBehaviour, IObserver
{
   

    public Transform spawnPoints;

    Transform[] _spawnPositions;

    Transform _target;

    int _totalEnemies;

    int _actualRound;

    //nombre de variable innecesariamente extenso 
    LookUpTable<int, int> _enemigosASpawnearPrimerasDiezRondas;

    public System.Func<bool> isPaused;

    void Awake()
    {
        _enemigosASpawnearPrimerasDiezRondas = new LookUpTable<int, int>(CalculateEnemiesToSpawn);
        for(int i = 1; i<11; i++) {
            _enemigosASpawnearPrimerasDiezRondas.ReturnValue(i);
        }

    }

    void Start()
    {
        EventManager.SubscribeToEvent(EventManager.EventsType.Event_EnemyDestroyed, EnemyDead);

        _target = FindObjectOfType<Player>().transform; //target que le voy a asignar al enemigo

        _spawnPositions = spawnPoints.GetComponentsInChildren<Transform>(); //Los puntos de spawn para los enemigos

        isPaused = boolIsPaused;

        StartCoroutine(SpawnEnemies());

    }
     public bool boolIsPaused()
     {
        return this.enabled;
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

        _totalEnemies = _enemigosASpawnearPrimerasDiezRondas.ReturnValue(_actualRound); //Total de enemigos a spawnear

        int enemiesToSpawn = _totalEnemies;

        int enemiesCont = 0;
        
        while (enemiesCont < enemiesToSpawn)
        {
            //e.manager = this; //Le paso el manager para que al morir le avise que reduza uno en _totalEnemies
            //e.target = _target; //Le paso el target
            // var e = Instantiate(enemyPrefab, _spawnPositions[posToSpawn].position, Quaternion.identity);  //Instancio enemy
            
            
            int posToSpawn = Random.Range(0, _spawnPositions.Length); //Posicion en la que va a spawnear

            yield return new WaitUntil(isPaused); //nice workaround

            Enemy e = EnemySpawner.Instance.pool.GetObject();
            e.transform.position = _spawnPositions[posToSpawn].position;
            e.transform.rotation = transform.rotation;
            e.setTarget(_target);
            e.setMananger(this);

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
