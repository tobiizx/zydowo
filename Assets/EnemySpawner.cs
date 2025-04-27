using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    List<Vector3> spawnPoints;
    public GameObject enemy;
    // Start is called before the first frame update
    void Start()
    {
        //znajdujemy transform SpawnPositions (zawieraj¹cy punkty) w hierarchii obiektu EnemySpawner
        Transform sp = transform.Find("SpawnPositions");
        spawnPoints = new List<Vector3>();
        foreach (Transform child in sp)
        {
            //dla ka¿dego dziecka transformu SpawnPositions dodajemy jego GameObject do listy spawnPoints
            spawnPoints.Add(child.position);
        }
        //wywo³ujemy funkcjê Spawn co 1 sekunde
        InvokeRepeating("Spawn", 0, 0.1f);
    }
    void Spawn()
    {
        //losujemy indeks punktu spawnu
        int index = Random.Range(0, spawnPoints.Count);
        //tworzymy nowego wroga na losowym punkcie spawnu
        Instantiate(enemy, spawnPoints[index], Quaternion.identity);
    }
}
