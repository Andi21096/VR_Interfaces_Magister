using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MoveMouseDown : MonoBehaviour
{
    public float stopStart = 1.5f, speed1 = 2f, speed, 
        rotationSpeed = 250f, heightPlayer = 1f;    //ustawienie początkowych parametrów
    public Transform cameraTransform;               //definicja zmiennych
    public float playerWalkingSpeed = 1f;
    public float verticalRotationLimit = 80f;
    public int stopien = 25;
    public GameObject User;
    public float timeToTeleport = 5f;
    public float timeToDeltaTeleport;
    [SerializeField] public Text TimeField;         //tworzenie objektów tekstowych do wyświetlania 
    [SerializeField] public Text Speed;             //zmiennych w projekcie           
    Vector3 verticalVelocity;
    RaycastHit hit;
    float verticalRotation = 0;
    private bool walk;
    private bool take;
    private bool give;
    public GiveObject obj = new GiveObject();
    public GiveObject plc = new GiveObject();
    float maxDistance = 5f;
    private float rayHitStart = 0f;
    void Awake()            
    {       
        Cursor.visible = false;                     //kursor niewidoczny
        Cursor.lockState = CursorLockMode.Locked;   //kursor jest zablokowany (żeby nie wychocził za ramkę programu)
    }
    private void OnTriggerStay(Collider other)      //funkcja jest wywołana gdy jest aktywowany trigger      
    {
        if (other.gameObject.tag == "Wall")         //jeżeli objekt ma tag "Wall" 
        {
            Debug.Log("Say something");
            transform.position -= transform.forward * speed * Time.deltaTime;   //zmień kierunek gracza na przeciwny
        }                                           //ta funkcja służy do tego, żeby gracz nie przekraczał ścian
    }
    void FixedUpdate()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance) && //jeżeli gracz patrzy na objekt
                    hit.collider.gameObject.CompareTag("Player"))           //  z tagiem "Player" i jest bliżej niż maxDistance 
        {
            rayHitStart += Time.deltaTime;          //zacznij odliczać  
            timeToDeltaTeleport -= Time.deltaTime;  
            if (hit.collider.gameObject.CompareTag("Player") == true && rayHitStart >= 5f)  //jeżeli wciąż patrzy na "Player'a"
            {                                                                               //i minęło 5 sekund
                rayHitStart = 0f;                   //wyzeruj licznik
            }
        }    
    }
    private void Update()
    {
        speed = speed1 * 0.5f;          //zmienna dla większej precyzji wyznaczania prędkości
        if (Input.GetKeyDown(KeyCode.Equals))   //jeżeli naciśnięty przycisk "="
            speed1++;                           //zwiększ prędkość
        if (Input.GetKeyDown(KeyCode.Minus))    //jeżeli naciśnięty przycisk "-"
            speed1--;                           //zmniejsz prędkość
        if (Input.GetKeyDown(KeyCode.P))        //jeżeli naciśnięty przycisk "P"
            speed1 = 0;                         //wyzeruj prędkość
        Speed.text = speed1.ToString("F");      //wyświetl prędkość
        //Rozglądanie na boki (sterowanie kamerą w poziomie)
        float horizontalRotation = Input.GetAxis("Mouse X")*2;
        transform.Rotate(0, horizontalRotation, 0);
        //Rozglądanie góra/dół ((sterowanie kamerą w pionie)
        verticalRotation -= Input.GetAxis("Mouse Y")*5;
        verticalRotation = Mathf.Clamp(verticalRotation, 
            -verticalRotationLimit, verticalRotationLimit);
        Camera.main.transform.localRotation = 
            Quaternion.Euler(verticalRotation, 0, 0);
        WhatYouSee();
        // poruszanie się patrzeniem w dół 
        if (verticalRotation > stopien)         
            //jeżeli kąt nachylenia jest większy od "stopien"
        {
            transform.position += transform.forward * speed * Time.deltaTime;   
            //zmień pozycję gracza
        }
    }
    public void WhatYouSee()    //funkcja służaca do podejmowania przedmiotów na które patrzy gracz
    {
        if (Physics.Raycast(User.transform.position, User.transform.forward, out hit, maxDistance) &&
                            hit.collider.gameObject.CompareTag("ChangePosition"))   //jeżeli gracz patrzy
        {                                                           // na objekt z tagiem "ChangePosition"
            rayHitStart += Time.deltaTime;                          //aktywuj licznik
            timeToDeltaTeleport -= Time.deltaTime;
            TimeField.text = timeToDeltaTeleport.ToString("F");     //wyświetl licznik dla gracza
            if (timeToDeltaTeleport <= 0.1)
                timeToDeltaTeleport = timeToTeleport;
            if (hit.collider.gameObject.CompareTag("ChangePosition") == true && rayHitStart >= 5f && take == false)
            {       //czy wciąż patrzy na "ChangePosition" i minęło 5 sekund
                take = true;                        //weź przedmiot
                rayHitStart = 0f;                   //wyzeruj licznik
                timeToDeltaTeleport = timeToTeleport;   
            }
        }
        else if (take == false)     //jeżeli przedmiot nie jest wzięty
        {
            rayHitStart = 0f;       //wyzeruj licznik
            timeToDeltaTeleport -= Time.deltaTime;
        }
        if (take == true)           //jeżeli wzięty
                obj.GiveToArm(hit.collider.gameObject); //przywiąż przedmiot do gracza              
        if (Physics.Raycast(User.transform.position, User.transform.forward, out hit, maxDistance) &&
                        hit.collider.gameObject.CompareTag("SelectPlace"))
        {                   //jeżeli widzi miejsce gdzie można ustawić przedmiot
            rayHitStart += Time.deltaTime;  //aktywuj licznik
            if (hit.collider.gameObject.CompareTag("SelectPlace") == true && rayHitStart >= 5f && take == true)
            {//jeżeli wciąż patrzy na miejsce gdzie można ustawić przemniot (tag "SelectPlace") i minęło 5 sekund
                //i przedmiot jest wzięty
                take = false;   //ustaw że przedmiot nie jest wzięty
                rayHitStart = 0f;               //wyzeruj licznik 
                obj.ChangePlace(hit.collider.gameObject);   //ustaw przedmiot na miejsce z tagiem "SelectPlace"
            }

        }
        else if (take == true)  
            rayHitStart = 0f;
    } 
}