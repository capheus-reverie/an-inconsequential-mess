using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMusicSync : MonoBehaviour
{
    #region Variables

    // Add state changes for song sections (used to change the global form of the piece
    [Header("Song Sections")]
    public AK.Wwise.State StartMenu;
    public AK.Wwise.State[] Verses;
    public AK.Wwise.State[] Choruses;
    public AK.Wwise.State[] Instrumentals;

    [Tooltip("Make this value the top value of the initial time signature as set in Wwise")]
    public int beatsPerBar = 9; // Initial Time Signature known in Wwise.
    [Tooltip("Make this value the tempo originally used in Wwise")]
    public float tempo = 60; // Initial tempo known in Wwise.
    [HideInInspector] public uint currentSection = 0;

    #endregion

    #region Unity Methods

    void Start()
    {
        Debug.Log(StartMenu.Id);
    }

	private void Update()
	{
        checkSectionState();

		/*
		 * This test code has the following music sectional changes at 10 seconds:
		 * Verse 1: <= 30% interaction ratio, with <= 5 interaction events
		 * Verse 2: <= 30% interaction ratio, with > 5 interaction events
		 * Chorus 1: more than 30% interaction ratio, with <= 10 interaction events
		 * Chorus 2: more than 30% interaction ratio, with > 10 interaction events
		 * */
        /*
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
        */

	}

	void globalSyncCallback(object in_cookie, AkCallbackType in_type, object in_info)
    {
        // Update bar and beat duration information
        AkMusicSyncCallbackInfo info = (AkMusicSyncCallbackInfo)in_info;
        tempo = 60/info.segmentInfo_fBeatDuration;
        beatsPerBar = (int)(info.segmentInfo_fBarDuration / info.segmentInfo_fBeatDuration);

        for(int i=0; i<1; i++)
		{
            if (in_type == AkCallbackType.AK_MusicSyncBeat)
            {
                Manager.instance.beatAdd();
                Debug.Log("Beat Pushed");
            }
            if(in_type == AkCallbackType.AK_MusicSyncBar)
			{
                Manager.instance.barAdd();
                Debug.Log("Bar Pushed");
			}

        }
        
    }

    void checkSectionState()
	{
        AkSoundEngine.GetState(StartMenu.GroupId, out currentSection);
        Debug.Log("Current section is " + currentSection);
	}

    #endregion

}
