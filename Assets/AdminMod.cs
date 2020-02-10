using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdminMod : MonoBehaviour
{


    public GameObject Kursor1;
    public GameObject Kursor2;
    public GameObject Kursor3;
    public GameObject Kursor4;
    public GameObject PTime;
    public GameObject POdleglosc;
    public GameObject PTimeTT;

    [SerializeField] public Text Time;
    [SerializeField] public Text Time2;
    private Ray ray;
    private RaycastHit hit;
    private int num1=0;
    private int num2=0;
    private int num3 = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);

        Time.text = DateTime.Now.ToString();
        Time2.text = hit.distance.ToString("F") + " m";


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);
        }

        if (Input.GetKeyDown(KeyCode.Keypad1))
        {

            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {

            SceneManager.LoadScene(2);
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {

            SceneManager.LoadScene(3);
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {

            SceneManager.LoadScene(4);
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {

            SceneManager.LoadScene(5);
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            Kursor1.gameObject.SetActive(true);
            Kursor2.gameObject.SetActive(false);
            Kursor3.gameObject.SetActive(false);
            Kursor4.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Kursor1.gameObject.SetActive(false);
            Kursor2.gameObject.SetActive(true);
            Kursor3.gameObject.SetActive(false);
            Kursor4.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Kursor1.gameObject.SetActive(false);
            Kursor2.gameObject.SetActive(false);
            Kursor3.gameObject.SetActive(true);
            Kursor4.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Kursor1.gameObject.SetActive(false);
            Kursor2.gameObject.SetActive(false);
            Kursor3.gameObject.SetActive(false);
            Kursor4.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            Kursor1.gameObject.SetActive(false);
            Kursor2.gameObject.SetActive(false);
            Kursor3.gameObject.SetActive(false);
            Kursor4.gameObject.SetActive(false);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            num1++;
            if (num1 % 2 == 0)
                PTime.gameObject.SetActive(false);
            if (num1 % 2 != 0)
                PTime.gameObject.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            num2++;
            if (num2 % 2 == 0)
                POdleglosc.gameObject.SetActive(false);
            if (num2 % 2 != 0)
                POdleglosc.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            num3++;
            if (num3 % 2 == 0)
                PTimeTT.gameObject.SetActive(false);
            if (num3 % 2 != 0)
                PTimeTT.gameObject.SetActive(true);
        }
    }
}
