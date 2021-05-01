using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalAdvance : IAdvance
{
    float _speed = 5;
    Transform _xf;

    public NormalAdvance(float speed, Transform transform)
    {
        _speed = speed;
        _xf = transform;
    }

    public void Advance()
    {
        _xf.transform.position += _xf.right * _speed * Time.deltaTime;
    }
}
