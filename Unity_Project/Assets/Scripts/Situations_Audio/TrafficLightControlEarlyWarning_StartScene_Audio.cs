using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficLightControlEarlyWarning_StartScene_Audio : MonoBehaviour
{
    private GameObject pedestrian;

    private bool pedestrianWalking = false;

    // Start is called before the first frame update
    void Start()
    {
        pedestrian = GameObject.Find("Pedestrian");
    }
    private void Update()
    {
        if (pedestrianWalking == true) pedestrian.GetComponent<Passersby>().ANIMATION_STATE = PeopleAnimState.walk;
    }

    // set trafficlight on green
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Actor")
        {
            Debug.Log("TrafficLight_1: Early Warning");

            pedestrianWalking = true;

            FindObjectOfType<AudioManager>().Play("StopAmpel");

            GameObject.Find("CommunicationInstance").GetComponent<UnityArduinoSerialCommunication>().AudioReasonForCurrentStop = UnityArduinoSerialCommunication.StoppingReasons.TrafficLight;
        }
    }
}
