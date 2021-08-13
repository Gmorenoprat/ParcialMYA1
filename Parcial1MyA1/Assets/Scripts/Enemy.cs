using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour , IObservable, IPrototype
{
    public Transform target;
    private float _speed = FlyWeightPointer.Asteroid.speed;
    
    public RoundManager manager;

    protected bool isClone = false;

    List<IObserver> _allObserver = new List<IObserver>();

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


    public IPrototype Clone()
    {
       
        Enemy e = Instantiate(this);
        e.transform.position = this.transform.position;
        e.transform.position = e.transform.position + new Vector3(Random.Range(0, 3), Random.Range(0, 3),0);
        e.setSpeed(FlyWeightPointer.MiniAsteroid.speed);
        e.setTarget(FindObjectOfType<Player>().transform);
        e.setScale(0.5f);
        return e;
        
    }
    
}
