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


    public InputField targetOrientationField;
    public InputField fireRotationSpeedField;
    public InputField reloadTargetOrientationField;
    public InputField reloadRotationalSpeedField;
    public InputField firePrecisionField;
    public InputField reloadPrecisionField;
    public InputField brushlessInterpolationTimeField;

    public Transform deviceRepresentation;
   

    public Button fireButton;


    public String defaultFireTargetOrientation;
    public String defaultFireRotationSpeed;
    public String defaultReloadTargetOrientation;
    public String defaultReloadRotationSpeed;
    public String defaultFirePrecision;
    public String defaultReloadPrecision;
    public String defaultBrushlessInterpolationTime;

    SerialDuplexManager serialDuplexManager;

    void Start () {
        serialDuplexManager = FindObjectOfType<SerialDuplexManager> ();
        SetupDefaultValues();
    }

    void SetupDefaultValues()
    {
        GetTextFromInputField(targetOrientationField).text = defaultFireTargetOrientation;
        GetTextFromInputField(fireRotationSpeedField).text = defaultFireRotationSpeed;
        GetTextFromInputField(reloadTargetOrientationField).text = defaultReloadTargetOrientation;
        GetTextFromInputField(reloadRotationalSpeedField).text = defaultReloadRotationSpeed;
        GetTextFromInputField(firePrecisionField).text = defaultFirePrecision;
        GetTextFromInputField(reloadPrecisionField).text = defaultReloadPrecision;
        GetTextFromInputField(brushlessInterpolationTimeField).text = defaultBrushlessInterpolationTime;

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

    public void btn_fire_command_Click()
    {
        byte motorType = 0;
        byte motorID = 0;
        byte functionID = 0;

        double fireTargetAngle = Single.Parse(GetTextFromInputField(targetOrientationField).text);
        fireTargetAngle = Math.Round(fireTargetAngle, 4);
        //byte[] bytes_fireTargetAngle = BitConverter.GetBytes(fireTargetAngle);

        double fireDegreesPerSecond = Single.Parse(GetTextFromInputField(fireRotationSpeedField).text);
        fireDegreesPerSecond = Math.Round(fireDegreesPerSecond, 4);
        //byte[] bytes_fireDegreesPerSecond = BitConverter.GetBytes(fireDegreesPerSecond);


        string firePrecisonString = GetTextFromInputField(firePrecisionField).text;


        string reloadTargetAngle = GetTextFromInputField(reloadTargetOrientationField).text;


        double reloadDegreesPerSecond = Single.Parse(GetTextFromInputField(reloadRotationalSpeedField).text);
        reloadDegreesPerSecond = Math.Round(reloadDegreesPerSecond, 4);


        string reloadPrecisionString = GetTextFromInputField(reloadPrecisionField).text;



        string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + fireTargetAngle.ToString() + "," + fireDegreesPerSecond.ToString() + "," + firePrecisonString + "," + reloadTargetAngle + "," + reloadDegreesPerSecond.ToString() + "," + reloadPrecisionString + ",";


        System.Diagnostics.Debug.WriteLine("--btn_fire_command_Click--");
        //System.Diagnostics.Debug.WriteLine(commandString);
        byte[] utf8String = Encoding.UTF8.GetBytes(commandString);
        serialDuplexManager.SendMessageViaSerial(commandString);

        Debug.Log(commandString);
    }



    private void SetBrushlessPWM(byte motorID, float targetPWM)
    {
        byte motorType = 2;
        byte functionID = 0;
        string commandString = motorType.ToString() + motorID.ToString() + functionID.ToString() + "," + targetPWM.ToString() + "," + GetTextFromInputField(brushlessInterpolationTimeField).text + ",";

        byte[] utf8String = Encoding.UTF8.GetBytes(commandString);
        serialDuplexManager.SendMessageViaSerial(commandString);
        Debug.Log(commandString);
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
                    //Debug.Log(message);
                    string telemetry = message.Remove(0, 1);
                    string[] orientations = telemetry.Split(',');
                    Debug.Log(telemetry);
                    int x = int.Parse(orientations[0]);
                    int y = int.Parse(orientations[1]);
                    int z = int.Parse(orientations[2]);

                    deviceRepresentation.rotation = Quaternion.Euler(0, -z, 0);
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