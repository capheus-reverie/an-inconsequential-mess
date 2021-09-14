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
    private Quaternion lastRotation;
    private Quaternion currentRotation;

    // Assign interaction bool and int
    bool interactionOn = false;
    int initialUpdate = 0;

    // Assign scale variables to convert movement to appropriate changes in values
    public int axisScale = 200;
    public int rotScale = 100;

    // Assign variables for delta
    public float deltaXVal;
    public float deltaYVal;
    public float deltaZVal;
    // public float deltaRotXVal;
    public float deltaRotYVal;
    // public float deltaRotZVal;

    private float xVelocity = 0.0f;
    private float yVelocity = 0.0f;
    private float zVelocity = 0.0f;
    private float rotYVelocity = 0.0f;

    float oldDeltaX = 0.0f;
    float oldDeltaY = 0.0f;
    float oldDeltaZ = 0.0f;
    // float oldDeltaRotX = 0.0f;
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
            currentPosition = transform.localPosition;
            currentRotation = transform.localRotation;

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

            if (currentRotation.y != lastPosition.y)
			{
                updateVariables("YRot");
			}

            lastPosition = currentPosition;
            lastRotation = currentRotation;

            
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
                    lastRotation = transform.localRotation;
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
                float deltaRotY = ((currentRotation.y - lastRotation.y) * rotScale);
                deltaRotYVal = Mathf.SmoothDamp(oldDeltaRotY, deltaRotY, ref rotYVelocity, 0.001f);
                oldDeltaRotY = deltaRotY;
                break;
		}
	}

    #endregion

}
