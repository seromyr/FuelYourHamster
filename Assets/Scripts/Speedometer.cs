using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Speedometer : MonoBehaviour
{
    // speedometer variables
    public Transform needleTransform;
    public Text distanceDisplay;
    private float ZERO_ANGLE;
    private float rotationSpeed;
    
    // placeholder hamster variables
    private float speed;
    private float distance;

    // enums for different speeds and their proper rotations for the display
    private enum Speed { Zero = -125, Twenty = -94, Forty = -65, Sixty = -35, Eighty = 0, Hundred = 32, HundredTwenty = 64, HundredForty = 94 };
    private Speed targetRotation;

    // Start is called before the first frame update
    void Start()
    {
        speed = 0;
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
        distanceDisplay.text = "" + distance;

        // change target based on input
        if (Input.GetKeyDown(KeyCode.UpArrow)) SpeedUp();
        if (Input.GetKeyDown(KeyCode.DownArrow)) SpeedDown();
        
        // rotate needle towards target display speed
        needleTransform.localRotation = Quaternion.Lerp(needleTransform.localRotation, Quaternion.Euler((float)targetRotation, 0 , 0), rotationSpeed * Time.deltaTime);
    }

    public void SpeedUp()
    {
        if (speed < 140)  speed += 20;
        UpdateTargetRotation();
    }

    public void SpeedDown()
    {
        if (speed != 0) speed -= 20;
        UpdateTargetRotation();
    }

    private void UpdateTargetRotation()
    {
        switch (speed)
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
