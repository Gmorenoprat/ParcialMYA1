using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinuousAdvance : IAdvance
{
    float _speed = 3;
    float _frecuencia = 1f;
    float _onda = 1f;
    Transform _xf;

    Vector3 pos;
    Vector3 axis;

    public SinuousAdvance(float speed, Transform transform)
    {
        _speed = speed;
        _xf = transform;
    }

    public void Advance()
    {
        pos = _xf.right * _speed * Time.deltaTime;
        axis = _xf.up;
        _xf.position += pos + new Vector3(0, Mathf.Sin(_xf.transform.position.x * _frecuencia * 2 * Mathf.PI) * _onda, 0);


     

    }
}


