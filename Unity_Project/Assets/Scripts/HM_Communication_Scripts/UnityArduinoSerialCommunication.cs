using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.SceneManagement;
using System.IO;

public class UnityArduinoSerialCommunication : MonoBehaviour
{
    /* The serial port where the Arduino is connected. */
    private string port = "COM4";
    /* The baudrate of the serial port. */
    private int baudrate = 9600;
    private SerialPort stream;
    private string response;

    //declarations of enumerations
    public enum ObstacleTyps
    {
        NoObstacle              = 0,    //Values: 0 = no obstacle
        MovingSameDirection     = 1,    //Values: 1 = Moving object that goes in the same direction
        MovingOppositeDirection = 2,    //Values: 2 = Moving object that goes in the opposite direction
        OtherObstacle           = 3     //Values: 3 = e.g. a stationary object like a tree or a parking car
    };
    public enum ExitDirections
    {
        NoValue     = 0,    //Values: 0 = car is driving
        ExitLeft    = 1,
        ExitRight   = 2
    };
    public enum StoppingReasons
    {
        NoStop          = 0,     //Values: 0 = car is driving
        TrafficLight    = 1,
        TrafficJam      = 2,
        StopSign        = 3,
        PedestrianCrossing = 4
    };
    private enum StateChanges
    {
        NoValueChanged                                  = 0,
        ObstaclesAroundCar_ValueChanged                 = 1,
        ExitDirection_ValueChanged                      = 2,
        ReasonForCurrentStop_ValueChanged               = 3,
        UpcommingCurve_ValueChanged                     = 4,
        DurationUntilArrival_ValueChanged               = 5,
        ArrivalVerification_ValueChanged                = 6,
        RelationCarAndDestinationPosition_ValueChanged  = 7
    };
    private enum ArduinoButtons
    {
        NoUserInput             = 0,
        ObstacleLeft            = 1,
        ObstacleRight           = 2,
        ExitDirectionLeft       = 3,
        ExitDirectionRight      = 4,
        ReasonForCurrentStop    = 5,
        DestinationSlider       = 6
    };



    //private variables that define the tactile-dispaly board state
    private ObstacleTyps[] ObstaclesAroundCar
        = new ObstacleTyps[2]
        {
            ObstacleTyps.NoObstacle,    //Position: [0] = left,
            ObstacleTyps.NoObstacle,    //Position: [1] = right,
        };
    private ExitDirections ExitDirection = ExitDirections.NoValue;
    private StoppingReasons ReasonForCurrentStop            = StoppingReasons.NoStop;  //= 0 if car is driving
    private int UpcommingCurveInDegree                      = 0;    //Set value to 0 after exiting a curve
    private int DurationUntilArrivalInPercentage            = 0;
    private int DurationUntilArrivalTotalTimeInMinutes      = 0;
    private bool ArrivalVerification                        = false;
    private int RelationCarAndDestinationPositionInDegree   = 0;
    private ArduinoButtons LatestRegisteredButtonPress      = ArduinoButtons.NoUserInput;
    //private List<ArduinoButtons> RegisteredButtonPresses  = new List<ArduinoButtons>();



    //private variables for the communication between the arduino and unity
    private List<StateChanges> CurrentStateChanges                      = new List<StateChanges>();
    private int FixedUpdateLoopGetButtonStatesControlVariable           = 0;
    private int FixedUpdateLoopGetButtonStatesControlVariableMaxValue   = 10;    //set to 10 later

    //private variables for logging
    private string csvDocumentName;

    //private variables for Audio-Variant
    public bool isKeyPressed = false;
    public bool isAudioManagerActivated = true;
    public StoppingReasons AudioReasonForCurrentStop = StoppingReasons.NoStop;
    public bool AudioArrivalVerification = false;

