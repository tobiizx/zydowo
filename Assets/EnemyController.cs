using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    GameObject player;
    public float speed = 4f;
    LevelManager lm;
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
        if(other.gameObject.CompareTag("PlayerWeapon"))
        {
            lm.AddPoints(1);
            Destroy(gameObject);
            //Destroy(other.gameObject);
        }
    }
}
