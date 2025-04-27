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
        //zapami�tujemy pozycje kamery wzgl�dem gracza
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //docelowa pozycja kamery
        //wczesniej zapami�tali�my sobie oddleg�o�� kamery od gracza i teraz j� dodajemy
        //�eby uzyska� now� pozycj� kamery
        Vector3 targetPosition = player.transform.position + offset;
        //p�ynnie przesu� kamer� pomi�dzy obecnym po�o�eniem, a docelowym po�o�eniem
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime);
    }
}
