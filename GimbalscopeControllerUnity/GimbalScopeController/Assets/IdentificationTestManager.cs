using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
public class IdentificaitonTestManager : MonoBehaviour
{

    public AudioClip advanceSound;
    public AudioClip triggerEnabledSound;


    public Text cueTypeLabel;
    public Text motorSaturationText;
    public Text maxTrialsText;
    public Text currentTrialNumberText;

    public int targetServoAngle = 120;

    public int currentTrialNumber = 0;

    List<Vector3> currentOrientationKeyframes = new List<Vector3>();



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
        public float brushlessPWM;
        public int motorSaturation;
        public int targetOrientationLeft;
        public int targetOrientationRight;
        public int fireRotationSpeed;
        public int reloadTargetOrientationLeft;
        public int reloadTargetOrientationRight;
        public int reloadRotationalSpeed;
        public int firePrecision;
        public int reloadPrecision;
        public int brushlessInterpolationTime;
        public float servoAngle;
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



    SerialCommunicator serialCommunicator;


    public List<TrialSettings> trialQueue;
    public TrialSettings currentTrial;

    [SerializeField]
    public List<CompletedTrial> completedTrials;


    [SerializeField]
    CompletedIdentificaionTrialsWrapper completedTrialsWrapper;


    private void Start()
    {
        serialCommunicator = GetComponent<SerialCommunicator>();
        GenerateTrials();
        SetTrial();
    }


    public void FireRequest()
    {
        // add request:
        currentRequestsCount += 1;

        // add current orientation
        currentOrientationKeyframes.Add(serialCommunicator.deviceRepresentation.transform.rotation.eulerAngles);

        Quaternion deviceOrientation = serialCommunicator.deviceRepresentation.transform.rotation;
        // send new settings

        // send fire command
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
        correctHits++;
        CompletedTrial completedTrial = new CompletedTrial();
        completedTrial.trial_number = currentTrialNumber;
        completedTrial.trial_id = currentTrial.trial_id;
        completedTrial.correct = true;
        completedTrial.requestsCount= currentRequestsCount;
        Vector3[] orientations = currentOrientationKeyframes.ToArray();
        completedTrial.orientationKeyframes = orientations;
        currentOrientationKeyframes.Clear();
        completedTrials.Add(completedTrial);
        AdvanceCue();
        Debug.Log("right");
    }
    public void IncorrectCue()
    {
        if (currentTrialNumber <= trialQueue.Count)
        {
            incorrectHits++;
            CompletedTrial completedTrial = new CompletedTrial();
            completedTrial.trial_number = currentTrialNumber;
            completedTrial.trial_id = currentTrial.trial_id;
            completedTrial.correct = false;
            completedTrial.requestsCount = currentRequestsCount;
            Vector3[] orientations = currentOrientationKeyframes.ToArray();
            completedTrial.orientationKeyframes = orientations;
            currentOrientationKeyframes.Clear();
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
    public void btn_populate_settings()
    {
        PopulateSettings(currentSettings);
    }

    float SaturationToPwm(int saturation)
    {
        float maxPwm = serialCommunicator.dualBrushlessSlider.maxValue;
        float minPwm = serialCommunicator.dualBrushlessSlider.minValue;

        return (Mathf.Lerp(minPwm, maxPwm, ((float)saturation / 100)));
    }



    public void PopulateSettings(CueSettings cueSettings)
    {
        currentSettings = cueSettings;

        serialCommunicator.servoInputField.text = cueSettings.servoAngle.ToString(); ;

        serialCommunicator.dualBrushlessSlider.value = SaturationToPwm(20);

        serialCommunicator.targetOrientationFieldLeft.text = cueSettings.targetOrientationLeft.ToString();
        serialCommunicator.targetOrientationFieldRight.text = cueSettings.targetOrientationRight.ToString();

        serialCommunicator.fireRotationSpeedField.text = cueSettings.fireRotationSpeed.ToString();

        serialCommunicator.reloadTargetOrientationFieldLeft.text = cueSettings.reloadTargetOrientationLeft.ToString();
        serialCommunicator.reloadTargetOrientationFieldRight.text = cueSettings.reloadTargetOrientationRight.ToString();

        serialCommunicator.reloadRotationalSpeedField.text = cueSettings.reloadRotationalSpeed.ToString();

        serialCommunicator.firePrecisionField.text = cueSettings.firePrecision.ToString();

        serialCommunicator.reloadPrecisionField.text = cueSettings.reloadPrecision.ToString();

        serialCommunicator.brushlessInterpolationTimeField.text = "1";

        cueTypeLabel.text = currentTrial.identificationOrientation.ToString();

        motorSaturationText.text =  " 20%";

        serialCommunicator.btn_set_settings_command_Click();


        Debug.Log("Trial number: " + currentTrialNumber.ToString() + "  trial ID: " + currentTrial.trial_id.ToString());
        PlayAdvancingToNextTrialSound();
        // Invoke(nameof(SerialCommunicatorSetSettings), 0.2f);
        Invoke(nameof(SerialReturnToZeroGimbals), 1.5f);
        Invoke(nameof(SerialCommunicatorServo), 2f);
        Invoke(nameof(SerialCommunicatorBrushless), 5f);
        Invoke(nameof(SerialCommunicatorSetGimbalsToReload), 7f);
        //Invoke(nameof(PlayTriggerEnabledSound), 9f);
    }

    public void PlayAdvancingToNextTrialSound()
    {
        GetComponent<AudioSource>().clip = (advanceSound);
        GetComponent<AudioSource>().Play();
    }
    public void PlayTriggerEnabledSound()
    {
        GetComponent<AudioSource>().clip = (triggerEnabledSound);
        GetComponent<AudioSource>().Play();
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
