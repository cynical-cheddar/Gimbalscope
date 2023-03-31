using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
public class IdTest : MonoBehaviour
{
    bool lastBackwards = false;
    public AudioClip fireSound;
    public AudioClip advanceSound;
    public AudioClip triggerEnabledSound;

    public AudioClip debugSelectedSound;
    public AudioClip calibratedSound;


    public Text cueTypeLabel;
    public Text motorSaturationText;
    public Text maxTrialsText;
    public Text currentTrialNumberText;

    int lastTargetServoAngle = 0;

    public int targetServoAngle = 120;

    public int currentTrialNumber = 0;

    List<Vector3> currentOrientationKeyframes = new List<Vector3>();

    List<Vector3> currentOrientationStream = new List<Vector3>();


    int currentRequestsCount = 0;

    public List<Transform> targetTransforms = new List<Transform>();
    public Transform currentTargetTransform;
    public int trialsPerTarget = 1;

    public Material targetDefaultMat;
    public Material targetHighlightedMat;



    [Serializable]
    public struct CompletedIdentificaionTrialsWrapper
    {
        public CompletedTrial[] completedTrials;
    }

    [Serializable]
    public struct CueSettings
    {
        public int targetOrientationLeft;
        public int targetOrientationRight;
        public int fireRotationSpeed;
        public int reloadTargetOrientationLeft;
        public int reloadTargetOrientationRight;
        public int reloadRotationalSpeed;
        public int firePrecision;
        public int reloadPrecision;
    }


    [Serializable]
    public struct TrialSettings
    {
        public int trial_id;
        public int identificationOrientation;
    }

    [Serializable]
    public struct CompletedTrial
    {
        [SerializeField]
        public int trial_id;
        [SerializeField]
        public int trial_number;
        [SerializeField]
        public Vector3[] orientationKeyframes;
        [SerializeField]
        public Vector3[] orientationStream;
        [SerializeField]
        public int identification_target_angle;
        [SerializeField]
        public bool correct;
        [SerializeField]
        public int requestsCount;
    }

    public int correctHits = 0;
    public int incorrectHits = 0;

    [SerializeField]

    public CueSettings currentSettings;

    public CueSettings tiltPreset;
    public CueSettings tiltPresetBackward;


    SerialCommunicator serialCommunicator;


    public List<TrialSettings> trialQueue;
    public TrialSettings currentTrial;

    [SerializeField]
    public List<CompletedTrial> completedTrials;


    [SerializeField]
    CompletedIdentificaionTrialsWrapper completedTrialsWrapper;


    public void ReceiveOrientationStreamPacket(Vector3 rot_packet)
    {
        currentOrientationStream.Add(rot_packet);
    }

    private void Start()
    {
        serialCommunicator = GetComponent<SerialCommunicator>();
        currentSettings = tiltPreset;
        GenerateTrials();
        SetTrial();
    }


