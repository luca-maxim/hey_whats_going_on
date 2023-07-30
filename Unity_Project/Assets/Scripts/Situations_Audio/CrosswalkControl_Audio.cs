using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosswalkControl_Audio : MonoBehaviour
{
    private GameObject crosswalk_1_StopColl_1;
    private float timeLeft = 20.0f;
    private bool crosswalkReached = false;

    // Start is called before the first frame update
    void Start()
    {
        crosswalk_1_StopColl_1 = GameObject.Find("Crosswalk_1_StopColl_1");
    }

    // Update is called once per frame
    void Update()
    {
        if(crosswalkReached)
        {
            timeLeft -= Time.deltaTime;
            //Debug.Log(timeLeft);
        }

        if(timeLeft<0)
        {
            Debug.Log("Crosswalk_1: Finished");

            crosswalk_1_StopColl_1.GetComponent<BoxCollider>().enabled = false;
            crosswalkReached = false;
            timeLeft = 20.0f;

            GameObject.Find("CommunicationInstance").GetComponent<UnityArduinoSerialCommunication>().AudioReasonForCurrentStop = UnityArduinoSerialCommunication.StoppingReasons.NoStop;
        }
    }


    // 
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Actor")
        {
            Debug.Log("Crosswalk_1: Start Timer");

            crosswalk_1_StopColl_1.GetComponent<BoxCollider>().enabled = true;
            crosswalkReached = true;
        }
    }
}
