using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswalkControlEarlyWarning_Audio : MonoBehaviour
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
        if(pedestrianWalking == true) pedestrian.GetComponent<Passersby>().ANIMATION_STATE = PeopleAnimState.walk;
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Actor")
        {
            Debug.Log("Crosswalk_1: Early Warning");

            pedestrianWalking = true;

            FindObjectOfType<AudioManager>().Play("StopZebrastreifen");

            GameObject.Find("CommunicationInstance").GetComponent<UnityArduinoSerialCommunication>().AudioReasonForCurrentStop = UnityArduinoSerialCommunication.StoppingReasons.PedestrianCrossing;
        }
    }
}
