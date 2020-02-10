using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLVL : MonoBehaviour
{
    ///public int scene;
    public int i;

      void OnTriggerEnter()
      {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(i);
      }

    public void LoadLevel(string lvl)
    {
              SceneManager.LoadScene(lvl);
        
    }
    private void Update()
    {
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
    }

}