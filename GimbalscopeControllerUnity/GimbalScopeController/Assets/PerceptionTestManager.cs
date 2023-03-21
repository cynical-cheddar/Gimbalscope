using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
public class PerceptionTestManager : MonoBehaviour
{
    public Text cueTypeLabel;
    public Text motorSaturationText;
    public Text maxTrialsText;
    public Text currentTrialNumberText;

    public int currentTrialNumber = 0;
    
    public int leftTiltTrialCount = 3;
    public int rightTiltTrialCount = 3;
    public int forwardsTrialCount = 3;
    public int backwardsTrialCount = 3;
    public int leftTwistTrialCount = 3;
    public int rightTwistTrialCount = 3;


    public List<int> MotorSaturationValues;

    


    public enum CueType
    {
        leftTilt,
        rightTilt,
        forwardsTilt,
        backwardsTilt,
        leftTwist,
        rightTwist
    }

    [Serializable]
    public struct CompletedTrialsWrapper
    {
        public CompletedTrial[] completedTrials;
    }


    [Serializable]
    public struct CueSettings
    {
        public int trial_id;
        public  CueType cueType;
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
    public struct CompletedTrial
    {
        [SerializeField]
        public int trial_id;
        [SerializeField]
        public int trial_number;
        [SerializeField]
        public CueType cueType;
        [SerializeField]
        public float brushlessPWM;
        [SerializeField]
        public float motorSaturation;
        [SerializeField]
        public bool correct;
    }

    public int correctHits = 0;
    public int incorrectHits = 0;

    [SerializeField]

    public CueSettings currentSettings;

    public CueSettings leftTiltSettingsPreset;

    public CueSettings rightTiltSettingsPreset;

    public CueSettings forwardsTiltSettingsPreset;

    public CueSettings backwardsTiltSettingsPreset;

    public CueSettings leftTwistSettingsPreset;

    public CueSettings rightTwistSettingsPreset;

    SerialCommunicator serialCommunicator;


    public List<CueSettings> trialQueue;

    [SerializeField]
    public List<CompletedTrial> completedTrials;


    [SerializeField]
    CompletedTrialsWrapper completedTrialsWrapper;



    private void Start()
    {
        serialCommunicator = GetComponent<SerialCommunicator>();
        GenerateTrials();

    }

    public void InitialPopulation()
    {
        PopulateSettings(trialQueue[0]);
    }

    float SaturationToPwm(int saturation)
    {
        float maxPwm = serialCommunicator.dualBrushlessSlider.maxValue;
        float minPwm = serialCommunicator.dualBrushlessSlider.minValue;

        return (Mathf.Lerp(minPwm, maxPwm, ((float)saturation / 100)));
    }




    void GenerateTrials()
    {
        currentTrialNumber = 0;
        int trial_id = 0;
        trialQueue.Clear();
        // left tilt
        for (int i = 0; i < (leftTiltTrialCount); i++)
        {
            
            foreach (int saturation in MotorSaturationValues)
            {
                CueSettings setting = leftTiltSettingsPreset;
                setting.brushlessPWM = SaturationToPwm(saturation);
                setting.motorSaturation = saturation;
                setting.cueType = CueType.leftTilt;
                setting.trial_id = trial_id;
                trial_id += 1;
                trialQueue.Add(setting);
            }
        }

        // right tilt
        for (int i = 0; i < (rightTiltTrialCount); i++)
        {

            foreach (int saturation in MotorSaturationValues)
            {
                CueSettings setting = rightTiltSettingsPreset;
                setting.brushlessPWM = SaturationToPwm(saturation);
                setting.motorSaturation = saturation;
                setting.cueType = CueType.rightTilt;
                setting.trial_id = trial_id;
                trial_id += 1;
                trialQueue.Add(setting);
            }
        }
        // forward tilt
        for (int i = 0; i < (forwardsTrialCount); i++)
        {

            foreach (int saturation in MotorSaturationValues)
            {
                CueSettings setting = forwardsTiltSettingsPreset;
                setting.brushlessPWM = SaturationToPwm(saturation);
                setting.motorSaturation = saturation;
                setting.cueType = CueType.forwardsTilt;
                setting.trial_id = trial_id;
                trial_id += 1;
                trialQueue.Add(setting);
            }
        }

        // forward tilt
        for (int i = 0; i < (backwardsTrialCount); i++)
        {

            foreach (int saturation in MotorSaturationValues)
            {
                CueSettings setting = backwardsTiltSettingsPreset;
                setting.brushlessPWM = SaturationToPwm(saturation);
                setting.motorSaturation = saturation;
                setting.cueType = CueType.backwardsTilt;
                setting.trial_id = trial_id;
                trial_id += 1;
                trialQueue.Add(setting);
            }
        }

        // left twist
        for (int i = 0; i < (leftTwistTrialCount); i++)
        {

            foreach (int saturation in MotorSaturationValues)
            {
                CueSettings setting = leftTwistSettingsPreset;
                setting.brushlessPWM = SaturationToPwm(saturation);
                setting.motorSaturation = saturation;
                setting.cueType = CueType.leftTwist;
                setting.trial_id = trial_id;
                trial_id += 1;
                trialQueue.Add(setting);
            }
        }

        // right twist
        for (int i = 0; i < (rightTwistTrialCount); i++)
        {

            foreach (int saturation in MotorSaturationValues)
            {
                CueSettings setting = rightTwistSettingsPreset;
                setting.brushlessPWM = SaturationToPwm(saturation);
                setting.motorSaturation = saturation;
                setting.cueType = CueType.rightTwist;
                setting.trial_id = trial_id;
                trial_id += 1;
                trialQueue.Add(setting);
            }
        }

        trialQueue = Fisher_Yates_CardDeck_Shuffle(trialQueue);
        // check if events are duplicate next to each other
        while (CheckForDuplicateSubsequentCueSettings(trialQueue))
        {
            Debug.Log("reshuffling");
            trialQueue = Fisher_Yates_CardDeck_Shuffle(trialQueue);
        }
        maxTrialsText.text = trial_id.ToString();

    }

