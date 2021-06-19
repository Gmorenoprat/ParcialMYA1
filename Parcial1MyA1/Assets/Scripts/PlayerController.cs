using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : IController
{
    Player _player;
    Camera _myCamera;

    public PlayerController(Player p, Camera c, PlayerView v)
    {
        _player = p;
        _myCamera = c;


        _player.fireCooldown += v.FireCooldown;
        _player.completedFireCooldown += v.CompletedFireCooldown;

    }

    public void OnUpdate()
    {
        //Movimiento
        Vector3 lookAtPos = _myCamera.ScreenToWorldPoint(Input.mousePosition);
        lookAtPos.z = _player.transform.position.z;
        _player.transform.right = lookAtPos - _player.transform.position;

        _player.transform.position += (_myCamera.transform.right * Input.GetAxisRaw("Horizontal") + _myCamera.transform.up * Input.GetAxisRaw("Vertical")).normalized * _player.speed * Time.deltaTime;

        //Disparo
        if (Input.GetMouseButtonDown(0))
        {
            if (_player._canShoot) _player.Shoot(Player.TipoDisparo.normal);
        }
        //Disparo Sinuoso
        if (Input.GetMouseButtonDown(1))
        {
            if (_player._canShoot) _player.Shoot(Player.TipoDisparo.sinuous);
        }

    }
}
