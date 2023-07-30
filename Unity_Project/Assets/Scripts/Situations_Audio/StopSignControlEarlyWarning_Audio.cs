using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSignControlEarlyWarning_Audio : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }


    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Actor")
        {
            Debug.Log("StopSign_1: Early Warning");

            FindObjectOfType<AudioManager>().Play("StopStoppschild");

            GameObject.Find("CommunicationInstance").GetComponent<UnityArduinoSerialCommunication>().AudioReasonForCurrentStop = UnityArduinoSerialCommunication.StoppingReasons.StopSign;
        }
    }
}
