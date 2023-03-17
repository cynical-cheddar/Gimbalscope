using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using System.Text;
using UnityEngine.UI;
public class SerialCommunicator : MonoBehaviour {
    public string portName;

    int leftGimbalLastValue = 0;
    int rightGimbalLastValue = 0;
    int dualGimbalLastValue = 0;

    int leftBrushlessLastValue = 70;
    int rightBrushlessLastValue = 70;
    int dualBrushlessLastValue = 70;


    int servoLastValue = 90;

    public Slider leftGimbalSlider;
    public Slider rightGimbalSlider;
    public Slider dualGimbalSlider;
    public Text leftGimbalText;
    public Text rightGimbalText;
    public Text dualGimbalText;


    public Slider leftBrushlessSlider;
    public Slider rightBrushlessSlider;
    public Slider dualBrushlessSlider;
    public Text leftBrushlessText;
    public Text rightBrushlessText;
    public Text dualBrushlessText;

    public InputField servoInputField;



    public InputField targetOrientationFieldLeft;
    public InputField targetOrientationFieldRight;
    public InputField fireRotationSpeedField;
    public InputField reloadTargetOrientationFieldLeft;
    public InputField reloadTargetOrientationFieldRight;
    public InputField reloadRotationalSpeedField;
    public InputField firePrecisionField;
    public InputField reloadPrecisionField;
    public InputField brushlessInterpolationTimeField;

    public Transform deviceRepresentation;


    public Button fireButton;


    public String defaultFireTargetOrientationLeft;
    public String defaultFireTargetOrientationRight;
    public String defaultFireRotationSpeed;
    public String defaultReloadTargetOrientationLeft;
    public String defaultReloadTargetOrientationRight;
    public String defaultReloadRotationSpeed;
    public String defaultFirePrecision;
    public String defaultReloadPrecision;
    public String defaultBrushlessInterpolationTime;

    SerialDuplexManager serialDuplexManager;

    void Start() {
        serialDuplexManager = FindObjectOfType<SerialDuplexManager>();
        SetupDefaultValues();
    }

    void SetupDefaultValues()
    {
        GetTextFromInputField(targetOrientationFieldLeft).text = defaultFireTargetOrientationLeft;
        GetTextFromInputField(targetOrientationFieldRight).text = defaultFireTargetOrientationRight;
        GetTextFromInputField(fireRotationSpeedField).text = defaultFireRotationSpeed;
        GetTextFromInputField(reloadTargetOrientationFieldLeft).text = defaultReloadTargetOrientationLeft;
        GetTextFromInputField(reloadTargetOrientationFieldRight).text = defaultReloadTargetOrientationRight;
        GetTextFromInputField(reloadRotationalSpeedField).text = defaultReloadRotationSpeed;
        GetTextFromInputField(firePrecisionField).text = defaultFirePrecision;
        GetTextFromInputField(reloadPrecisionField).text = defaultReloadPrecision;
        GetTextFromInputField(brushlessInterpolationTimeField).text = defaultBrushlessInterpolationTime;

    }

    void UpdateGimbalSliders()
    {
        leftGimbalSlider.value = (float)Int32.Parse(reloadTargetOrientationFieldLeft.text);
        rightGimbalSlider.value = (float)Int32.Parse(reloadTargetOrientationFieldRight.text);
        leftGimbalLastValue = (int)leftGimbalSlider.value;
        rightGimbalLastValue = (int)rightGimbalSlider.value;
        leftGimbalText.text = leftGimbalLastValue.ToString();
        rightGimbalText.text = rightGimbalLastValue.ToString();
    }

    InputField GetTextFromInputField(InputField field)
    {
        return field;
    }



    private void SetLeftGimbalPosition()
    {
        leftGimbalText.text = leftGimbalSlider.value.ToString();

        byte motorType = 0;
        byte motorID = 1;
        byte functionID = 1;
        float targetAngle = leftGimbalSlider.value;
        float moveSpeed = 90f;
        float precision = 1f;
        string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + targetAngle.ToString() + "," + moveSpeed.ToString() + "," + precision + ",";
        serialDuplexManager.SendMessageViaSerial(commandString);
        Debug.Log(commandString);
    }

    private void SetRightGimbalPosition()
    {
        rightGimbalText.text = rightGimbalSlider.value.ToString();

        byte motorType = 0;
        byte motorID = 2;
        byte functionID = 1;
        float targetAngle = rightGimbalSlider.value;
        float moveSpeed = 90f;
        float precision = 1f;
        string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + targetAngle.ToString() + "," + moveSpeed.ToString() + "," + precision + ",";


        byte[] utf8String = Encoding.UTF8.GetBytes(commandString);
        serialDuplexManager.SendMessageViaSerial(commandString);
        Debug.Log(commandString);
    }

