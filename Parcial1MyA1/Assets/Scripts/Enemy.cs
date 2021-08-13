using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour , IObservable
{
    public Transform target;
    private float _speed = FlyWeightPointer.Asteroid.speed;
    protected int _score = FlyWeightPointer.Asteroid.score;
    
    public RoundManager manager;


    public event Action<int> onDie;


    List<IObserver> _allObserver = new List<IObserver>();


    void Start()
    {
        onDie = ScoreMananger.Instance.SumarScore;
    }
    #region Builders
    public Enemy setScale(float Multiplier)
    {
        this.transform.localScale = this.transform.localScale * Multiplier;
        return this;
    }

    public Enemy setSpeed(float speed)
    {
        this._speed = speed;
        return this;
    }
    public Enemy setScore(int score)
    {
        this._score = score;
        return this;
    }
    public Enemy setTarget(Transform target)
    {
        this.target = target;
        return this;
    }

    public Enemy setMananger(RoundManager manager)
    {
        this.manager = manager;
        return this;
    }
    #endregion

    void Update()
    {
        if (!target) return; //Si no hay target no hago nada

        //Movimiento
        Vector3 dir = target.position - transform.position;
        dir.z = target.position.z;
        dir.Normalize();

        transform.position += dir * _speed * Time.deltaTime;
    }

    public virtual void GetShot()
    {
        NotifyToObservers("EnemyDestroyed");
        onDie(_score);
        EnemySpawner.Instance.ReturnEnemy(this);
    }

    #region POOL
    //Funcion para agarrar una bullet del pool
    public static void TurnOn(Enemy e)
    {
        e.gameObject.SetActive(true);  //La activo
    }

    //Funcion para devolver una bullet al pool
    public static void TurnOff(Enemy e)
    {
        e.gameObject.SetActive(false); //La deshabilito
    }

    #endregion

    #region IObservable
    public void Subscribe(IObserver obs)
    {
        if (!_allObserver.Contains(obs))
        {
            _allObserver.Add(obs);
        }
    }

    public void Unsubscribe(IObserver obs)
    {
        if (_allObserver.Contains(obs))
        {
            _allObserver.Remove(obs);
        }
    }

    public void NotifyToObservers(string action)
    {
        for (int i = _allObserver.Count - 1; i >= 0; i--)
        {
            _allObserver[i].Notify(action);
        }
    }
    #endregion



    
}
