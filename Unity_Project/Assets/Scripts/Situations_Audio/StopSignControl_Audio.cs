using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopSignControl_Audio : MonoBehaviour
{
    private GameObject stopSign_1_StopColl_1;
    private float timeLeft = 3.0f;
    private bool stopSignReached = false;
    private bool carIsActor = false;

    // Start is called before the first frame update
    void Start()
    {
        stopSign_1_StopColl_1 = GameObject.Find("StopSign_1_StopColl_1");
    }

    // Update is called once per frame
    void Update()
    {
        if(stopSignReached)
        {
            timeLeft -= Time.deltaTime;
            //Debug.Log(timeLeft);
        }

        if(timeLeft<0)
        {
            stopSign_1_StopColl_1.GetComponent<BoxCollider>().enabled = false;
            stopSignReached = false;
            timeLeft = 3.0f;

            if(carIsActor == true)
            {
                Debug.Log("StopSign_1: Finished");

                carIsActor = false;

                GameObject.Find("CommunicationInstance").GetComponent<UnityArduinoSerialCommunication>().AudioReasonForCurrentStop = UnityArduinoSerialCommunication.StoppingReasons.NoStop;
            }
        }
    }


    // 
    private void OnTriggerEnter(Collider col)
    {
        stopSign_1_StopColl_1.GetComponent<BoxCollider>().enabled = true;
        stopSignReached = true;

        if (col.gameObject.name == "Actor")
        {
            Debug.Log("StopSign_1: Start Timer");

            carIsActor = true;
        }
    }
}
