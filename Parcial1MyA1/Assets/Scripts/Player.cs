using System;
using System.Collections;
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

    public event Func<float, Func<bool>, IEnumerator> fireCooldown;// = delegate { };
    public event Action completedFireCooldown = delegate { };

    public Func<bool> isPaused;


    //Strategy
    public IAdvance advance;

    public enum TipoDisparo{
        normal = 0,
        sinuous = 1,
    }
    public bool boolIsPaused()
    {
        return this.enabled;
    }


    // Start is called before the first frame update
    void Start()
    {
        isPaused = boolIsPaused;

        _myCamera = Camera.main;
        _canShoot = true;
        completedFireCooldown();

        EventManager.SubscribeToEvent(EventManager.EventsType.Event_BulletHit, TargetHit);
        playerView = new PlayerView(cooldownBar, audios);
        playerController = new PlayerController(this);

        fireCooldown += playerView.FireCooldown;
        completedFireCooldown += playerView.CompletedFireCooldown;
    }

    void Update()
    {
        playerController.OnUpdate();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>())
        {
            //to-enhace: crear y exportar a SceneController
            UnityEngine.SceneManagement.SceneManager.LoadScene(2);
        }
    }

    #region MOVEMENT
    internal void Move(float h, float v)
    {
        Vector3 lookAtPos = _myCamera.ScreenToWorldPoint(Input.mousePosition);
        lookAtPos.z = this.transform.position.z;
        this.transform.right = lookAtPos - this.transform.position;

        this.transform.position += (_myCamera.transform.right * h + _myCamera.transform.up * v).normalized * this.speed * Time.deltaTime;
    }
    #endregion

    #region BATTLE
    public void Shoot(TipoDisparo tipoDisparo)
    {
        Bullet b = BulletSpawner.Instance.pool.GetObject().SetSpeed(bulletSpeed).SetTimeToDie(shootCooldown).SetOwner(this);
        b.transform.position = pointToSpawn.position;
        b.transform.rotation = transform.rotation;

        //Strategy?
        if (tipoDisparo == TipoDisparo.normal)  { advance = new NormalAdvance(bulletSpeed, b.transform); playerView.normalShoot(); }
        else if (tipoDisparo == TipoDisparo.sinuous) { advance = new SinuousAdvance(bulletSpeed, b.transform); playerView.sinuousShoot(); }
        b.SetType(advance);
       
        b.Subscribe(this);

        _shootCDCor = StartCoroutine(ShootCooldown());  //Corrutina del cooldown para volver a disparar
        
    }

    internal void ShootSinuous()
    {
        if (_canShoot) Shoot(Player.TipoDisparo.sinuous);
    }
    internal void ShootNormal()
    {
        if (_canShoot) Shoot(Player.TipoDisparo.normal);
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
        
        yield return fireCooldown(shootCooldown, isPaused);

        completedFireCooldown();
        _canShoot = true;
        playerView.Reload();

    }
    #endregion

    #region IObserver
    public void Notify(string action)
    {
        if(action=="BulletHit")
        {
            TargetHit();
        }
    }
    #endregion
}
