using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour , IObserver
{
    public float speed;
    public float shootCooldown;
    public Bullet bulletPrefab;
    public Transform pointToSpawn;
    public Image cooldownBar;

    Camera _myCamera;
    bool _canShoot;
    Coroutine _shootCDCor;


    // Start is called before the first frame update
    void Start()
    {
        _myCamera = Camera.main;
        _canShoot = true;
        CompletedFireCooldown();

        EventManager.SubscribeToEvent(EventManager.EventsType.Event_BulletHit, TargetHit);
    }

    // Update is called once per frame
    void Update()
    {
        //Movimiento
        Vector3 lookAtPos = _myCamera.ScreenToWorldPoint(Input.mousePosition);
        lookAtPos.z = transform.position.z;
        transform.right = lookAtPos - transform.position;

        transform.position += (_myCamera.transform.right * Input.GetAxisRaw("Horizontal") + _myCamera.transform.up * Input.GetAxisRaw("Vertical")).normalized * speed * Time.deltaTime;

        //Disparo
        if (Input.GetMouseButtonDown(0))
        {
            if (_canShoot) Shoot();
        }
    }

    void Shoot()
    {
        Bullet b = BulletSpawner.Instance.pool.GetObject();
        b.transform.position = pointToSpawn.position;
        b.transform.rotation = transform.rotation;
        //Bullet b = Instantiate(bulletPrefab, pointToSpawn.position, transform.rotation); //Instancio bala
        b.timeToDie = shootCooldown;  //Le paso el cooldown como tiempo de vida
        b.owner = this;  //Le paso que el owner es este script para que cuando mate un enemigo me avise
        b.Subscribe(this);

        _shootCDCor = StartCoroutine(ShootCooldown());  //Corrutina del cooldown para volver a disparar
    }

    //Funcion para cuando la bala toca un enemigo
    public void TargetHit()
    {
        if (_shootCDCor != null)
        {
            StopCoroutine(_shootCDCor);
        }

        _canShoot = true;
        CompletedFireCooldown();

    }

    //Setea cambios de la barra de CD del UI
    void CompletedFireCooldown()
    {
        cooldownBar.color = Color.green;
        cooldownBar.fillAmount = 1;
    }

    IEnumerator ShootCooldown()
    {
        _canShoot = false;

        float ticks = 0;

        cooldownBar.color = Color.red;
        cooldownBar.fillAmount = 0;

        while (ticks < shootCooldown)
        {
            ticks += Time.deltaTime;
            cooldownBar.fillAmount = ticks;
            yield return null;
        }

        CompletedFireCooldown();
        _canShoot = true;
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
            EventManager.TriggerEvent(EventManager.EventsType.Event_BulletHit);

        }
    }
}