    public void FireRequest()
    {
        bool backwardsFire = false;
        // add request:
        currentRequestsCount += 1;

        // add current orientation
        currentOrientationKeyframes.Add(serialCommunicator.deviceRepresentation.transform.rotation.eulerAngles);

        Quaternion deviceOrientation = serialCommunicator.deviceRepresentation.transform.rotation;
        // send new settings
        // calculate the degrees difference in the horizontal plane between the forward vector of the device and the target position
        Transform device = serialCommunicator.deviceRepresentation.transform;

        Vector3 targetPos = currentTargetTransform.position;
        Vector3 forward = serialCommunicator.deviceRepresentation.transform.forward;
        //Vector3 angles = Quaternion.FromToRotation(forward, targetPos - device.position).eulerAngles;
        float angle = Vector3.SignedAngle(forward, targetPos -device.position, Vector3.up);
        float angleRecord = Vector3.SignedAngle(forward, targetPos - device.position, Vector3.up);
        
        float send_angle = angle;
        Debug.Log("ANGLES");
        Debug.Log(angle);
        
        if(angle > 135)
        {
            angle = 135;
            send_angle = angle;
        }
        if(angle < 0 && angle > -45)
        {
            angle = 0;
            send_angle = angle;
        }
        if(angle < 0)
        {
            send_angle = 180 - (-1*angle);
            backwardsFire = true;
        }
        Debug.Log("send_angle");
        Debug.Log(send_angle);
        
        /*
        if (yAngleDiff > 180)
        {
            Debug.Log("yAngleDiff > 180");
            yAngleDiff = (360 - yAngleDiff);
            yAngleDiff *= -1;
            Debug.Log(yAngleDiff);
            backwardsFire = true;
            // FIX
            yAngleDiff += 180-30;
            Debug.Log(yAngleDiff);
        }

        else
        {
            backwardsFire=false;
        }
        */
        // send fire command

        if (backwardsFire)
        {
            currentSettings = tiltPreset;
        }
        
        else
        {
            currentSettings = tiltPresetBackward;
        }
        targetServoAngle = (int)(send_angle + 30);

        if (targetServoAngle < 30) targetServoAngle = 30;

        if (targetServoAngle > 150) targetServoAngle = 150;

        // signal the servo to move
        SerialCommunicatorServo();
       // PopulateSettings(currentSettings, true);
        
        if (backwardsFire != lastBackwards)
        {
            // relaod gimbals
            PopulateSettings(currentSettings, true);
        }
        else
        {
            // dont relaod gimbals
            PopulateSettings(currentSettings, false);
        }
        
        

        lastBackwards = backwardsFire;
        lastTargetServoAngle = targetServoAngle;
    }


    public void AddRequestStat()
    {
        currentRequestsCount += 1;
    }



    // generate a list of trials (targets)
    void GenerateTrials()
    {
        int id = 0;
        foreach (Transform target in targetTransforms)
        {
            for (int i = 0; i < trialsPerTarget; i++)
            {
                TrialSettings ts = new TrialSettings();
                ts.trial_id = id;
                ts.identificationOrientation = target.GetComponent<TargetScript>().orientation;


                trialQueue.Add(ts);
                id++;
            }
        }
        maxTrialsText.text = id.ToString();

        // shuffle the queue
        trialQueue = Fisher_Yates_CardDeck_Shuffle(trialQueue);




    }



    public List<TrialSettings> Fisher_Yates_CardDeck_Shuffle(List<TrialSettings> aList)
    {

        System.Random _random = new System.Random();

        TrialSettings myGO;

        int n = aList.Count;
        for (int i = 0; i < n; i++)
        {
            // NextDouble returns a random number between 0 and 1.
            // ... It is equivalent to Math.random() in Java.
            int r = i + (int)(_random.NextDouble() * (n - i));
            myGO = aList[r];
            aList[r] = aList[i];
            aList[i] = myGO;
        }

        return aList;
    }

    public void CorrectCue()
    {
        serialCommunicator.recordMovements = false;
        correctHits++;
        CompletedTrial completedTrial = new CompletedTrial();
        completedTrial.trial_number = currentTrialNumber;
        completedTrial.trial_id = currentTrial.trial_id;
        completedTrial.correct = true;
        completedTrial.requestsCount = currentRequestsCount;
        completedTrial.identification_target_angle = currentTrial.identificationOrientation;
        Vector3[] orientations = currentOrientationKeyframes.ToArray();
        completedTrial.orientationKeyframes = orientations;

        Vector3[] orientation_stream = currentOrientationStream.ToArray();
        completedTrial.orientationStream = orientation_stream;

        currentOrientationKeyframes.Clear();
        currentOrientationStream.Clear();
        completedTrials.Add(completedTrial);
        AdvanceCue();
        Debug.Log("right");
    }
    public void IncorrectCue()
    {
        serialCommunicator.recordMovements = false;
        if (currentTrialNumber <= trialQueue.Count)
        {
            incorrectHits++;
            CompletedTrial completedTrial = new CompletedTrial();
            completedTrial.trial_number = currentTrialNumber;
            completedTrial.trial_id = currentTrial.trial_id;
            completedTrial.correct = false;
            completedTrial.identification_target_angle = currentTrial.identificationOrientation;
            completedTrial.requestsCount = currentRequestsCount;
            Vector3[] orientations = currentOrientationKeyframes.ToArray();
            completedTrial.orientationKeyframes = orientations;

            Vector3[] orientation_stream = currentOrientationStream.ToArray();
            completedTrial.orientationStream = orientation_stream;
            currentOrientationKeyframes.Clear();
            currentOrientationStream.Clear();
            completedTrials.Add(completedTrial);
            AdvanceCue();
            Debug.Log("wrong");
        }
    }

