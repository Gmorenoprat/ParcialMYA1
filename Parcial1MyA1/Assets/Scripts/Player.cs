using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour, IObserver
{
    public float speed;
    public float bulletSpeed = 5f;
    public float shootCooldown;
    public bool _canShoot;
    public Bullet bulletPrefab;
    public Transform pointToSpawn;
    public Image cooldownBar;

    Camera _myCamera;
    Coroutine _shootCDCor;

    PlayerController playerController;
    PlayerView playerView;

    public AudioSource[] audios;

    public event Func<float, IEnumerator> fireCooldown;// = delegate { };
    public event Action completedFireCooldown = delegate { };

    //Strategy
    public IAdvance advance;

    public enum TipoDisparo{
        normal = 0,
        sinuous = 1,
    }


    // Start is called before the first frame update
    void Start()
    {
        _myCamera = Camera.main;
        _canShoot = true;
        completedFireCooldown();

        EventManager.SubscribeToEvent(EventManager.EventsType.Event_BulletHit, TargetHit);
        playerView = new PlayerView(cooldownBar, audios);
        playerController = new PlayerController(this, _myCamera, playerView);
    }

    // Update is called once per frame
    void Update()
    {
        playerController.OnUpdate();
    }

    public void Shoot(TipoDisparo tipoDisparo)
    {
        //Bullet b = Instantiate(bulletPrefab, pointToSpawn.position, transform.rotation); //Instancio bala

        //b.timeToDie = shootCooldown;  //Le paso el cooldown como tiempo de vida
        //b.owner = this;  //Le paso que el owner es este script para que cuando mate un enemigo me avise
        
        Bullet b = BulletSpawner.Instance.pool.GetObject().SetSpeed(bulletSpeed).SetTimeToDie(shootCooldown).SetOwner(this);
        b.transform.position = pointToSpawn.position;
        b.transform.rotation = transform.rotation;

        //Strategy?
        if (tipoDisparo == TipoDisparo.normal)  { advance = new NormalAdvance(bulletSpeed, b.transform); playerView.normalShoot(); }
        if (tipoDisparo == TipoDisparo.sinuous) { advance = new SinuousAdvance(bulletSpeed, b.transform); playerView.sinuousShoot(); }
        b.SetType(advance);
       
        b.Subscribe(this);

        _shootCDCor = StartCoroutine(ShootCooldown());  //Corrutina del cooldown para volver a disparar
    }

    //Funcion para cuando la bala toca un enemigo
    public void TargetHit()
    {
        if (_shootCDCor != null && this != null)
        {
            StopCoroutine(_shootCDCor);
        }
        playerView.TargetHit();


        _canShoot = true;
        completedFireCooldown();
        playerView.Reload();
    }


    IEnumerator ShootCooldown()
    {
        _canShoot = false;
        yield return fireCooldown(shootCooldown);
        completedFireCooldown();
        _canShoot = true;
        playerView.Reload();

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }
    }

    public void Notify(string action)
    {
        if(action=="BulletHit")
        {
            TargetHit();
            //EventManager.TriggerEvent(EventManager.EventsType.Event_BulletHit);

        }
    }
}
