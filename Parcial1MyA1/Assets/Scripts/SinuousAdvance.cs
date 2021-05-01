using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinuousAdvance : IAdvance
{
    float _speed = 3;
    float _frecuencia = 9f;
    float _magnitud = 0.04f;
    Transform _xf;

   
    

    public SinuousAdvance(float speed, Transform transform)
    {
        _speed = speed;
        _xf = transform;
    }

    public void Advance()
    {
        var pos = _xf.position;
        pos += _xf.right * Time.deltaTime * _speed;
        _xf.position = pos + _xf.up * Mathf.Sin(Time.time * _frecuencia) * _magnitud;
      
    }
}


