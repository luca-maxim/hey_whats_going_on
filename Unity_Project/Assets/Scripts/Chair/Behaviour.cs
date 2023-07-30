using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour : MonoBehaviour
{
    GameObject myCar;

    
    float acceleration;
    float lastAcceleration;

    // Start is called before the first frame update
    void Start()
    {
         myCar = GameObject.Find("Actor");
         lastAcceleration = myCar.GetComponent<Rigidbody>().velocity.magnitude;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.B) == true){
            
            Debug.Log("Velocity in x/z-Direction: " + acceleration);
            Debug.Log("angularVelocity ((x,y,z)-Direction): " + myCar.GetComponent<Rigidbody>().angularVelocity);
        }
    }

    //Wird jedes mal aufgerufen, wenn die pyhsics engine einen neuen Wert berechnet
    void FixedUpdate ()
    {
        //Acceleration of the car in x/z Direction
        acceleration = (myCar.GetComponent<Rigidbody>().velocity.magnitude - lastAcceleration) / Time.fixedDeltaTime;

        //Last Acceleration of the car in x/z Direction in the lasz time step
        lastAcceleration = myCar.GetComponent<Rigidbody>().velocity.magnitude;
    }
}
