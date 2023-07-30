using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * A = PING
 * S = Check button states
 * 
 * Q = ObstaclesAroundCar
 * W = ReasonForCurrentStop
 * E = UpcommingCurveInDegree
 * R = DurationUntilArrivalInPercentage
 * T = ArrivalVerification
 * Z = ExitDirection
 * U = RelationCarAndDestinationPositionInDegree
*/

public class Testbench : MonoBehaviour
{
    public UnityArduinoSerialCommunication Arduino;

    private string response;

    int obstacleCounter = 0;
    int reasonForStopCounter = 0;
    int curveCounter = 90;
    int arivalCounter = 5;
    int exitDirectionCounter = 0;
    int DestinationPositionCounter = 45;

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) == true)
        {
            Debug.Log("A: Send \"PING\" to arduino");
            Arduino.WriteToArduino("PING");
            response = Arduino.ReadFromArduino(50);
            Debug.Log(response);
        }
        if (Input.GetKeyDown(KeyCode.S) == true)
        {
            Debug.Log("S: Check button states");
            Arduino.WriteToArduino("GBS");
            response = Arduino.ReadFromArduino(50);
            Debug.Log(response);
        }
        if (Input.GetKeyDown(KeyCode.Q) == true)
        {
            Debug.Log("Q: ObstaclesAroundCar");
            if (obstacleCounter == 0)
            {
                Arduino.SetObstaclesAroundCar(new UnityArduinoSerialCommunication.ObstacleTyps[]{
                    UnityArduinoSerialCommunication.ObstacleTyps.NoObstacle,            //Position: [0] = left,
                    UnityArduinoSerialCommunication.ObstacleTyps.MovingSameDirection,   //Position: [1] = right,
                });
            }
            else if (obstacleCounter == 1)
            {
                Arduino.SetObstaclesAroundCar(new UnityArduinoSerialCommunication.ObstacleTyps[]{
                    UnityArduinoSerialCommunication.ObstacleTyps.MovingOppositeDirection,   //Position: [0] = left,
                    UnityArduinoSerialCommunication.ObstacleTyps.MovingSameDirection,       //Position: [1] = right,
                });
            }
            else if (obstacleCounter == 2)
            {
                Arduino.SetObstaclesAroundCar(new UnityArduinoSerialCommunication.ObstacleTyps[]{
                    UnityArduinoSerialCommunication.ObstacleTyps.MovingOppositeDirection,   //Position: [0] = left,
                    UnityArduinoSerialCommunication.ObstacleTyps.NoObstacle,                //Position: [1] = right,
                });
            }
            else
            {
                Arduino.SetObstaclesAroundCar(new UnityArduinoSerialCommunication.ObstacleTyps[]{
                    UnityArduinoSerialCommunication.ObstacleTyps.NoObstacle,   //Position: [0] = left,
                    UnityArduinoSerialCommunication.ObstacleTyps.NoObstacle,   //Position: [1] = right,
                });
            }
            obstacleCounter++;
            if (obstacleCounter >= 4) obstacleCounter = 0;
        }
        if (Input.GetKeyDown(KeyCode.W) == true)
        {
            Debug.Log("W: ReasonForCurrentStop");
            if (reasonForStopCounter == 0)
                Arduino.SetReasonForCurrentStop(UnityArduinoSerialCommunication.StoppingReasons.StopSign);
            else
                Arduino.SetReasonForCurrentStop(UnityArduinoSerialCommunication.StoppingReasons.NoStop);

            reasonForStopCounter++;
            if (reasonForStopCounter >= 2) reasonForStopCounter = 0;
        }
        if (Input.GetKeyDown(KeyCode.E) == true)
        {
            Debug.Log("E: UpcommingCurveInDegree");
            Arduino.SetUpcommingCurveInDegree(curveCounter);

            if (curveCounter == 90) curveCounter = 0;
            else if (curveCounter == 0) curveCounter = -90;
            else if (curveCounter == -90) curveCounter = 90;
        }
        if (Input.GetKeyDown(KeyCode.R) == true)
        {
            Debug.Log("R: DurationUntilArrivalInPercentage");
            Arduino.SetDurationUntilArrivalInPercentage(arivalCounter);

            arivalCounter += 5;
            if (arivalCounter > 10) arivalCounter = 0;
        }
        if (Input.GetKeyDown(KeyCode.T) == true)
        {
            Debug.Log("T: ArrivalVerification");
            Arduino.SetArrivalVerification(true);
        }
        if (Input.GetKeyDown(KeyCode.Y) == true)
        {
            Debug.Log("Z: ExitDirection");

            if (exitDirectionCounter == 0)
                Arduino.SetExitDirection(UnityArduinoSerialCommunication.ExitDirections.ExitRight);
            else if (exitDirectionCounter == 1)
                Arduino.SetExitDirection(UnityArduinoSerialCommunication.ExitDirections.ExitLeft);
            else
                Arduino.SetExitDirection(UnityArduinoSerialCommunication.ExitDirections.NoValue);

            exitDirectionCounter++;
            if (exitDirectionCounter >= 3) exitDirectionCounter = 0;

        }
        if (Input.GetKeyDown(KeyCode.U) == true)
        {
            Debug.Log("U: RelationCarAndDestinationPositionInDegree");
            Arduino.SetRelationCarAndDestinationPositionInDegree(DestinationPositionCounter);

            if (DestinationPositionCounter == 45) DestinationPositionCounter = -45;
            else DestinationPositionCounter = 45;

        }
    }
}