    void Start()
    {
        Debug.Log("UnityArduinoSerialCommunication_Startup");

        try
        {
            CreateNewCSVFile();
        }
        catch(Exception e)
        {
            Debug.Log(e);
        }

        if (SceneManager.GetActiveScene().name.Contains("Audio"))
        {
            isAudioManagerActivated = false;
            Debug.Log("We are in Audio!");
        }
            

        this.Open();

    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.A) == true)
        {
            PlayAudioTimeTillArrival();
        }
        */
        if (SceneManager.GetActiveScene().name.Contains("Audio"))
            AudioVariant_CheckButtonState();
    }

    private void FixedUpdate()
    {
        TactileDisplay_Command_LoopFunction();
        TactileDisplay_CheckButtonStates_LoopFunction();
    }






    //Audio-Variant Button functions
    private void AudioVariant_CheckButtonState()
    {
        if (Input.GetKeyDown(KeyCode.G) == true)
        {
            if(isKeyPressed == false)
            {
                isKeyPressed = true;
                LogDataInCSV("Button pressed", "Audio Variant");

                Debug.Log("Audioansage An");
                FindObjectOfType<AudioManager>().Play("AudioansageAn");
                StartCoroutine(ActivateAudioManager());
            }
        }

        if (Input.GetKeyUp(KeyCode.G) == true)
        {
            if (isKeyPressed == true)
            {
                isKeyPressed = false;
                LogDataInCSV("Button released", "Audio Variant");

                isAudioManagerActivated = false;
                Debug.Log("Audioansage Aus");
                FindObjectOfType<AudioManager>().Play("AudioansageAus");
                StartCoroutine(DeactivateAudioManager());
            }
        }
    }

    IEnumerator ActivateAudioManager()
    {
        yield return new WaitForSeconds(2.5f);
        isAudioManagerActivated = true;

        if(AudioReasonForCurrentStop != StoppingReasons.NoStop && isKeyPressed == true)
        {
            PlayAudioReasonForCurrentStop(AudioReasonForCurrentStop);
        }

        if (AudioArrivalVerification == true && isKeyPressed == true)
        {
            FindObjectOfType<AudioManager>().Play("ZielErreicht");
        }
    }

    IEnumerator DeactivateAudioManager()
    {
        yield return new WaitForSeconds(2.5f);
        isAudioManagerActivated = false;
    }




    //Logging functions

    private void CreateNewCSVFile()
    {
        //Debug.Log("CreateNewCSVFile_Start");

        csvDocumentName = Application.dataPath + "/LoggingFiles/" + DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH/mm") + "_" + SceneManager.GetActiveScene().name + "_LogData.csv";

        if (!File.Exists(csvDocumentName))
        {
            File.WriteAllText(csvDocumentName, "Time; Position x; Position z; Action; Specific Action-Type \n");
        }

        //Debug.Log("CreateNewCSVFile_End");
    }

    public void LogDataInCSV (string logText_Action, string logText_SpecificActionType)
    {
        GameObject myActor = GameObject.Find("Actor");
        if(File.Exists(csvDocumentName))
        {
            File.AppendAllText(csvDocumentName, DateTime.Now.ToString("dd/MM/yyyy") + "_" + DateTime.Now.ToString("HH:mm:ss") + ";" 
            + myActor.transform.position.x + ";"
            + myActor.transform.position.z + ";"
            + logText_Action + ";"
            + logText_SpecificActionType + "\n");
        }
        else{
            Debug.Log("CSV-File doesnt exist! Cant write in file.");
        }
        
    }




    //Audio functions

    private void PlayAudioReasonForCurrentStop(StoppingReasons reasonForCurrentStop)
    {
        switch (reasonForCurrentStop)
        {
            case StoppingReasons.NoStop:
                break;
            case StoppingReasons.TrafficLight:
                FindObjectOfType<AudioManager>().Play("StopAmpel");
                break;
            case StoppingReasons.TrafficJam:
                FindObjectOfType<AudioManager>().Play("StopStau");
                break;
            case StoppingReasons.StopSign:
                FindObjectOfType<AudioManager>().Play("StopStoppschild");
                break;
            case StoppingReasons.PedestrianCrossing:
                FindObjectOfType<AudioManager>().Play("StopZebrastreifen");
                break;
            default:
                Debug.Log("PlayAudioReasonForCurrentStop played but no stopping reason!");
                break;
        }
    }

    private void PlayAudioTimeTillArrival()
    {
        int timeTillArrival = (int)Math.Ceiling((float)(DurationUntilArrivalTotalTimeInMinutes - ((float)DurationUntilArrivalTotalTimeInMinutes * (float)(DurationUntilArrivalInPercentage / 100f))) * 2f);

        Debug.Log("Test: " + timeTillArrival + ", " + DurationUntilArrivalTotalTimeInMinutes + ", " + DurationUntilArrivalInPercentage);

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

    private void PlayAudioButton(ArduinoButtons latestButton)
    {
        switch (latestButton)
        {
            case ArduinoButtons.ObstacleLeft:
                if (ObstaclesAroundCar[0] != ObstacleTyps.NoObstacle)
                    FindObjectOfType<AudioManager>().Play("HindernisLinks");
                break;
            case ArduinoButtons.ObstacleRight:
                if (ObstaclesAroundCar[1] != ObstacleTyps.NoObstacle)
                    FindObjectOfType<AudioManager>().Play("HindernisRechts");
                break;
            case ArduinoButtons.ExitDirectionLeft:
                if (ExitDirection != ExitDirections.NoValue)
                    FindObjectOfType<AudioManager>().Play("AusstiegLinks");
                break;
            case ArduinoButtons.ExitDirectionRight:
                if (ExitDirection != ExitDirections.NoValue)
                    FindObjectOfType<AudioManager>().Play("AusstiegRechts");
                break;
            case ArduinoButtons.ReasonForCurrentStop:
                PlayAudioReasonForCurrentStop(ReasonForCurrentStop);
                break;
            case ArduinoButtons.DestinationSlider:
                PlayAudioTimeTillArrival();
                break;
            default:
                Debug.Log("Function PlayAudioButton called but no button registered!");
                break;
        }
    }







    // Tactile Display & Communication functions 

    private void TactileDisplay_Command_LoopFunction()
    {
        // Check if variables have changed
        if (CurrentStateChanges.Count > 0)
        {
            //Debug.Log("UnityArduinoSerialCommunication_FixedUpdate Count should be > 1 -> Current value: " + CurrentStateChanges.Count.ToString());

            foreach (StateChanges state in CurrentStateChanges.ToArray())
            {
                switch (state)
                {
                    case StateChanges.ObstaclesAroundCar_ValueChanged:
                        WriteToArduino("OAC " +
                            ((int)this.ObstaclesAroundCar[0]).ToString() +
                            ((int)this.ObstaclesAroundCar[1]).ToString()
                            );
                        break;
                    case StateChanges.ExitDirection_ValueChanged:
                        WriteToArduino("ED " + ((int)this.ExitDirection).ToString());
                        break;
                    case StateChanges.ReasonForCurrentStop_ValueChanged:
                        WriteToArduino("RFCS " + ((int)this.ReasonForCurrentStop).ToString());
                        break;
                    case StateChanges.UpcommingCurve_ValueChanged:
                        WriteToArduino("UC " + this.UpcommingCurveInDegree.ToString());
                        break;
                    case StateChanges.DurationUntilArrival_ValueChanged:
                        WriteToArduino("DUA " + this.DurationUntilArrivalInPercentage.ToString());
                        break;
                    case StateChanges.ArrivalVerification_ValueChanged:
                        WriteToArduino("AV " + this.ArrivalVerification.ToString());
                        break;
                    case StateChanges.RelationCarAndDestinationPosition_ValueChanged:
                        WriteToArduino("RCADP " + this.RelationCarAndDestinationPositionInDegree.ToString());
                        break;
                    default:
                        Debug.Log("Unregistered StateChange");
                        break;
                }

                response = ReadFromArduino(50);
                Debug.Log(response);
                CurrentStateChanges.RemoveAll(currentState => currentState.Equals(state));

                //Debug.Log("UnityArduinoSerialCommunication_FixedUpdate_Count should be 0 -> Current value:" + CurrentStateChanges.Count.ToString());
            }
        }
    }

    private void TactileDisplay_CheckButtonStates_LoopFunction()
    {
        // Call Arduino to check for user-input
        if (FixedUpdateLoopGetButtonStatesControlVariable >= FixedUpdateLoopGetButtonStatesControlVariableMaxValue)
        {
            FixedUpdateLoopGetButtonStatesControlVariable = 0;

            //Debug.Log("GetButtonStates");
            WriteToArduino("GBS");
            response = ReadFromArduino(50);

            if (response != "ACK" && response != null)
            {
                LatestRegisteredButtonPress = (ArduinoButtons)Enum.Parse(typeof(ArduinoButtons), response);
                //RegisteredButtonPresses.Add((ArduinoButtons)Enum.Parse(typeof(ArduinoButtons), response));

                //Log in csv file
                LogDataInCSV("Button pressed", LatestRegisteredButtonPress.ToString());

                Debug.Log(LatestRegisteredButtonPress);
                //Debug.Log("RegisteredButtonPresses.Count: " + RegisteredButtonPresses.Count);

                PlayAudioButton(LatestRegisteredButtonPress);
            }
        }
        else
        {
            FixedUpdateLoopGetButtonStatesControlVariable++;

            //Test: später löschen
            LatestRegisteredButtonPress = ArduinoButtons.NoUserInput;
            //RegisteredButtonPresses.Clear();
        }
    }

    // Set-functions that set the private variables
    public void SetObstaclesAroundCar(ObstacleTyps[] _ObstaclesAroundCar)
    {
        this.ObstaclesAroundCar = _ObstaclesAroundCar;
        this.CurrentStateChanges.Add(StateChanges.ObstaclesAroundCar_ValueChanged);
    }
    public void SetExitDirection(ExitDirections _ExitDirection)
    {
        this.ExitDirection = _ExitDirection;
        this.CurrentStateChanges.Add(StateChanges.ExitDirection_ValueChanged);
    }
    public void SetReasonForCurrentStop(StoppingReasons _ReasonForCurrentStop)
    {
        this.ReasonForCurrentStop = _ReasonForCurrentStop;
        this.CurrentStateChanges.Add(StateChanges.ReasonForCurrentStop_ValueChanged);
    }
    public void SetUpcommingCurveInDegree(int _UpcommingCurveInDegree)
    {
        this.UpcommingCurveInDegree = _UpcommingCurveInDegree;
        this.CurrentStateChanges.Add(StateChanges.UpcommingCurve_ValueChanged);
    }
    public void SetDurationUntilArrivalInPercentage(int _DurationUntilArrivalInPercentage)
    {
        this.DurationUntilArrivalInPercentage = _DurationUntilArrivalInPercentage;
        this.CurrentStateChanges.Add(StateChanges.DurationUntilArrival_ValueChanged);
    }
    public void SetDurationUntilArrivalTotalTimeInMinutes(int _DurationUntilArrivalTotalTimeInMinutes)
    {
        this.DurationUntilArrivalTotalTimeInMinutes = _DurationUntilArrivalTotalTimeInMinutes;
    }
    public void SetArrivalVerification(bool _ArrivalVerification)
    {
        this.ArrivalVerification = _ArrivalVerification;
        this.CurrentStateChanges.Add(StateChanges.ArrivalVerification_ValueChanged);
    }
    public void SetRelationCarAndDestinationPositionInDegree(int _RelationCarAndDestinationPositionInDegree)
    {
        this.RelationCarAndDestinationPositionInDegree = _RelationCarAndDestinationPositionInDegree;
        this.CurrentStateChanges.Add(StateChanges.RelationCarAndDestinationPosition_ValueChanged);
    }

    // Initialise the serial port
    public void Open()
    {
        stream = new SerialPort(port, baudrate);
        stream.ReadTimeout = 50;
        stream.Open();
    }

    // Writes string to serial port (synchron)
    public void WriteToArduino(string message)
    {
        stream.WriteLine(message);
        stream.BaseStream.Flush();
    }

    // Reads from serial port (synchron)
    public string ReadFromArduino(int timeout = 0)
    {
        stream.ReadTimeout = timeout;
        try
        {
            stream.DiscardInBuffer();
            return stream.ReadLine();
        }
        catch (TimeoutException)
        {
            return null;
        }
    }

    public void Close()
    {
        stream.Close();
    }
}