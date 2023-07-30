using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightControlEarlyWarning_Audio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }


    // set trafficlight on green
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Actor")
        {
            Debug.Log("TrafficLight_1: Early Warning");
            FindObjectOfType<AudioManager>().Play("StopAmpel");

            GameObject.Find("CommunicationInstance").GetComponent<UnityArduinoSerialCommunication>().AudioReasonForCurrentStop = UnityArduinoSerialCommunication.StoppingReasons.TrafficLight;
        }
    }
}
