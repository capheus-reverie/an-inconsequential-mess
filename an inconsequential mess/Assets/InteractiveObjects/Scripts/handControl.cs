using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class handControl : MonoBehaviour
{
    #region Variables

    // Assign centre eye camera anchor
    public GameObject CentreCameraAnchor;

    // Assign required transform and rotation variables
    private Vector3 lastPosition;
    private Vector3 currentPosition;
    // private Quaternion lastRotation;
    // private Quaternion currentRotation;

    private Vector3 currentForwardDirection;
    private Vector3 lastForwardDirection;
    private Vector3 currentUpDirection;
    private Vector3 lastUpDirection;
    private Vector3 currentRightDirection;
    private Vector3 lastRightDirection;

    // Assign interaction bool and int
    bool interactionOn = false;
    int initialUpdate = 0;

    // Assign scale variables to convert movement to appropriate changes in values
    public int axisScale = 200;
    public float rotScale = 0.75f;
    public float rotAdjust = 1.5f;
    public float dirAdjust = 0.01f;

    // Assign variables for delta
    public float deltaXVal;
    public float deltaYVal;
    public float deltaZVal;
    public float deltaRotXVal;
    public float deltaRotYVal;
    // public float deltaRotZVal;

    private float xVelocity = 0.0f;
    private float yVelocity = 0.0f;
    private float zVelocity = 0.0f;
    private float rotYVelocity = 0.0f;
    private float rotXVelocity = 0.0f;

    float oldDeltaX = 0.0f;
    float oldDeltaY = 0.0f;
    float oldDeltaZ = 0.0f;
    float oldDeltaRotX = 0.0f;
    float oldDeltaRotY = 0.0f;
    // float oldDeltaRotZ = 0.0f;

    

    #endregion

    #region Unity Methods

    void Start()
    {

    }

    void Update()
    {
        if (interactionOn)
		{
            // Update transform position for X, Y, Z axis position interactions
            currentRightDirection = transform.right;
            currentForwardDirection = transform.forward; // forward vector
            currentUpDirection = transform.up; // up vector

             currentPosition = transform.localPosition;
             // currentRotation = transform.localRotation;

              if (currentPosition != lastPosition)
              {
                 if (currentPosition.x != lastPosition.x)
                 {
                     updateVariables("XAxis");
                 }
                 if (currentPosition.y != lastPosition.y)
                 {
                     updateVariables("YAxis");
                 }
                 if (currentPosition.z != lastPosition.x)
                 {
                     updateVariables("ZAxis");
                 }
              }
              

             
            /* OLD VERSION
            if (currentRotation.y != lastPosition.y)
			{
                updateVariables("YRot");
			}
            */

            if (currentForwardDirection != lastForwardDirection && Mathf.Abs(currentPosition.x - lastPosition.x) < dirAdjust && Mathf.Abs(currentPosition.z - lastPosition.z) < dirAdjust)
			{
                // updateVariables("XAxisTEST");
                updateVariables("YRot");
			}

            if (currentUpDirection != lastUpDirection && Mathf.Abs(currentPosition.y - lastPosition.y) < dirAdjust && Mathf.Abs(currentPosition.x - lastPosition.x) < dirAdjust)
            {
                updateVariables("XRot");
			}
            

            lastPosition = currentPosition;
            // lastRotation = currentRotation;
            lastForwardDirection = currentForwardDirection;
            lastUpDirection = currentUpDirection;
            lastRightDirection = currentRightDirection;

            
		}
    }

    public void interactionActive(int interactionState)
    {
        switch (interactionState)
        {
            case 1:
                while (initialUpdate < 1)
                {
                    lastPosition = transform.localPosition;
                    // lastRotation = transform.localRotation;
                    lastForwardDirection = transform.forward;
                    lastUpDirection = transform.forward;
                    lastRightDirection = transform.forward;
                    initialUpdate++;
                }
                interactionOn = true;
                break;
            case 2:
                interactionOn = false;
                initialUpdate = 0;
                break;
            default:
                interactionOn = false;
                initialUpdate = 0;
                break;
        }
    }

    void updateVariables(string axisSelect)
	{
        switch (axisSelect)
		{
            /*
            case "XAxisTEST":
                float deltaX = Vector3.Distance(currentForwardDirection, lastForwardDirection) * axisScale;
                deltaXVal = Mathf.SmoothDamp(oldDeltaX, deltaX, ref xVelocity, 0.001f);
                oldDeltaX = deltaX;
                break;
            */
            case "XAxis":
                float deltaX = ((currentPosition.x - lastPosition.x) * axisScale);
                deltaXVal = Mathf.SmoothDamp(oldDeltaX, deltaX, ref xVelocity, 0.001f);
                oldDeltaX = deltaX;
                break;
            case "YAxis":
                float deltaY = ((currentPosition.y - lastPosition.y) * axisScale);
                deltaYVal = Mathf.SmoothDamp(oldDeltaY, deltaY, ref yVelocity, 0.001f);
                oldDeltaY = deltaY;
                break;
            case "ZAxis":
                float deltaZ = ((currentPosition.z - lastPosition.z) * axisScale);
                deltaZVal = Mathf.SmoothDamp(oldDeltaZ, deltaZ, ref zVelocity, 0.001f);
                oldDeltaZ = deltaZ;
                break;

            case "YRot":
                // SignedAngle between current and last forward vectors, around the up vector
                float deltaRotY = Vector3.SignedAngle(currentForwardDirection, lastForwardDirection, currentUpDirection) * rotScale;
                if (Mathf.Abs(deltaRotY) > rotAdjust)
				{
                    deltaRotYVal = Mathf.SmoothDamp(oldDeltaRotY, deltaRotY, ref rotYVelocity, 0.001f);
                }
				else
				{
                    deltaRotYVal = Mathf.SmoothDamp(oldDeltaRotY, 0, ref rotYVelocity, 0.001f);
				}
                oldDeltaRotY = deltaRotYVal;
                break;

            case "XRot":
                float deltaRotX = Vector3.SignedAngle(currentUpDirection, lastUpDirection, currentForwardDirection) * rotScale;
                if (Mathf.Abs(deltaRotX) > rotAdjust)
				{
                    deltaRotXVal = Mathf.SmoothDamp(oldDeltaRotX, deltaRotX, ref rotXVelocity, 0.001f);
				}
				else
				{
                    deltaRotXVal = Mathf.SmoothDamp(oldDeltaRotX, 0, ref rotXVelocity, 0.001f);
				}
                oldDeltaRotX = deltaRotXVal;
                break;

                /* OLD VERSION
                float deltaRotY = (((currentRotation.y - lastRotation.y) * rotScale) - 0.25f);
                deltaRotYVal = Mathf.SmoothDamp(oldDeltaRotY, deltaRotY, ref rotYVelocity, 0.001f);
                oldDeltaRotY = deltaRotY;
                break;
                */
		}
	}

    #endregion

}