    bool CheckForDuplicateSubsequentCueSettings(List<CueSettings> cues)
    {
        CueSettings lastCue = cues[0];
        int i = 0;
        foreach (CueSettings cue in cues)
        {
            if( i > 0)
            {
                if(cue.cueType == lastCue.cueType)
                {
                    return true;
                }
            }
            lastCue = cue;
            i += 1;
        }
        return false;
    }

    public List<CueSettings> Fisher_Yates_CardDeck_Shuffle(List<CueSettings> aList)
    {

        System.Random _random = new System.Random();

        CueSettings myGO;

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
        completedTrial.trial_id = currentSettings.trial_id;
        completedTrial.brushlessPWM = currentSettings.brushlessPWM;
        completedTrial.cueType = currentSettings.cueType;
        completedTrial.correct = true;
        completedTrial.motorSaturation = currentSettings.motorSaturation;
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
            completedTrial.trial_id = currentSettings.trial_id;
            completedTrial.brushlessPWM = currentSettings.brushlessPWM;
            completedTrial.cueType = currentSettings.cueType;
            completedTrial.correct = false;
            completedTrial.motorSaturation = currentSettings.motorSaturation;
            completedTrials.Add(completedTrial);
            AdvanceCue();
            Debug.Log("wrong");
        }
    }


    public void AdvanceCue()
    {
        currentTrialNumber++;
        if (currentTrialNumber >= trialQueue.Count)
        {
            Debug.Log("DONE");
        }
        else
        {
            currentTrialNumberText.text = (currentTrialNumber+1).ToString();
            Debug.Log("advancing trial");
            PopulateSettings(trialQueue[currentTrialNumber]);
        }
    }

    float CalculateSaturation(CueSettings cueSettings)
    {
        float amt = cueSettings.brushlessPWM - serialCommunicator.dualBrushlessSlider.minValue;
        amt /= (serialCommunicator.dualBrushlessSlider.maxValue - serialCommunicator.dualBrushlessSlider.minValue);
        float saturation = Mathf.Lerp(0.0f, 100.0f, amt);
        return saturation;
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
        serialCommunicator.servoInputField.text = currentSettings.servoAngle.ToString();
        serialCommunicator.SetServoAngle((int)currentSettings.servoAngle);
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

    public void PopulateSettings(CueSettings cueSettings)
    {
        currentSettings = cueSettings;

        serialCommunicator.servoInputField.text = cueSettings.servoAngle.ToString(); ;

        serialCommunicator.dualBrushlessSlider.value = cueSettings.brushlessPWM;

        serialCommunicator.targetOrientationFieldLeft.text = cueSettings.targetOrientationLeft.ToString();
        serialCommunicator.targetOrientationFieldRight.text = cueSettings.targetOrientationRight.ToString();

        serialCommunicator.fireRotationSpeedField.text = cueSettings .fireRotationSpeed.ToString();

        serialCommunicator.reloadTargetOrientationFieldLeft.text = cueSettings.reloadTargetOrientationLeft.ToString();
        serialCommunicator.reloadTargetOrientationFieldRight.text = cueSettings.reloadTargetOrientationRight.ToString();

        serialCommunicator.reloadRotationalSpeedField.text = cueSettings.reloadRotationalSpeed.ToString();

        serialCommunicator.firePrecisionField.text = cueSettings.firePrecision.ToString();

        serialCommunicator.reloadPrecisionField.text = cueSettings.reloadPrecision.ToString();

        serialCommunicator.brushlessInterpolationTimeField.text = cueSettings.brushlessInterpolationTime.ToString();

        if(cueSettings.cueType == CueType.leftTilt)
        {
            cueTypeLabel.text = "Left tilt";
        }
        if (cueSettings.cueType == CueType.rightTilt)
        {
            cueTypeLabel.text = "Right tilt";
        }
        if (cueSettings.cueType == CueType.forwardsTilt)
        {
            cueTypeLabel.text = "Forward tilt";
        }
        if (cueSettings.cueType == CueType.backwardsTilt)
        {
            cueTypeLabel.text = "Backwards tilt";
        }
        if (cueSettings.cueType == CueType.leftTwist)
        {
            cueTypeLabel.text = "Left twist";
        }
        if (cueSettings.cueType == CueType.rightTwist)
        {
            cueTypeLabel.text = "Right twist";
        }
        motorSaturationText.text = CalculateSaturation(cueSettings).ToString() + " %";

        serialCommunicator.btn_set_settings_command_Click();


        Debug.Log("Trial number: " + currentTrialNumber.ToString() + "  trial ID: " + currentSettings.trial_id.ToString());

        Invoke(nameof(SerialCommunicatorSetSettings), 0.2f);
        Invoke(nameof(SerialReturnToZeroGimbals), 1.2f);
        Invoke(nameof(SerialCommunicatorServo), 3f);
        Invoke(nameof(SerialCommunicatorBrushless), 4.5f);
        Invoke(nameof(SerialCommunicatorSetGimbalsToReload), 8f);
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
            filename = filename.Replace("/","_");
            filename = filename.Replace(":", "_");
            Debug.Log(filename);
        }

        string path = "D:\\Repos\\Gimbalscope\\Gimbalscope\\GimbalscopeControllerUnity\\GimbalScopeController\\Assets\\test_results_dir\\" + filename + ".json";

        File.WriteAllText(path, completedTrialsJson);
    }
}
