using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float timeToDie;
    public Player owner;

    // Update is called once per frame
    void Update()
    {
        //Movimiento
        transform.position += transform.right * speed * Time.deltaTime;

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
            owner.TargetHit(); //Le digo al player que le pegue
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
        b.gameObject.SetActive(false); //La deshabilito
    }
}

