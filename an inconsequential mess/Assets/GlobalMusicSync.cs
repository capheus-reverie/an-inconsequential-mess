using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMusicSync : MonoBehaviour
{
    #region Variables

    [Header ("Initialisation")]
    public AK.Wwise.Event Clicktrack;

    [Header("Song Sections")]
    [SerializeField]
    private AK.Wwise.State Intro;
    [SerializeField]
    private AK.Wwise.State[] Verses;
    [SerializeField]
    private AK.Wwise.State[] Choruses;
    [SerializeField]
    private AK.Wwise.State[] Instrumentals;

    #endregion

    #region Unity Methods

    void Start()
    {

        Clicktrack.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncBeat, pushBeat);

    }

	private void Update()
	{
		/*
		 * This test code has the following music sectional changes at 10 seconds:
		 * Verse 1: <= 30% interaction ratio, with <= 5 interaction events
		 * Verse 2: <= 30% interaction ratio, with > 5 interaction events
		 * Chorus 1: more than 30% interaction ratio, with <= 10 interaction events
		 * Chorus 2: more than 30% interaction ratio, with > 10 interaction events
		 * */

        if(Time.fixedUnscaledTime > 10)
		{
            if (Manager.instance.interactionRatio <= 30 && Manager.instance.interactionEvents <= 5)
            {
                Verses[0].SetValue();
                Debug.Log("Verse 1");
            }
            else if (Manager.instance.interactionRatio <= 30 && Manager.instance.interactionEvents > 5)
            {
                Verses[1].SetValue();
                Debug.Log("Verse 2");
            }
            else if (Manager.instance.interactionRatio > 30 && Manager.instance.interactionEvents <= 10)
            {
                Choruses[0].SetValue();
                Debug.Log("Chorus 1");
            }
            else if (Manager.instance.interactionRatio > 30 && Manager.instance.interactionEvents > 10)
            {
                Choruses[1].SetValue();
                Debug.Log("Chorus 1");
            }
            else { }
        }

	}

	void pushBeat(object in_cookie, AkCallbackType in_type, object in_info)
    {
        for(int i = 0; i < 1; i++)
		{
            Manager.instance.beatStart();
            Debug.Log("Beat Pushed");
        }
    }

    #endregion

}
