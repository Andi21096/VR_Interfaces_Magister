using UnityEngine;
using System.Collections;


public class GiveObject : MonoBehaviour
{ 
    public GameObject User;
    
    GameObject ob;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
    public void GiveToArm(GameObject Object2)
    {
        //Object2 = Object2
        Object2.transform.position = User.transform.position + User.transform.forward * 0.5f;
        //Object2.transform.position = new Vector3(User.transform.position.x, User.transform.position.y, User.transform.position.y) + User.transform.forward / 2;
        ob = Object2;
        //Debug.Log(Object2.transform.position);
    }
    public void ChangePlace(GameObject Place2)
    {
        ob.transform.position = Place2.transform.position;

    }
}