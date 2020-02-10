using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class MoveTeleport : MonoBehaviour
{
    public float stopStart = 1.5f, speed = 1f, rotationSpeed = 250f, heightPlayer = 1f;

    private float mag, angleToTarget;
    private Ray ray;
    private RaycastHit hit;
    private Vector3 dir;
    private Vector3 target = new Vector3();
    private Vector3 lastTarget = new Vector3();
    private float rayHitStart = 0f;

    public Transform cameraTransform;
    private bool walk;

    public float gran_x1 = 252.5f;
    public float gran_x2 = 244.3f;
    public float gran_z1 = 259f;
    public float gran_z2 = 243;

    public float verticalRotationLimit = 80f;
    public int stopien = 25;

    public float timeToTeleport = 5f;
    public float timeToDeltaTeleport;
    Vector3 verticalVelocity;
    public List<Vector3> forward = new List<Vector3>();

    float verticalRotation = 0;

    [SerializeField] public Text TimeField;
    [SerializeField] public Text Speed;

    void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

    }

    private void Update()
    {

        Debug.Log(timeToTeleport);
       //Rozglądanie na boki
       float horizontalRotation = Input.GetAxis("Mouse X")*2;
        transform.Rotate(0, horizontalRotation, 0);

        //Rozglądanie góra dół
        verticalRotation -= Input.GetAxis("Mouse Y")*5;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
        Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        // poruszanie się patrzeniem w dół(w miejsce patrzenia kursora)

        
        ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 10000.0f))
            {
                target = hit.point;
            }

        
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            timeToTeleport++;
        }
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            timeToTeleport--;
        }
        Speed.text = timeToTeleport.ToString("F");
        MoveTo();
    }
    private void CalculateAngle(Vector3 temp)
    {
        dir = new Vector3(temp.x, transform.position.y, temp.z) - transform.position;
        angleToTarget = Vector3.Angle(dir, transform.forward);
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
            if ((transform.position - target).sqrMagnitude > heightPlayer + 0.1f &&
                rayHitStart >= timeToTeleport)
                {
                    transform.position = target;
                    rayHitStart = 0f;
                    timeToDeltaTeleport = timeToTeleport;
                ray = new Ray(transform.position, -Vector3.up);
                    if (Physics.Raycast(ray, out hit, 3000.0f))
                    {
                        transform.position = new Vector3(transform.position.x, hit.point.y + heightPlayer, transform.position.z);
                    }
                }
                //else if (target.y >= 0.2f)
                //rayHitStart = 0f;
        }
        
    }
    }
