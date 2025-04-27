using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    public float speed = 4f;
    LevelManager lm;
    public int health = 10; // liczba hp przeciwnika

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        lm = GameObject.Find("LevelManager").GetComponent<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        transform.LookAt(player.transform);
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlayerWeapon_Sword")) // jeœli bijemy z miecza to wiêcej hp zmniejszamy
        {
            health -= 3; // zmniejszamy hp przeciwnika o 3
            Debug.Log("Hit! Health: " + health);
        }
        else if (other.gameObject.CompareTag("PlayerWeapon_Gun")) // jeœli strzelamy z pistola to mniej hp zmniejszamy
        {
            health -= 1; // zmniejszamy hp przeciwnika o 1
            Debug.Log("Hit! Health: " + health);
        }
        if (health <= 0)
        {
            lm.AddPoints(1);
            Destroy(gameObject);
        }
    }
}
