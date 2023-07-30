using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FCG;
using System;

public class ReachedDestination_Audio : MonoBehaviour
{
    private GameObject destination_1_StopColl_1;
    private GameObject destination_1_StopColl_2;

    private double actualTime = 0;
    public double totalTime = 0; //Set value in the scene
    private float percentage = 0;
    private float lastPercentage = 0;
    private bool destinationReached = false;

    private int DurationUntilArrivalInPercentage = 0;
    private int DurationUntilArrivalTotalTimeInMinutes = 0;

    bool [] isPlayed = {false, false, false, false, false};

    // Start is called before the first frame update
    void Start()
    {
        //trafficCar = FindObjectOfType<TrafficCar>();
        destination_1_StopColl_1 = GameObject.Find("Destination_1_StopColl_1");
        destination_1_StopColl_2 = GameObject.Find("Destination_1_StopColl_2");

        if(totalTime == 20400)
            DurationUntilArrivalTotalTimeInMinutes = 7;
        if(totalTime == 5900)
            DurationUntilArrivalTotalTimeInMinutes = 2;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        actualTime++;


        percentage = (float)((actualTime / totalTime) * 100);

        if (percentage - lastPercentage >= 1 && destinationReached == false)
        {
            if (percentage > 100)
            {
                percentage = 100;
                destinationReached = true;
            }

            DurationUntilArrivalInPercentage = (int)percentage;
            lastPercentage = percentage;


            switch(DurationUntilArrivalInPercentage)
            {
                case 1:
                    if(isPlayed[0] == false)
                    {
                        isPlayed[0] = true;
                        PlayAudioTimeTillArrival();
                    }
                    break;
                case 52:
                    if (isPlayed[1] == false)
                    {
                        isPlayed[1] = true;
                        PlayAudioTimeTillArrival();
                    }
                    break;
                case 79:
                    if (isPlayed[2] == false)
                    {
                        isPlayed[2] = true;
                        PlayAudioTimeTillArrival();
                    }
                    break;
                case 90:
                    if (isPlayed[3] == false)
                    {
                        isPlayed[3] = true;
                        PlayAudioTimeTillArrival();
                    }
                    break;
                case 95:
                    if (isPlayed[4] == false)
                    {
                        isPlayed[4] = true;
                        PlayAudioTimeTillArrival();
                    }
                    break;
                default:
                    break;
            }
        }

        //Debug.Log("time: " + actualTime + " distance in percent: " + percentage);
    }



    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.name == "Actor")
        {   
            FindObjectOfType<AudioManager>().Play("ZielErreicht");

            GameObject.Find("CommunicationInstance").GetComponent<UnityArduinoSerialCommunication>().AudioArrivalVerification = true;

            Debug.Log("destination! needed time: " + actualTime);
            destination_1_StopColl_1.GetComponent<BoxCollider>().enabled = true;
            destination_1_StopColl_2.GetComponent<BoxCollider>().enabled = true;
            destinationReached = true;
        }
    }

    private void PlayAudioTimeTillArrival()
    {
        int timeTillArrival = (int)Math.Ceiling((float)(DurationUntilArrivalTotalTimeInMinutes - ((float)DurationUntilArrivalTotalTimeInMinutes * (float)(DurationUntilArrivalInPercentage / 100f))) * 2f);

        switch (timeTillArrival)
        {
            case 0:
                FindObjectOfType<AudioManager>().Play("ZielErreicht");
                break;
            case 1:
                FindObjectOfType<AudioManager>().Play("0_5Min");
                break;
            case 2:
                FindObjectOfType<AudioManager>().Play("1Min");
                break;
            case 3:
                FindObjectOfType<AudioManager>().Play("1_5Min");
                break;
            case 4:
                FindObjectOfType<AudioManager>().Play("2Min");
                break;
            case 5:
                FindObjectOfType<AudioManager>().Play("2_5Min");
                break;
            case 6:
                FindObjectOfType<AudioManager>().Play("3Min");
                break;
            case 7:
                FindObjectOfType<AudioManager>().Play("3_5Min");
                break;
            case 8:
                FindObjectOfType<AudioManager>().Play("4Min");
                break;
            case 9:
                FindObjectOfType<AudioManager>().Play("4_5Min");
                break;
            case 10:
                FindObjectOfType<AudioManager>().Play("5Min");
                break;
            case 11:
                FindObjectOfType<AudioManager>().Play("5_5Min");
                break;
            case 12:
                FindObjectOfType<AudioManager>().Play("6Min");
                break;
            case 13:
                FindObjectOfType<AudioManager>().Play("6_5Min");
                break;
            case 14:
                FindObjectOfType<AudioManager>().Play("7Min");
                break;
            default:
                Debug.Log("Slider Info Button pressed but value is not between 0 and 7 minutes!");
                break;
        }
    }
}
