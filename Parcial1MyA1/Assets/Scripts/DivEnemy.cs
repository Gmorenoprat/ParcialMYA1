using UnityEngine;

public class DivEnemy : Enemy, IPrototype
{
    bool _cloned = false;
    float _scaleMuliplier = 0.5f;
    float _distanceToClone = 1.5f;
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

        wasCloned = false;
        Reset();
        base.GetShot();
    }

    public IPrototype Clone()
    {

        DivEnemy e = (DivEnemy) Instantiate(this)
            .setSpeed(FlyWeightPointer.MiniAsteroid.speed)
            .setTarget(FindObjectOfType<Player>().transform)
            .setScale(_scaleMuliplier)
            .setScore(FlyWeightPointer.MiniAsteroid.score);

        e.transform.position = this.transform.position;
        e.transform.position = e.transform.position + new Vector3(Random.Range(0, _distanceToClone), Random.Range(0, _distanceToClone), 0);

        e.wasCloned = true;

        return e;
    }

    public void Reset()
    {
        this.transform.localScale = new Vector3(1, 1, 1);
        this.setSpeed(FlyWeightPointer.Asteroid.speed).setScore(FlyWeightPointer.Asteroid.score);
    }
}
