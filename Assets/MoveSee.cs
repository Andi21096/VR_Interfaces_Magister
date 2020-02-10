using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class MoveSee : MonoBehaviour
{
    public float stopStart = 1.5f, speed1 = 2f, speed, rotationSpeed = 250f, heightPlayer = 1f;

    private float mag, angleToTarget;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 dir;
    private Vector3 target = new Vector3();
    private Vector3 lastTarget = new Vector3();
    public List<Vector3> forward = new List<Vector3>();
    public Transform cameraTransform;
    private bool walk;
    private float rayHitStart = 0f;

    public float timeToTeleport = 3f;
    public float timeToDeltaTeleport;
    public float verticalRotationLimit = 80f;
    public int stopien = 25;
    float maxDistance = 5f;
    public GameObject User;

    public float timeToTeleport1 = 3f;
    public float timeToDeltaTeleport1;
    [SerializeField] public Text TimeField;
    [SerializeField] public Text Speed;
    private bool take;
    private bool give;

    public GiveObject obj = new GiveObject();
    public GiveObject plc = new GiveObject();

    Vector3 verticalVelocity;
    [SerializeField] public Text TimeField1;

    float verticalRotation = 0;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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

        // poruszanie się patrzeniem w dół(w miejsce patrzenia kursora)

        if (verticalRotation > 12)
        {
            ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10000.0f))
            {
                target = hit.point;
            }
        }
        MoveTo();
    }
        private void CalculateAngle(Vector3 temp)
    {
        dir = new Vector3(temp.x, transform.position.y, temp.z) - transform.position;
        angleToTarget = Vector3.Angle(dir, transform.forward);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Wall")
        {
            Debug.Log("Say something");
            transform.position -= transform.forward * speed * Time.deltaTime;
        }
    }
    private void LookAtThis()
    {
        if (target != lastTarget)
        {
            CalculateAngle(target);
            if (angleToTarget > 3)
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(dir), rotationSpeed * UnityEngine.Time.deltaTime);
        }
    }

    void SaveForward()
    {
        if (forward.Count < 5)
            forward.Add(transform.forward);
        else
        {
            forward.Remove(forward[0]);
            forward.Add(transform.forward);
            //Debug.Log(forward[0]);
        }

    }

    private void MoveTo()
    {
        SaveForward();
        if (target != lastTarget)
        {
            //Debug.Log(hit.point);
            if (target.y >= 0.2f ||
                transform.forward.x <= forward[0].x - 0.1 ||
                transform.forward.x >= forward[0].x + 0.1 ||
                transform.forward.y <= forward[0].y - 0.1 ||
                transform.forward.y >= forward[0].y + 0.1)
            {
                //Debug.Log("Zmiana kursora");
                rayHitStart = 0f;
                timeToDeltaTeleport = timeToTeleport;
            }
            else
            {
                rayHitStart += Time.deltaTime;
                timeToDeltaTeleport -= Time.deltaTime;
            }
            TimeField.text = timeToDeltaTeleport.ToString("F");
            if (timeToDeltaTeleport <= 0.1)
                timeToDeltaTeleport = timeToTeleport;
            if ((transform.position - target).sqrMagnitude > heightPlayer + 0.1f 
                //&& rayHitStart >= timeToTeleport
                )
            {
                if (!walk)
                {

                    walk = true;
                }
                mag = (transform.position - target).magnitude;
                transform.position = Vector3.MoveTowards(transform.position, target, mag > stopStart ? speed * UnityEngine.Time.deltaTime : Mathf.Lerp(speed * 0.5f, speed, mag / stopStart) * UnityEngine.Time.deltaTime);
                ray = new Ray(transform.position, -Vector3.up);
                timeToDeltaTeleport = timeToTeleport;
                if (Physics.Raycast(ray, out hit, 1000.0f))
                {
                    transform.position = new Vector3(transform.position.x, hit.point.y + heightPlayer, transform.position.z);
                }
            }
            else
            {
                lastTarget = target;
                if (walk)
                {

                    walk = false;
                }
            }
        }
    }
    public void WhatYouSee()
    {
        if (Physics.Raycast(User.transform.position, User.transform.forward, out hit, maxDistance) &&
                            hit.collider.gameObject.CompareTag("ChangePosition"))
        {
            rayHitStart += Time.deltaTime;
            timeToDeltaTeleport1 -= Time.deltaTime;
            TimeField.text = timeToDeltaTeleport1.ToString("F");
            if (timeToDeltaTeleport1 <= 0.1)
                timeToDeltaTeleport1 = timeToTeleport;
            Debug.Log("Widzę");
            if (hit.collider.gameObject.CompareTag("ChangePosition") == true && rayHitStart >= 5f && take == false)
            {
                take = true;
                Debug.Log("Biorę");
                rayHitStart = 0f;
                timeToDeltaTeleport1 = timeToTeleport1;
            }
        }
        else if (take == false)
        {
            rayHitStart = 0f;
            timeToDeltaTeleport1 -= Time.deltaTime;
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