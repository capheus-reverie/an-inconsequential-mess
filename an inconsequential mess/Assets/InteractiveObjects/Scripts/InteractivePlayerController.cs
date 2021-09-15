using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractivePlayerController : MonoBehaviour
{
    #region Raycast Initialization

    [Header("Raycasting Initialization")]
    // Layer Mask Selection
    public LayerMask layerMask;

    // Gaze Selection Parameters
    [Range(0f, 100f)]
    public float maxDistance = 10f;
    public float gazeSelectionTime = 0.3f;
    private float timer = 0f;

    // Gaze Vector Setup
    private Vector3 gazeVector = new Vector3(0.5f, 0.5f, 0f);
    private Ray gazeRay;

    // Object Selection Setup
    private GameObject SelectedObject;
    private ObjectController selectedObjectController;
    private string RaycastReturn;
    private RaycastHit hitData;

    #endregion

    #region Vector Display Initialization

    [Header("Dot Vector Display Initialization")]

    public GameObject leftDotVectorDisplay;
    public GameObject rightDotVectorDisplay;
    public dotVectorDisplay leftDot;
    public dotVectorDisplay rightDot;
    public Slider leftYRotationSlider;
    public Slider rightYRotationSlider;

    [Header("")]

    public GameObject leftHandControl;
    private handControl leftHandDelta;
    public GameObject rightHandControl;
    private handControl rightHandDelta;

    #endregion

    #region Variables

    // Left Hand Variables. NOTE: No use of Z Axis currently.
    private float leftXVal;
    private float leftYVal;
    private float leftZVal;
    private float leftRotYVal;

    // Right Hand Variables. NOTE: No use of Z Axis currently.
    private float rightXVal;
    private float rightYVal;
    private float rightZVal;
    private float rightRotYVal;

    [Header("")]
    [Header("Minimum and Maximum Values as used in Wwise")]

    // Mathf Clamp variables to match Wwise Variables
    public float fMin = 0f;
    public float fMax = 100f;

    // Left Hand Button Bools
    private bool leftTriggerValue = false;
    private bool leftGripValue = false;
    private bool leftPrimaryValue = false;
    private bool leftSecondaryValue = false;
    private bool leftJoystickClickValue = false;

    private int leftPrimarySwitch = 0;

    // Right Hand Button Bools
    private bool rightTriggerValue = false;
    private bool rightGripValue = false;
    private bool rightPrimaryValue = false;
    private bool rightSecondaryValue = false;
    private bool rightJoystickClickValue = false;

    private int rightPrimarySwitch = 0;

    // Set initial update integer to make sure that initialization only occurs once per interaction as opposed to each frame
    private int initialLeftUpdate = 0;
    private int initialRightUpdate = 0;

    // objectAdmin bools
    private bool objectMute = false;
    private bool objectPlay = false;

	#endregion

	#region Unity Methods

	void Start()
    {
        // Initialize hand controls
        leftHandDelta = leftHandControl.GetComponent<handControl>();
        rightHandDelta = rightHandControl.GetComponent<handControl>();

        // Hide interaction display
        leftDotVectorDisplay.SetActive(false);
        rightDotVectorDisplay.SetActive(false);

    }

    void Update()
    {
        // Update Raycast
        gazeRay = Camera.main.ViewportPointToRay(gazeVector);

        // If the ray hits something less than maxDistance, on the chosen layerMask, and not a trigger
        if (Physics.Raycast(gazeRay, out hitData, maxDistance, layerMask, QueryTriggerInteraction.Ignore))
		{
            // Check if the object hit has changed. Update to new object if true, else initialize gaze select.
            if (RaycastReturn != hitData.collider.gameObject.name)
			{
                // Identify GameObject hit by raycast
                RaycastReturn = hitData.collider.gameObject.name;
                Debug.Log(RaycastReturn);

                // Assign the selected object to this GameObject
                SelectedObject = GameObject.Find(RaycastReturn);
                selectedObjectController = SelectedObject.GetComponent<ObjectController>();

                // reset interaction
                deactivate("gaze");
			}
			else
			{
                // Countdown to selection
                timer += Time.deltaTime;

                // After alloted time interval for gaze select is reached, initialize interaction
                if (timer >= gazeSelectionTime)
				{
                    // Identify controllers
                    // Assign Left Hand
                    var leftHandDevices = new List<UnityEngine.XR.InputDevice>();
                    UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.LeftHand, leftHandDevices);
                    UnityEngine.XR.InputDevice leftHandController = leftHandDevices[0];

                    // Assign Right Hand
                    var rightHandDevices = new List<UnityEngine.XR.InputDevice>();
                    UnityEngine.XR.InputDevices.GetDevicesAtXRNode(UnityEngine.XR.XRNode.RightHand, rightHandDevices);
                    UnityEngine.XR.InputDevice rightHandController = rightHandDevices[0];

                    // Assess button selections and initialize object control with grip buttons
                    // If left grip is pressed, but not left trigger
                    if (leftHandController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out leftGripValue) && leftGripValue &!leftTriggerValue)
					{
                        string hand = "left";
                        while (initialLeftUpdate < 1)
						{
                            initializeObjectController(hand);
                            initialLeftUpdate++;
						}

                        handInteraction(hand);
					}
					else
					{
                        deactivate("left");
					}
                    // If right grip is pressed, but not right trigger
                    if (rightHandController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.gripButton, out rightGripValue) && rightGripValue &!rightTriggerValue)
					{
                        while (initialRightUpdate < 1)
						{
                            initializeObjectController("right");
                            initialRightUpdate++;
						}

                        handInteraction("right");
					}
					else
					{
                        deactivate("right");
					}

                    // If left trigger is pressed, but not the grip
                    if (leftHandController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out leftTriggerValue) && leftTriggerValue & !leftGripValue)
                    {
                        Debug.Log("Left Trigger Pressed");
                    }
                    // if right trigger is pressed, but not the grip
                    if (rightHandController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.triggerButton, out rightTriggerValue) && rightTriggerValue & !rightGripValue)
                    {
                        Debug.Log("Right Trigger Pressed");
                    }

                    // if "x" is pressed
                    if (leftHandController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out leftPrimaryValue) && leftPrimaryValue)
                    {
                        while(leftPrimarySwitch < 1)
						{
                            Debug.Log("x Button Pressed");
                            objectAdmin("mute");
                            leftPrimarySwitch++;
                        }
                    }
					else
					{
                        leftPrimarySwitch = 0;
					}

                    // if "y" is pressed
                    if (leftHandController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out leftSecondaryValue) && leftSecondaryValue)
                    {
                        Debug.Log("y Button Pressed");
                    }

                    // if "a" is pressed
                    if (rightHandController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primaryButton, out rightPrimaryValue) && rightPrimaryValue)
                    {
                        while (rightPrimarySwitch < 1)
                        {
                            Debug.Log("a Button Pressed");
                            objectAdmin("play");
                            rightPrimarySwitch++;
                        }
                    }
					else
					{
                        rightPrimarySwitch = 0;
					}
                    // if "b" is pressed
                    if (rightHandController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.secondaryButton, out rightSecondaryValue) && rightSecondaryValue)
                    {
                        Debug.Log("b Button Pressed");
                    }

                    // if left joystick is pressed
                    if (leftHandController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out leftJoystickClickValue) && leftJoystickClickValue)
                    {
                        Debug.Log("Left Joystick Clicked");
                    }
                    // if right joystick is pressed
                    if (rightHandController.TryGetFeatureValue(UnityEngine.XR.CommonUsages.primary2DAxisClick, out rightJoystickClickValue) && rightJoystickClickValue)
                    {
                        Debug.Log("Right Joystick Clicked");
                    }

                }
			}
		}
		else
		{
            deactivate("gaze");
		}

    }

    void initializeObjectController(string hand)
	{
        switch (hand)
		{
            case "left":
                // Activate left hand vector display and left hand control
                leftHandDelta.interactionActive(1);
                leftDotVectorDisplay.SetActive(true);

                // Update left vector display to current audio parameters
                leftXVal = selectedObjectController.leftXAxis;
                leftYVal = selectedObjectController.leftYAxis;
                leftZVal = selectedObjectController.leftZAxis;
                leftRotYVal = selectedObjectController.leftYAxisRotation;
                break;

            case "right":
                // Activate right hand vector display and right hand control
                rightHandDelta.interactionActive(1);
                rightDotVectorDisplay.SetActive(true);

                // Update right vector display to current audio parameters;
                rightXVal = selectedObjectController.rightXAxis;
                rightYVal = selectedObjectController.rightYAxis;
                rightZVal = selectedObjectController.rightZAxis;
                rightRotYVal = selectedObjectController.rightYAxisRotation;
                break;

            default:
                break;
        } 
	}
    void deactivate(string hand)
	{
        switch (hand)
		{
            case "left":
                // Disable Left Controller Interactions and visible vector display
                leftHandDelta.interactionActive(2);
                leftDotVectorDisplay.SetActive(false);

                // Reset initial update int
                initialLeftUpdate = 0;
                break;

            case "right":
                // Disable Right Controller Interactions and visible vector display
                rightHandDelta.interactionActive(2);
                rightDotVectorDisplay.SetActive(false);

                // Reset initial update int
                initialRightUpdate = 0;
                break;

            case "gaze":
                // Reset timer int
                timer = 0;
                break;

            default:
                break;
        } 
	}

    void handInteraction(string hand)
	{
        switch (hand)
		{
            // Update left hand
            case "left":

                //Update X axis interaction on Left Hand
                float deltaXLeft = leftHandDelta.deltaXVal + selectedObjectController.leftXAxis; // Change this to speed in new project
                leftXVal = Mathf.Clamp(deltaXLeft, fMin, fMax);
                selectedObjectController.leftXAxis = leftXVal;

                // Update Y axis interaction on Left Hand
                float deltaYLeft = leftHandDelta.deltaYVal + selectedObjectController.leftYAxis;
                leftYVal = Mathf.Clamp(deltaYLeft, fMin, fMax);
                selectedObjectController.leftYAxis = leftYVal;

                // Update Z Axis interaction on Left Hand
                float deltaZLeft = leftHandDelta.deltaZVal + selectedObjectController.leftZAxis;
                leftZVal = Mathf.Clamp(deltaZLeft, fMin, fMax);
                selectedObjectController.leftZAxis = leftZVal;

                // Update Y Rotation interaction on Left Hand
                float deltaRotYLeft = selectedObjectController.leftYAxisRotation - leftHandDelta.deltaRotYVal;
                leftRotYVal = Mathf.Clamp(deltaRotYLeft, fMin, fMax);
                selectedObjectController.leftYAxisRotation = leftRotYVal;

                // Post variables to dot vector display
                leftDot.xPos = (leftXVal) / 100;
                leftDot.yPos = (leftYVal) / 100;
                leftDot.zPos = (leftZVal) / 100;
                leftYRotationSlider.value = leftRotYVal;

                break;

            case "right":

                //Update X axis interaction on Right Hand
                float deltaXRight = rightHandDelta.deltaXVal + selectedObjectController.rightXAxis; 
                rightXVal = Mathf.Clamp(deltaXRight, fMin, fMax);
                selectedObjectController.rightXAxis = rightXVal;

                // Update Y axis interaction on Right Hand.
                float deltaYRight = rightHandDelta.deltaYVal + selectedObjectController.rightYAxis;
                rightYVal = Mathf.Clamp(deltaYRight, fMin, fMax);
                selectedObjectController.rightYAxis = rightYVal;

                // Update Z axis interaction on Right Hand
                float deltaZRight = rightHandDelta.deltaZVal + selectedObjectController.rightZAxis;
                rightZVal = Mathf.Clamp(deltaZRight, fMin, fMax);
                selectedObjectController.rightZAxis = rightZVal;

                // Update Y Rotation interaction on Left Hand
                float deltaRotYRight = selectedObjectController.rightYAxisRotation - rightHandDelta.deltaRotYVal;
                rightRotYVal = Mathf.Clamp(deltaRotYRight, fMin, fMax);
                selectedObjectController.rightYAxisRotation = rightRotYVal;

                // Post variables to dot vector display
                rightDot.xPos = (rightXVal) / 100;
                rightDot.yPos = (rightYVal) / 100;
                rightDot.zPos = (rightZVal) / 100;
                rightYRotationSlider.value = rightRotYVal;

                break;

            default:
                break;
        }
	}

    void objectAdmin(string state)
    {
        switch (state)
        {
            case "mute":
                if (objectMute)
                {
                    selectedObjectController.objectAdmin("unmute");
                    objectMute = false;
                }
                else
                {
                    selectedObjectController.objectAdmin("mute");
                    objectMute = true;
                }
                break;
         
            case "play":
                if (!objectPlay)
				{
                    selectedObjectController.objectAdmin("play");
                    objectPlay = true;
				}
				else
				{
                    selectedObjectController.objectAdmin("stop");
                    objectPlay = false;
				}
                break;

            default:
                break;
        }
    }

    #endregion

}
