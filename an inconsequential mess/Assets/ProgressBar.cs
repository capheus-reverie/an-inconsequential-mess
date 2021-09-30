using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]

public class ProgressBar : MonoBehaviour
{
    #region Variables

    public int maximum;
    [Range(0.0f, 100.0f)]
    public int current;
    public Image mask;

    #endregion

    #region Unity Methods

    void Start()
    {
        
    }

    void Update()
    {
        GetCurrentFill();
    }

    void GetCurrentFill()
	{
        float fillAmount = (float)current / (float)maximum;
        mask.fillAmount = fillAmount;
	}

    #endregion

}
