using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalMusicSync : MonoBehaviour
{
    #region Variables

    public AK.Wwise.Event Clicktrack;

    #endregion

    #region Unity Methods

    void Start()
    {

        Clicktrack.Post(gameObject, (uint)AkCallbackType.AK_MusicSyncBeat, pushBeat);

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
