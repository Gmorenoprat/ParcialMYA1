using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerView 
{
    public Image _cooldownBar;
    public AudioSource[] _audios;

    public PlayerView(Image img, AudioSource[] audios)
    {
        _cooldownBar = img;
        _audios = audios;
    }
    //Setea cambios de la barra de CD del UI
    public void CompletedFireCooldown()
    {
        if (_cooldownBar == null) return;
        _cooldownBar.color = Color.green;
        _cooldownBar.fillAmount = 1;
    }


    public IEnumerator FireCooldown(float cooldown, Func<bool> isPaused) {
        float ticks = 0;
        _cooldownBar.color = Color.red;
        _cooldownBar.fillAmount = 0;
        while(ticks < cooldown) {

            yield return new WaitUntil(isPaused); //nice workaround

            ticks += Time.deltaTime;
            _cooldownBar.fillAmount = ticks;
            yield return null;
        }
    }

    public void normalShoot()
    {
        _audios[0].Play();
    }
    public void sinuousShoot()
    {
        _audios[1].Play();
    }

    public void TargetHit()
    {
        _audios[2].Play();
    }
    public void Reload()
    {
        _audios[3].Play();
    }



}