    private void SetBothGimbalPositions()
    {

        byte motorType = 0;
        byte motorID = 0;
        byte functionID = 1;
        float targetAngle = dualGimbalSlider.value;
        float moveSpeed = 90f;
        float precision = 1f;
        string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + targetAngle.ToString() + "," + moveSpeed.ToString() + "," + precision + ",";


        byte[] utf8String = Encoding.UTF8.GetBytes(commandString);
        serialDuplexManager.SendMessageViaSerial(commandString);
        Debug.Log(commandString);
    }

    public void btn_zero_gimbals_command_Click()
    {
        byte motorType = 0;
        byte motorID = 0;
        byte functionID = 3;

        dualGimbalSlider.value = 0;
        leftGimbalSlider.value = 0;
        rightGimbalSlider.value = 0;

        dualGimbalLastValue = 0;
        leftGimbalLastValue = 0;
        rightGimbalLastValue = 0;

        string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + "0000";
        Debug.Log(commandString);
        serialDuplexManager.SendMessageViaSerial(commandString);
    }

    public void btn_set_settings_command_Click()
    {
        byte motorType = 0;
        byte motorID = 0;
        byte functionID = 4;

        double fireTargetAngleLeft = Single.Parse(GetTextFromInputField(targetOrientationFieldLeft).text);
        fireTargetAngleLeft = Math.Round(fireTargetAngleLeft, 4);

        double fireTargetAngleRight = Single.Parse(GetTextFromInputField(targetOrientationFieldRight).text);
        fireTargetAngleRight = Math.Round(fireTargetAngleRight, 4);

        double fireDegreesPerSecond = Single.Parse(GetTextFromInputField(fireRotationSpeedField).text);
        fireDegreesPerSecond = Math.Round(fireDegreesPerSecond, 4);


        string firePrecisonString = GetTextFromInputField(firePrecisionField).text;


        string reloadTargetAngleLeft = GetTextFromInputField(reloadTargetOrientationFieldLeft).text;
        string reloadTargetAngleRight = GetTextFromInputField(reloadTargetOrientationFieldRight).text;

        double reloadDegreesPerSecond = Single.Parse(GetTextFromInputField(reloadRotationalSpeedField).text);
        reloadDegreesPerSecond = Math.Round(reloadDegreesPerSecond, 4);


        string reloadPrecisionString = GetTextFromInputField(reloadPrecisionField).text;



        string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + fireDegreesPerSecond.ToString() + "," + firePrecisonString + "," + reloadDegreesPerSecond.ToString() + "," + reloadPrecisionString + ",";
        // motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + fireDegreesPerSecond.ToString() + "," + firePrecisonString + "," + reloadDegreesPerSecond.ToString() + "," + reloadPrecisionString +, ",";

        System.Diagnostics.Debug.WriteLine("--btn_set_settings_command_Click--");

        serialDuplexManager.SendMessageViaSerial(commandString);

        //  UpdateGimbalSliders();

        Debug.Log(commandString);
    }

    

    public void btn_fire_command_Click()
    {
        byte motorType = 0;
        byte motorID = 0;
        byte functionID = 0;

        double fireTargetAngleLeft = Single.Parse(GetTextFromInputField(targetOrientationFieldLeft).text);
        fireTargetAngleLeft = Math.Round(fireTargetAngleLeft, 4);

        double fireTargetAngleRight = Single.Parse(GetTextFromInputField(targetOrientationFieldRight).text);
        fireTargetAngleRight = Math.Round(fireTargetAngleRight, 4);

        double fireDegreesPerSecond = Single.Parse(GetTextFromInputField(fireRotationSpeedField).text);
        fireDegreesPerSecond = Math.Round(fireDegreesPerSecond, 4);


        string firePrecisonString = GetTextFromInputField(firePrecisionField).text;


        string reloadTargetAngleLeft = GetTextFromInputField(reloadTargetOrientationFieldLeft).text;
        string reloadTargetAngleRight = GetTextFromInputField(reloadTargetOrientationFieldRight).text;

        double reloadDegreesPerSecond = Single.Parse(GetTextFromInputField(reloadRotationalSpeedField).text);
        reloadDegreesPerSecond = Math.Round(reloadDegreesPerSecond, 4);


        string reloadPrecisionString = GetTextFromInputField(reloadPrecisionField).text;



        string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + fireTargetAngleLeft.ToString() + "," + fireTargetAngleRight.ToString() + "," + reloadTargetAngleLeft + "," + reloadTargetAngleRight + ",";
        // motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + fireDegreesPerSecond.ToString() + "," + firePrecisonString + "," + reloadDegreesPerSecond.ToString() + "," + reloadPrecisionString +, ",";

        System.Diagnostics.Debug.WriteLine("--btn_fire_command_Click--");

        serialDuplexManager.SendMessageViaSerial(commandString);

        UpdateGimbalSliders();

        Debug.Log(commandString);
    }

    public void btn_servo_set_command()
    {
        SetServoAngle(int.Parse(servoInputField.text));
    }

