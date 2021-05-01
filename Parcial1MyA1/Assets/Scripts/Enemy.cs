using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour , IObservable
{
    public float speed;
    public Transform target;

    public RoundManager manager;

    List<IObserver> _allObserver = new List<IObserver>();


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


    // Update is called once per frame
    void Update()
    {
        if (!target) return; //Si no hay target no hago nada

        //Movimiento
        Vector3 dir = target.position - transform.position;
        dir.z = target.position.z;
        dir.Normalize();
        transform.position += dir * speed * Time.deltaTime;
    }

    public void GetShot()
    {
        NotifyToObservers("EnemyDestroyed");
        //manager.EnemyDead(); //Le digo al manager que mori
        EnemySpawner.Instance.ReturnEnemy(this);
        //Destroy(gameObject); //Me destruyo
    }
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

    #region IOBServable
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
