using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IObservable
{
    public float speed;
    public float timeToDie;
    public Player owner;

    //Lista donde guardo todos los IObservers 
    List<IObserver> _allObserver = new List<IObserver>();

    //Strategy
    IAdvance myCurrentStrategy;
    IAdvance myCurrentStrategyNormal;
    IAdvance myCurrentStrategySinuoso;


    void Awake()
    {
        myCurrentStrategyNormal = new NormalAdvance(speed,transform);
        myCurrentStrategySinuoso = new SinuousAdvance(speed,transform);

    }


    void Start()
    {
        myCurrentStrategy = myCurrentStrategySinuoso;
    }
    // Update is called once per frame
    void Update()
    {
        //Movimiento
        if (myCurrentStrategy != null)
            myCurrentStrategy.Advance();
        //transform.position += transform.right * speed * Time.deltaTime;

        //Lifetime
        timeToDie -= Time.deltaTime;

        if (timeToDie<= 0)
        {
            BulletSpawner.Instance.ReturnBullet(this);
            //Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy en = collision.GetComponent<Enemy>();

        if (en)
        {

            NotifyToObservers("BulletHit");
            //owner.TargetHit(); //Le digo al player que le pegue
            en.GetShot(); //Le hago damage al enemigo

            BulletSpawner.Instance.ReturnBullet(this); //Devuelvo al pool
            //Destroy(this.gameObject);
        }
    }

    //Funcion para agarrar una bullet del pool
    public static void TurnOn(Bullet b)
    {
        b.gameObject.SetActive(true);  //La activo
    }

    //Funcion para devolver una bullet al pool
    public static void TurnOff(Bullet b)
    {
        b.Unsubscribe(b.owner);
        b.gameObject.SetActive(false); //La deshabilito

    }

    #region Interfaz IObservable

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

