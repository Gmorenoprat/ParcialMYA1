using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivEnemy : Enemy, IPrototype
{
    bool _cloned = false;
    bool wasCloned { set { _cloned = value; } }

    public int cantClones = 2;

    public override void GetShot()
    {
        if (!_cloned)
        {
            for (int i = 0; i < cantClones; i++)
            { 
                Clone();
            }
        }
        NotifyToObservers("EnemyDestroyed");
        Destroy(this.gameObject);
    }

    public IPrototype Clone()
    {

        DivEnemy e = (DivEnemy) Instantiate(this)
            .setSpeed(FlyWeightPointer.MiniAsteroid.speed)
            .setTarget(FindObjectOfType<Player>().transform)
            .setScale(0.5f);

        e.transform.position = this.transform.position;
        e.transform.position = e.transform.position + new Vector3(Random.Range(0, 1.5f), Random.Range(0, 1.5f), 0);

        e.wasCloned = true;

        return e;
    }
}
