using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    #region Variables

    public static Manager instance = null;
    public int quaver = 1;

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
        if (quaver < 9)
        {
            quaver++;
        }
        else
        {
            quaver = 1;
        }

    }

    void Update()
    {
        Debug.Log(quaver);
    }

    #endregion

}
