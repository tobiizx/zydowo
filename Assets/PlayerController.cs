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
        //pobieramy wychylenie wirtualne joysticka w osi x (lewo/prawo) i y (g�ra/d�)
        controllerInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        //tworzymy wektor ruchu na podstawie wychylenia joysticka - zamieniamy y joysticka na z �wiata
        //Vector3 movementVector = new Vector3(controllerInput.x, 0, controllerInput.y);
        //przesuwamy obiekt gracza o wektor ruchu
        //transform.Translate(movementVector * Time.deltaTime * speed);

        //wyci�gamy sobie ze sceny wszystkie obiekty z tagiem Enemy i pakujemy na list�
        enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        //sortujemy list� wrog�w wed�ug odleg�o�ci od gracza i zpaisujemy ponownie jako liste
        //sk�adnia LINQ - to co jest w nawiasie po orderby czytamy jako
        //dla ka�dego wroga w li�cie enemies oblicz odleg�o�� od gracza i posortuj list� wed�ug tej odleg�o�ci
        enemies = enemies.OrderBy(enemy => Vector3.Distance(enemy.transform.position, transform.position)).ToList();
        //je�eli przeciwnik znajduje si� bli�ej ni� 2 metry to go atakujemy
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
        //wyliczamy docelow� pozycj� gracza _po_ ruchu
        //najpierw liczymy wektor przesuni�cia
        Vector3 movementVector = new Vector3(controllerInput.x, 0, controllerInput.y);
        //mno�ymy go przez czas od ostatniej klatki fizyki i predkosc ruchu
        //dodajemy go do obecnego po�o�enia gracza tworz�c pozycj� docelow�
        Vector3 targetPosition = transform.position + movementVector * Time.fixedDeltaTime * speed;
        //przesuwamy gracza przy u�yciu MovePosition
        rb.MovePosition(targetPosition);
    }
    void Shoot()
    {
        //sprawdz czy mamy jaki� wrog�w na li�cie
        if(enemies.Count > 0)
        {
            //obr�� "pistolet" w kierunku najbli�szego wroga
            gun.transform.LookAt(enemies[0].transform);
            //stw�rz pocisk na wsp�rz�dnych bulletSpawn z rotacj� "pistoletu" i zapisz referencje do niego do bullet
            GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.transform.position, gun.transform.rotation);
            //rozp�d� pocisk w prz�d
            bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.forward * 1000);
            Destroy(bullet, 2f); //zniszcz pocisk po 2 sekundach
            //skasuj najbli�szego wroga
            //Destroy(enemies[0]); //czy to jest bezpieczne? zostanie refencja do obiektu w enemies?
            Debug.Log("Pif paf!");
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //sprawdzamy czy gracz zderzy� si� z wrogiem
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //zmniejszamy zdrowie gracza
            lm.ReducePlayerHealth(5);
            //niszczymy wroga
            //Destroy(collision.gameObject);
        }
    }
}