    public void SetTrial()
    {
        if (currentTrialNumber >= trialQueue.Count)
        {
            Debug.Log("DONE");
            foreach (Transform t in targetTransforms)
            {
                t.GetComponent<MeshRenderer>().material = targetDefaultMat;
            }
        }
        else
        {
            // prompt the user to recalibrate the device

            // set the new trial settings
            currentTrial = trialQueue[currentTrialNumber];

            // set the new target from which we can base our calculations off
            int rot = currentTrial.identificationOrientation;
            foreach (Transform t in targetTransforms)
            {
                if (t.GetComponent<TargetScript>().orientation == rot)
                {
                    currentTargetTransform = t;
                    Debug.Log("set new target transform");
                    Debug.Log(currentTargetTransform);
                    t.GetComponent<MeshRenderer>().material = targetHighlightedMat;
                }
                else
                {
                    t.GetComponent<MeshRenderer>().material = targetDefaultMat;
                }
            }

        }
        cueTypeLabel.text = currentTrial.identificationOrientation.ToString();
        currentTrialNumberText.text = currentTrialNumber.ToString();
        motorSaturationText.text = " 20%";
        PopulateSettingsNoFire(currentSettings);
        serialCommunicator.btn_set_settings_command_Click();
    }

    public void AdvanceCue()
    {

        currentTrialNumber++;
        SetTrial();
    }

    void SerialCommunicatorSetGimbalsToReload()
    {
        serialCommunicator.btn_go_to_reload_pos_click();
    }

    void SerialCommunicatorSetSettings()
    {
        serialCommunicator.btn_set_settings_command_Click();
    }
    void SerialCommunicatorServo()
    {
        Debug.Log("SERVO");
        serialCommunicator.servoInputField.text = targetServoAngle.ToString();
        serialCommunicator.SetServoAngle(targetServoAngle);
    }
    void SerialCommunicatorBrushless()
    {
        serialCommunicator.SetBrushlessPWM(0, serialCommunicator.dualBrushlessSlider.value);

    }

    void SerialReturnToZeroGimbals()
    {
        serialCommunicator.btn_return_to_zero_click();
    }
    void SerialFire()
    {
        reloaded = false;
        PlayFireSound();
        serialCommunicator.btn_fire_command_Click();
    }

    public void btn_populate_settings()
    {
        PopulateSettings(currentSettings, true);
    }

    float SaturationToPwm(int saturation)
    {
        float maxPwm = serialCommunicator.dualBrushlessSlider.maxValue;
        float minPwm = serialCommunicator.dualBrushlessSlider.minValue;

        return (Mathf.Lerp(minPwm, maxPwm, ((float)saturation / 100)));
    }

    public void PopulateSettingsNoFire(CueSettings cueSettings)
    {

        currentSettings = cueSettings;


        serialCommunicator.targetOrientationFieldLeft.text = cueSettings.targetOrientationLeft.ToString();
        serialCommunicator.targetOrientationFieldRight.text = cueSettings.targetOrientationRight.ToString();

        serialCommunicator.fireRotationSpeedField.text = cueSettings.fireRotationSpeed.ToString();

        serialCommunicator.reloadTargetOrientationFieldLeft.text = cueSettings.reloadTargetOrientationLeft.ToString();
        serialCommunicator.reloadTargetOrientationFieldRight.text = cueSettings.reloadTargetOrientationRight.ToString();

        serialCommunicator.reloadRotationalSpeedField.text = cueSettings.reloadRotationalSpeed.ToString();

        serialCommunicator.firePrecisionField.text = cueSettings.firePrecision.ToString();

        serialCommunicator.reloadPrecisionField.text = cueSettings.reloadPrecision.ToString();

        serialCommunicator.brushlessInterpolationTimeField.text = "1";

        Invoke(nameof(SerialReturnToZeroGimbals), 1f);
    }

