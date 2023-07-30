using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficJamControl_Audio : MonoBehaviour
{
    private GameObject trafficJam_1_StopColl_1;
    private float timeLeft = 110.0f;
    private float timeLeftForArduinoCommand = 120.0f;

    private bool trafficJamReached = false;

    // Start is called before the first frame update
    void Start()
    {
        trafficJam_1_StopColl_1 = GameObject.Find("TrafficJam_1_StopColl_1");
    }

    // Update is called once per frame
    void Update()
    {
        if(trafficJamReached)
        {
            timeLeft -= Time.deltaTime;
            timeLeftForArduinoCommand -= Time.deltaTime;
            //Debug.Log(timeLeft);
        }

        if(timeLeft<0)
        {
            trafficJam_1_StopColl_1.GetComponent<BoxCollider>().enabled = false;
            timeLeft = 100.0f;
        }

        if (timeLeftForArduinoCommand < 0)
        {
            Debug.Log("TrafficJam_1: Finished");

            trafficJamReached = false;
            timeLeftForArduinoCommand = 110.0f;

            GameObject.Find("CommunicationInstance").GetComponent<UnityArduinoSerialCommunication>().AudioReasonForCurrentStop = UnityArduinoSerialCommunication.StoppingReasons.NoStop;
        }
    }


    // break jam
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Actor")
        {   
            Debug.Log("TrafficJam_1:  Start Timer");

            trafficJamReached = true;
            trafficJam_1_StopColl_1.GetComponent<BoxCollider>().enabled = true;
        }
    }
}
