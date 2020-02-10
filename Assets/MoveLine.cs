using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveLine : MonoBehaviour
{
    public float stopStart = 1.5f, speed1 = 2f, speed, rotationSpeed = 250f, heightPlayer = 1f;

    private float mag;
    private Ray ray;
    private RaycastHit hit;
    //private Vector3 target = new Vector3();
   // private Vector3 lastTarget = new Vector3();

    public GameObject punkt;
    public GameObject punkt2;
    public GameObject punkt3;
    public GameObject punkt4;
    public GameObject punkt5;
    public GameObject punkt6;

    public List<Vector3> punkts = new List<Vector3>();

    public Transform cameraTransform;
    private bool walk;

    public float verticalRotationLimit = 80f;
    public int stopien = 25;
    [SerializeField] public Text Speed;
    Vector3 verticalVelocity;
    int i = 0;

    float verticalRotation = 0;

    private void Start()
    {
        MovePoints();
        
    }
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


        ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        
        if (punkts[i] != transform.position && hit.distance >= 2)
        {
            Debug.Log(hit.distance + "distance");
            walk = true;

            mag = (transform.position - punkts[i]).magnitude;
            transform.position = Vector3.MoveTowards(transform.position, punkts[i], mag > stopStart ? speed * UnityEngine.Time.deltaTime : Mathf.Lerp(speed * 0.5f, speed, mag / stopStart) * UnityEngine.Time.deltaTime);
            ray = new Ray(transform.position, -Vector3.up);
           
        }
        else
        {
            if (hit.distance < 2)
            {
                walk = false;
            }
        

        }
        if (i < punkts.Count - 1)
        {
            if (punkts[i] == transform.position)
            {
                Debug.Log(punkts[i]);
                i++;
            }
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("Menu");
        }

        //Rozglądanie na boki
        float horizontalRotation = Input.GetAxis("Mouse X")*2;
            transform.Rotate(0, horizontalRotation, 0);

            //Rozglądanie góra dół
            verticalRotation -= Input.GetAxis("Mouse Y")*5;
            verticalRotation = Mathf.Clamp(verticalRotation, -verticalRotationLimit, verticalRotationLimit);
            Camera.main.transform.localRotation = Quaternion.Euler(verticalRotation, 0, 0);
     
    }

    private void MoveTo(Vector3 point)
    {
            
            if (point != transform.position)
            {
                Debug.Log("Funkcja if- =!");
                if (!walk)
                {
                    Debug.Log("Funkcja if-walk");
                    walk = true;
                }
                mag = (transform.position - point).magnitude;
                transform.position = Vector3.MoveTowards(transform.position, point, mag > stopStart ? speed * UnityEngine.Time.deltaTime : Mathf.Lerp(speed * 0.5f, speed, mag / stopStart) * UnityEngine.Time.deltaTime);
                ray = new Ray(transform.position, -Vector3.up);
            }
            {
                    if (point == transform.position)
                    {
                        Debug.Log(point);
                        walk = false;
                    }
            }
                  
    }


    void MovePoints()
    {       
        punkts.Add(punkt.transform.position);
        punkts.Add(punkt2.transform.position);
        punkts.Add(punkt3.transform.position);
        punkts.Add(punkt4.transform.position);
        punkts.Add(punkt5.transform.position);
        punkts.Add(punkt6.transform.position);
    }

}