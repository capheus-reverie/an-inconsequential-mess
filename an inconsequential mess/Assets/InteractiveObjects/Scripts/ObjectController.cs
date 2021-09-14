using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectController : MonoBehaviour
{
    #region Variables

    // Setup Inspector Menus and Wwise RTPC components

    [Header("Left Hand Manipulation")]

    [Range(0f, 100f)]
    public float leftXAxis = 90f;
    [Range(0f, 100f)]
    public float leftYAxis = 40f;
    [Range(0f, 100f)]
    public float leftZAxis = 100f;
    [Range(0f, 100f)]
    public float leftYAxisRotation = 40f;

    public AK.Wwise.RTPC leftXAxisRTPC;
    public AK.Wwise.RTPC leftYAxisRTPC;
    public AK.Wwise.RTPC leftZAxisRTPC;
    public AK.Wwise.RTPC leftYAxisRotationRTPC;

    [Header("Right Hand Manipulation")]

    [Range(0f, 100f)]
    public float rightXAxis = 50f;
    [Range(0f, 100f)]
    public float rightYAxis = 50f;
    [Range(0f, 100f)]
    public float rightZAxis = 50f;
    [Range(0f, 100f)]
    public float rightYAxisRotation = 50f;

    public AK.Wwise.RTPC rightXAxisRTPC;
    public AK.Wwise.RTPC rightYAxisRTPC;
    public AK.Wwise.RTPC rightZAxisRTPC;
    public AK.Wwise.RTPC rightYAxisRotationRTPC;

    [Header("")]
    [Header("Event Setup")]

    public AK.Wwise.Event mute;
    public AK.Wwise.Event unmute;
    public AK.Wwise.Event play;
    public AK.Wwise.Event stop;

    // Initialize particle component variable
    private ParticleSystem particle = null;


    #endregion

    #region Unity Methods

    void Start()
    {
        particle = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        leftXAxisRTPC.SetValue(this.gameObject, leftXAxis);
        leftYAxisRTPC.SetValue(this.gameObject, leftYAxis);
        leftZAxisRTPC.SetValue(this.gameObject, leftZAxis);
        leftYAxisRotationRTPC.SetValue(this.gameObject, leftYAxisRotation);

        rightXAxisRTPC.SetValue(this.gameObject, rightXAxis);
        rightYAxisRTPC.SetValue(this.gameObject, rightYAxis);
        rightZAxisRTPC.SetValue(this.gameObject, rightZAxis);
        rightYAxisRotationRTPC.SetValue(this.gameObject, rightYAxisRotation);
    }

    public void objectAdmin(string state)
	{
        switch (state)
		{
            case "mute":
                mute.Post(gameObject);
                break;
            case "unmute":
                unmute.Post(gameObject);
                break;
            case "play":
                play.Post(gameObject, (uint)AkCallbackType.AK_MIDIEvent, triggerParticle);
                break;
            case "stop":
                stop.Post(gameObject);
                break;
		}
	}

    void triggerParticle(object in_cookie, AkCallbackType in_type, object in_object)
	{
        particle.Play();
	}

    #endregion

}
