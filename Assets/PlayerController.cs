using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 controllerInput;
    public float speed = 5f;
    //odniesienie do komponentu Rigidbody gracza
    Rigidbody rb;
    // Start is called before the first frame update
    public List<GameObject> enemies;
    public GameObject gun;
    public GameObject bulletSpawn;
    public GameObject swordHandle;
    public GameObject bulletPrefab;
    //referencja do komponentu LevelManager
    LevelManager lm;
    void Start()
    {
        //przypisujemy level manager ze sceny do zmiennej lm
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        //pobieramy odniesienie do komponentu Rigidbody
        rb = GetComponent<Rigidbody>();
        enemies = new List<GameObject>();
        InvokeRepeating("Shoot", 0, 2);
    }

    // Update is called once per frame
    void Update()
    {
        //pobieramy wychylenie wirtualne joysticka w osi x (lewo/prawo) i y (góra/dó³)
        controllerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //tworzymy wektor ruchu na podstawie wychylenia joysticka - zamieniamy y joysticka na z œwiata
        //Vector3 movementVector = new Vector3(controllerInput.x, 0, controllerInput.y);
        //przesuwamy obiekt gracza o wektor ruchu
        //transform.Translate(movementVector * Time.deltaTime * speed);

        //wyci¹gamy sobie ze sceny wszystkie obiekty z tagiem Enemy i pakujemy na listê
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        //sortujemy listê wrogów wed³ug odleg³oœci od gracza i zpaisujemy ponownie jako liste
        //sk³adnia LINQ - to co jest w nawiasie po orderby czytamy jako
        //dla ka¿dego wroga w liœcie enemies oblicz odleg³oœæ od gracza i posortuj listê wed³ug tej odleg³oœci
        enemies = enemies.OrderBy(enemy => Vector3.Distance(enemy.transform.position, transform.position)).ToList();
        //je¿eli przeciwnik znajduje siê bli¿ej ni¿ 2 metry to go atakujemy
        if (enemies.Count > 0 && Vector3.Distance(transform.position, enemies[0].transform.position) < 2f)
        {
            swordHandle.SetActive(true);
            swordHandle.transform.Rotate(0, 2f, 0);
        }
        else
        {
            swordHandle.SetActive(false);
        }
    }
    void FixedUpdate()
    {
        //wyliczamy docelow¹ pozycjê gracza _po_ ruchu
        //najpierw liczymy wektor przesuniêcia
        Vector3 movementVector = new Vector3(controllerInput.x, 0, controllerInput.y);
        //mno¿ymy go przez czas od ostatniej klatki fizyki i predkosc ruchu
        //dodajemy go do obecnego po³o¿enia gracza tworz¹c pozycjê docelow¹
        Vector3 targetPosition = transform.position + movementVector * Time.fixedDeltaTime * speed;
        //przesuwamy gracza przy u¿yciu MovePosition
        rb.MovePosition(targetPosition);
    }
    void Shoot()
    {
        //sprawdz czy mamy jakiœ wrogów na liœcie
        if(enemies.Count > 0)
        {
            //obróæ "pistolet" w kierunku najbli¿szego wroga
            gun.transform.LookAt(enemies[0].transform);
            //stwórz pocisk na wspó³rzêdnych bulletSpawn z rotacj¹ "pistoletu" i zapisz referencje do niego do bullet
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, gun.transform.rotation);
            //rozpêdŸ pocisk w przód
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 1000);
            Destroy(bullet, 2f); //zniszcz pocisk po 2 sekundach
            //skasuj najbli¿szego wroga
            //Destroy(enemies[0]); //czy to jest bezpieczne? zostanie refencja do obiektu w enemies?
            Debug.Log("Pif paf!");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //sprawdzamy czy gracz zderzy³ siê z wrogiem
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //zmniejszamy zdrowie gracza
            lm.ReducePlayerHealth(5);
            //niszczymy wroga
            //Destroy(collision.gameObject);
        }
    }
}
