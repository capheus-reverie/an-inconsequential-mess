using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dotVectorDisplay : MonoBehaviour
{
    #region Variables

    public float xPos = 0f;
    public float yPos = 0f;
    public float zPos = 0f;
    public float adjust = 0.5f;

    private Vector3 dotPosition;
    private Vector3 dotAdjust;

    #endregion

    #region Unity Methods

    void Start()
    {
        dotAdjust = new Vector3(adjust, adjust, adjust);
    }

    void Update()
    {
        dotPosition = new Vector3(xPos, yPos, zPos) - dotAdjust;
        transform.localPosition = dotPosition;
    }

    #endregion

}
