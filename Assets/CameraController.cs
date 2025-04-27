using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    Vector3 offset = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 144;
        //znajdujemy gracza na scenie
        player = GameObject.FindGameObjectWithTag("Player");
        //zapamiêtujemy pozycje kamery wzglêdem gracza
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //docelowa pozycja kamery
        //wczesniej zapamiêtaliœmy sobie oddleg³oœæ kamery od gracza i teraz j¹ dodajemy
        //¿eby uzyskaæ now¹ pozycjê kamery
        Vector3 targetPosition = player.transform.position + offset;
        //p³ynnie przesuñ kamerê pomiêdzy obecnym po³o¿eniem, a docelowym po³o¿eniem
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
    }
}