    public void SetServoAngle(int angle)
    {
        byte motorType = 1;
        byte motorID = 0;
        byte functionID = 0;

        string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + angle.ToString() + ",";
        Debug.Log(commandString);
        serialDuplexManager.SendMessageViaSerial(commandString);
        Debug.Log(commandString);
    }
    public void SetBrushlessPWM(byte motorID, float targetPWM)
    {
        byte motorType = 2;
        byte functionID = 0;
        string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + targetPWM.ToString() + "," + GetTextFromInputField(brushlessInterpolationTimeField).text + ",";

        byte[] utf8String = Encoding.UTF8.GetBytes(commandString);
        serialDuplexManager.SendMessageViaSerial(commandString);
        Debug.Log(commandString);
    }

    public void btn_return_to_zero_click()
    {
        byte motorType = 0;
        byte motorID = 0;
        byte functionID = 5;

        string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + 'a' + ",";
        Debug.Log(commandString);
        serialDuplexManager.SendMessageViaSerial(commandString);
    }


    private void Update()
    {
        // read duplex manager queue
        if(serialDuplexManager.incomingMessageBuffer.Count > 0)
        {
            string message = (string) serialDuplexManager.incomingMessageBuffer.Dequeue();
            if (message.Length > 2)
            {
                if (message[0] == '#')
                {
                    // telemetry
                    if (message[1] != '#')
                    {
                        //Debug.Log(message);
                        string telemetry = message.Remove(0, 1);
                        string[] orientations = telemetry.Split(',');
                        //Debug.Log(telemetry);
                        int x = int.Parse(orientations[0]);
                        int y = int.Parse(orientations[1]);
                        int z = int.Parse(orientations[2]);

                        deviceRepresentation.rotation = Quaternion.Euler(0, -z, 0);
                    }
                    // requests
                    else if(message[1] == '#')
                    {
                        // cue
                        if(message[2] == 'c')
                        {
                            Debug.Log("cue requested");
                            btn_fire_command_Click();
                        }
                        // target select
                        if(message[2] == 't')
                        {
                            Debug.Log("target selected");
                        }
                        if(message[2] == 'd')
                        {
                            Debug.Log("calibration successful");
                        }
                    }
                }
                else
                {
                    Debug.Log(message);
                }
            }
        }
        

    }


    public float updateInterval = 0.5f;
    float cooldown = 0.5f;
    private void FixedUpdate()
    {
        cooldown -= Time.fixedDeltaTime;
        if (cooldown < 0)
        {
            // gimbal control
            if (dualGimbalLastValue != (int)dualGimbalSlider.value)
            {
                SetBothGimbalPositions();
                dualGimbalLastValue = (int)dualGimbalSlider.value;

                leftGimbalSlider.value = dualGimbalLastValue;
                rightGimbalSlider.value = dualGimbalLastValue;

                dualGimbalText.text = dualGimbalLastValue.ToString();
                leftGimbalText.text = dualGimbalLastValue.ToString();
                rightGimbalText.text = dualGimbalLastValue.ToString();

                leftGimbalLastValue = dualGimbalLastValue;
                rightGimbalLastValue = dualGimbalLastValue;
            }

            else if (leftGimbalLastValue != (int)leftGimbalSlider.value)
            {
                SetLeftGimbalPosition();
                leftGimbalLastValue = (int)leftGimbalSlider.value;
                leftGimbalText.text = leftGimbalSlider.value.ToString();


            }
            else if (rightGimbalLastValue != (int)rightGimbalSlider.value)
            {
                SetRightGimbalPosition();
                rightGimbalLastValue = (int)rightGimbalSlider.value;
                rightGimbalText.text = rightGimbalLastValue.ToString();
            }

            


            // brushless control
            //dual
            if (dualBrushlessLastValue != (int)dualBrushlessSlider.value)
            {
                byte both_id = 0;
                SetBrushlessPWM(both_id, dualBrushlessSlider.value);

                dualBrushlessText.text = dualBrushlessSlider.value.ToString();


                leftBrushlessText.text = leftBrushlessSlider.value.ToString();
                rightBrushlessText.text = rightBrushlessSlider.value.ToString();

                leftBrushlessSlider.value = dualBrushlessSlider.value;
                rightBrushlessSlider.value = dualBrushlessSlider.value;

                dualBrushlessLastValue = (int)dualBrushlessSlider.value;
                leftBrushlessLastValue = (int)dualBrushlessSlider.value;
                rightBrushlessLastValue = (int)dualBrushlessSlider.value;
            }
            // left
            else if (leftBrushlessLastValue != (int)leftBrushlessSlider.value)
            {
                byte left_id = 1;
                SetBrushlessPWM(left_id, leftBrushlessSlider.value);
                leftBrushlessText.text = leftBrushlessSlider.value.ToString();
                leftBrushlessLastValue = (int)leftBrushlessSlider.value;
            }
            //right 
            else if (rightBrushlessLastValue != (int)rightBrushlessSlider.value)
            {
                byte right_id = 2;
                SetBrushlessPWM(right_id, rightBrushlessSlider.value);
                rightBrushlessText.text = rightBrushlessSlider.value.ToString();
                rightBrushlessLastValue = (int)rightBrushlessSlider.value;
            }

            cooldown = updateInterval;
        }
        
    }




}