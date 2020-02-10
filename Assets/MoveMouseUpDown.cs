using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MoveMouseUpDown : MonoBehaviour
{
    public float stopStart = 1.5f, speed1 = 2f, speed, rotationSpeed = 250f, heightPlayer = 1f;

    public Transform cameraTransform;
    public float playerWalkingSpeed = 1f;

    public float verticalRotationLimit = 80f;
    public int stopien = 25;

    Vector3 verticalVelocity;
    public GameObject User;
    RaycastHit hit;
    float verticalRotation = 0;

    private bool walk;
    private bool take;
    private bool give;
    public float timeToTeleport = 5f;
    public float timeToDeltaTeleport;
    [SerializeField] public Text TimeField;
    [SerializeField] public Text Speed;

    public GiveObject obj = new GiveObject();
    public GiveObject plc = new GiveObject();

    private void Start()
    {

    }
    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Whatever is your max distance (remove if not needed). However, it is nice to have a max distance to which your monster can see the player.
    float maxDistance = 10;

    private float rayHitStart = 0f;

    void FixedUpdate()
    {
        // Will contain the information of which object the raycast hit
        RaycastHit hit;
        // if raycast hits, it checks if it hit an object with the tag Player
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxDistance) &&
                    hit.collider.gameObject.CompareTag("Player"))
        {
            rayHitStart += Time.deltaTime;
            if (hit.collider.gameObject.CompareTag("Player") == true && rayHitStart >= 5f)
            {
                Debug.Log("Coś");
                rayHitStart = 0f;
            }
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Debug.Log("Say something");
            transform.position -= transform.forward * speed * Time.deltaTime;
        }
    }

    private void Update()
    {
        speed = speed1 * 0.5f;

        if (Input.GetKeyDown(KeyCode.Equals))
        {
            speed1++;
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            speed1--;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            speed1 = 0;
        }
        Speed.text = speed1.ToString("F");

        //Rozglądanie na boki
        float horizontalRotation = Input.GetAxis("Mouse X")*2;
        transform.Rotate(0, horizontalRotation, 0);

        //Rozglądanie góra dół
        verticalRotation -= Input.GetAxis("Mouse Y")*5;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // poruszanie się patrzeniem w dół 
        if (verticalRotation > stopien)
        {
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        // poruszanie się patrzeniem w górę 
        if (verticalRotation < -stopien)
        {
            transform.position -= transform.forward * speed * Time.deltaTime;      
        }
        WhatYouSee();
    }
    public void WhatYouSee()
    {
        if (Physics.Raycast(User.transform.position, User.transform.forward, out hit, maxDistance) &&
                            hit.collider.gameObject.CompareTag("ChangePosition"))
        {
            rayHitStart += Time.deltaTime;
            timeToDeltaTeleport -= Time.deltaTime;
            TimeField.text = timeToDeltaTeleport.ToString("F");
            if (timeToDeltaTeleport <= 0.1)
                timeToDeltaTeleport = timeToTeleport;
            Debug.Log("Widzę");
            if (hit.collider.gameObject.CompareTag("ChangePosition") == true && rayHitStart >= 5f && take == false)
            {
                take = true;
                Debug.Log("Biorę");
                rayHitStart = 0f;
                timeToDeltaTeleport = timeToTeleport;
            }
        }
        else if (take == false)
        {
            rayHitStart = 0f;
            timeToDeltaTeleport -= Time.deltaTime;
        }
        if (take == true)
        {
            obj.GiveToArm(hit.collider.gameObject);

        }



        if (Physics.Raycast(User.transform.position, User.transform.forward, out hit, maxDistance) &&
                        hit.collider.gameObject.CompareTag("SelectPlace"))
        {
            rayHitStart += Time.deltaTime;
            Debug.Log("Widzę miejsce");
            if (hit.collider.gameObject.CompareTag("SelectPlace") == true && rayHitStart >= 5f && take == true)
            {
                take = false;

                Debug.Log("Kładę");
                rayHitStart = 0f;


                obj.ChangePlace(hit.collider.gameObject);
            }

        }
        else if (take == true)
            rayHitStart = 0f;







    }
}