        public void PopulateSettings(CueSettings cueSettings, bool reloadGimbals)
    {
        
        currentSettings = cueSettings;


        serialCommunicator.targetOrientationFieldLeft.text = cueSettings.targetOrientationLeft.ToString();
        serialCommunicator.targetOrientationFieldRight.text = cueSettings.targetOrientationRight.ToString();

        serialCommunicator.fireRotationSpeedField.text = cueSettings.fireRotationSpeed.ToString();

        serialCommunicator.reloadTargetOrientationFieldLeft.text = cueSettings.reloadTargetOrientationLeft.ToString();
        serialCommunicator.reloadTargetOrientationFieldRight.text = cueSettings.reloadTargetOrientationRight.ToString();

        serialCommunicator.reloadRotationalSpeedField.text = cueSettings.reloadRotationalSpeed.ToString();

        serialCommunicator.firePrecisionField.text = cueSettings.firePrecision.ToString();

        serialCommunicator.reloadPrecisionField.text = cueSettings.reloadPrecision.ToString();

        serialCommunicator.brushlessInterpolationTimeField.text = "1";





        reloaded = false;



        PlayAdvancingToNextTrialSound();
        Invoke(nameof(SerialCommunicatorSetSettings), 0.2f);

        if (reloadGimbals)
        {
            Invoke(nameof(SerialCommunicatorSetGimbalsToReload), 0.8f);
            // Invoke(nameof(SerialFire), 2f);
            StartCoroutine(nameof(WaitForGoSerialFire));
            //  Invoke(nameof(PlayTriggerEnabledSound), 8f);
        }
        else
        {
            Invoke(nameof(SerialCommunicatorSetGimbalsToReload), 0.8f);
            // Invoke(nameof(SerialFire), 2f);
            StartCoroutine(nameof(WaitForGoSerialFire));
         //   Invoke(nameof(PlayTriggerEnabledSound), 5f);
        }
        
    }

    IEnumerator WaitForGoSerialFire()
    {
        yield return new WaitForSeconds(1);
        float elapsedTime = 0f;
        while (true)
        {
            elapsedTime += Time.deltaTime;
            if (reloaded || elapsedTime > 4)
            {
                SerialFire();
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    public void PlayAdvancingToNextTrialSound()
    {
        GetComponent<AudioSource>().clip = (advanceSound);
        GetComponent<AudioSource>().Play();
        Debug.Log("play sound");
    }





    public void PlayDebugSelectedSound()
    {
        GetComponent<AudioSource>().clip = (debugSelectedSound);
        GetComponent<AudioSource>().Play();
    }

    public void PlayCalibratedSound()
    {
        GetComponent<AudioSource>().clip = (calibratedSound);
        GetComponent<AudioSource>().Play();
    }
    public void PlayFireSound()
    {
        GetComponent<AudioSource>().clip = (fireSound);
        GetComponent<AudioSource>().Play();
        Debug.Log("play sound");
    }

    public bool reloaded = false;
    public void PlayTriggerEnabledSound()
    {
        GetComponent<AudioSource>().clip = (triggerEnabledSound);
        GetComponent<AudioSource>().Play();
        Debug.Log("trigger1");
        reloaded = true;
    }
    public string folder = "test_results_dir";
    public string filename = "";
    public void SaveCompletedTrialsToJSON()
    {
        CompletedTrial[] completedTrialsArray = completedTrials.ToArray();

        completedTrialsWrapper.completedTrials = completedTrialsArray;


        String completedTrialsJson = JsonUtility.ToJson(completedTrialsWrapper);
        Debug.Log(completedTrialsJson);

        // we have recorded all of the data in a json

        // save the data to file
        if (filename == "")
        {
            filename = DateTime.Now.ToString();
            filename = filename.Replace("/", "_");
            filename = filename.Replace(":", "_");
            Debug.Log(filename);
        }

        string path = "D:\\Repos\\Gimbalscope\\Gimbalscope\\GimbalscopeControllerUnity\\GimbalScopeController\\Assets\\identification_test_results_dir\\" + filename + ".json";

        File.WriteAllText(path, completedTrialsJson);
    }
}
