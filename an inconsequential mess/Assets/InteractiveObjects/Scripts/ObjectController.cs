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

    [SerializeField] private AK.Wwise.RTPC leftXAxisRTPC;
    [SerializeField] private AK.Wwise.RTPC leftYAxisRTPC;
    [SerializeField] private AK.Wwise.RTPC leftZAxisRTPC;
    [SerializeField] private AK.Wwise.RTPC leftYAxisRotationRTPC;

    [Header("Right Hand Manipulation")]

    [Range(0f, 100f)]
    public float rightXAxis = 50f;
    [Range(0f, 100f)]
    public float rightYAxis = 50f;
    [Range(0f, 100f)]
    public float rightZAxis = 50f;
    [Range(0f, 100f)]
    public float rightYAxisRotation = 50f;

    [SerializeField] private AK.Wwise.RTPC rightXAxisRTPC;
    [SerializeField] private AK.Wwise.RTPC rightYAxisRTPC;
    [SerializeField] private AK.Wwise.RTPC rightZAxisRTPC;
    [SerializeField] private AK.Wwise.RTPC rightYAxisRotationRTPC;

    [Header("")]
    [Header("Object Setup")]

    // setup the list of available instruments
    [SerializeField] private AK.Wwise.Switch[] instrumentSwitch;
    private int instrument = 0;

    [SerializeField] private AK.Wwise.Event solo;
    [SerializeField] private AK.Wwise.Event unsolo;
    [SerializeField] private AK.Wwise.Event play;
    [SerializeField] private AK.Wwise.Event stop;

    private bool objectPlaying = false;
    private bool coroutineStopped = true;
    private int nextBeat;

    // Initialize particle component variable
    private ParticleSystem particle = null;


    #endregion

    #region Unity Methods

    void Start()
    {
        particle = gameObject.GetComponentInChildren<ParticleSystem>();
        instrumentSwitch[0].SetValue(this.gameObject);
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

        if (objectPlaying == true && coroutineStopped == false)
		{
            StopCoroutine(playSync(0));
            coroutineStopped = true;
		}

    }

    public void objectAdmin(string state)
	{
        switch (state)
		{
            case "play":
				if (!objectPlaying)
				{
                    nextBeat = Manager.instance.beat + 1;
                    StartCoroutine(playSync(nextBeat));
                    coroutineStopped = false;
                }
				else
				{
                    stop.Post(this.gameObject);
                    nextBeat = Manager.instance.beat + 1;
                    objectPlaying = false;
				}
                break;
            case "addInst":
                if(instrument < instrumentSwitch.Length)
				{
                    instrument++;
                    instrumentSwitch[instrument].SetValue(this.gameObject);
                }
				else
				{
                    instrument = 0;
				}
                break;
            case "subInst":
                if(instrument != 0)
				{
                    instrument--;
                    instrumentSwitch[instrument].SetValue(this.gameObject);
                }
				else
				{
                    instrument = instrumentSwitch.Length;
				}
                break;
		}
	}

    void triggerParticle(object in_cookie, AkCallbackType in_type, object in_object)
	{
        particle.Play();
	}

    IEnumerator playSync(int i)
	{
        Debug.Log("Coroutine started");

        if(Manager.instance.beat == i)
		{
            play.Post(this.gameObject, (uint)AkCallbackType.AK_MIDIEvent, triggerParticle);
            objectPlaying = true;
            Debug.Log("Object Playing");
            yield return null;
		}
		else if(Manager.instance.beat == i - 1)
		{
            do
            {
                yield return null;
            } while (Manager.instance.beat < i);


            play.Post(gameObject, (uint)AkCallbackType.AK_MIDIEvent, triggerParticle);
            objectPlaying = true;
            Debug.Log("Object Playing");

            yield return null;
        }
        else
		{
            nextBeat = Manager.instance.beat + 1;

            yield return null;
        }

	}

    #endregion

}
