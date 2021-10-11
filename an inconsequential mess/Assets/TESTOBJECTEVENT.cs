using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TESTOBJECTEVENT : MonoBehaviour
{
    #region Variables

    [SerializeField]
    private AK.Wwise.Event TEST = null;
    [SerializeField]
    private AK.Wwise.State TESTSTATE = null;

    #endregion

    #region Unity Methods

    void Start()
    {
        AkSoundEngine.RegisterGameObj(gameObject, gameObject.name);
        StartCoroutine(playEvent());
    }

    private IEnumerator playEvent()
	{
        yield return new WaitForSeconds(15);
        Debug.Log("Event Coroutine Finished");
        TESTSTATE.SetValue();
        TEST.Post(gameObject);
	}

    void Update()
    {
        
    }

    #endregion

}
