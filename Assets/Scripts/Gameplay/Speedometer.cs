using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Constants;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    // speedometer variables
    public Transform needleTransform;
    public Text distanceDisplay;
    private float ZERO_ANGLE;
    private float rotationSpeed;

    // gameplay variables
    [SerializeField]
    private GameObject wheel;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float speedMph;
    [SerializeField]
    private float distance;
    public float Distance { get { return distance; } }


    // enums for different speeds and their proper rotations for the display
    private enum Speed { Zero = -125, Twenty = -94, Forty = -65, Sixty = -35, Eighty = 0, Hundred = 32, HundredTwenty = 64, HundredForty = 94 };
    private Speed targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        wheel = GameObject.Find("Wheel");
        speed = wheel.GetComponent<WheelMechanic>().Speed;
        distance = 0;
        
        // set speed/rotation/angle stuff
        ZERO_ANGLE = needleTransform.rotation.x;
        targetRotation = Speed.Zero;
        rotationSpeed = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        // get/update current distance and update display
        if (speed > 0) distance += speed/60 * Time.deltaTime;
        distanceDisplay.text = distance.ToString();

        // determine the MPH number based on rotation speed
        TranslateSpeed();
        
        // change rotation based on speedMPH
        UpdateTargetRotation();

        // rotate needle towards target display speed
        needleTransform.localRotation = Quaternion.Lerp(needleTransform.localRotation, Quaternion.Euler((float)targetRotation, 0 , 0), rotationSpeed * Time.deltaTime);
    }
    
    public void TranslateSpeed()
    {
        //speed = wheel.GetComponent<WheelMechanic>().Speed;

        //switch (GameManager.main.Difficulty)
        //{
        //    case Difficulty.Kindergarten: speedMph = 20; break;
        //    case Difficulty.Decent: speedMph = 40; break;
        //    case Difficulty.Engaged: speedMph = 60; break;
        //    case Difficulty.Difficult: speedMph = 80; break;
        //    case Difficulty.Lightspeed: speedMph = 100; break;
        //    case Difficulty.Victory: speedMph = 120; break;
        //    default: speedMph = 0; break;
        //}
    }

    private void UpdateTargetRotation()
    {
        switch (speedMph)
        {
            case 0: targetRotation = Speed.Zero; break;
            case 20: targetRotation = Speed.Twenty; break;
            case 40: targetRotation = Speed.Forty; break;
            case 60: targetRotation = Speed.Sixty; break;
            case 80: targetRotation = Speed.Eighty; break;
            case 100: targetRotation = Speed.Hundred; break;
            case 120: targetRotation = Speed.HundredTwenty; break;
            case 140: targetRotation = Speed.HundredForty; break;
        }
    }
    
}
