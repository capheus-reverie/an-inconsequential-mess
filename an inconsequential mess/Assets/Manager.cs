using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    #region Variables

    public static Manager instance = null;
    public int beatsPerBar = 9;
    [HideInInspector] public int beat = 0;

    #endregion

    #region Unity Methods

    private void Awake()
	{
		if (instance == null)
		{
            instance = this;
		}
        else if (instance != null)
		{
            Destroy(gameObject);
		}
	}

	void Start()
    {
        
    }

    public void beatStart()
    {
        if (beat < beatsPerBar)
        {
            beat++;
        }
        else
        {
            beat = 1;
        }

    }

    void Update()
    {

    }

    #endregion

}
