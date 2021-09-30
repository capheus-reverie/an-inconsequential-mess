using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneIndexes
{
    _MANAGER = 0,
    _TRAINING = 1,
    _EXPERIENCE = 2
}

public class Manager : MonoBehaviour
{
    #region Variables

    [Header("Scene Setup")]
    public GameObject loadingScreen;
    public GlobalMusicSync WwiseGlobal;
    public ProgressBar progressBar;

    public static Manager instance = null;
    public int beatsPerBar = 9;
    [HideInInspector] public int beat = 0;
    public int interactionEvents = 0;
    private float interactionDuration = 0;
    public float interactionRatio = 0;
    private float interactionLeftStart = 0;
    private float interactionRightStart = 0;
    private float interactionLeftStop = 0;
    private float interactionRightStop = 0;



    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    #endregion

    #region Unity Methods

    private void Awake()
	{
		if (instance == null)
		{
            instance = this;
            StartCoroutine(updateTime(1.0f));
        }
        else if (instance != null)
		{
            Destroy(gameObject);
		}

	}

	void Start()
    {
        loadingScreen.gameObject.SetActive(true);

        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes._TRAINING, LoadSceneMode.Additive));
        scenesLoading.Add(SceneManager.LoadSceneAsync((int)SceneIndexes._EXPERIENCE, LoadSceneMode.Additive));

        StartCoroutine(GetSceneLoadProgress());
    }

    float totalSceneProgress;
    public IEnumerator GetSceneLoadProgress()
	{
        for(int i=0; i<scenesLoading.Count; i++)
		{
            while (!scenesLoading[i].isDone)
			{
                totalSceneProgress = 0;

                foreach(AsyncOperation operation in scenesLoading)
				{
                    totalSceneProgress += operation.progress;
				}

                totalSceneProgress = (totalSceneProgress / scenesLoading.Count) * 100f;

                progressBar.current = Mathf.RoundToInt(totalSceneProgress);

                yield return null;
			}

            loadingScreen.gameObject.SetActive(false);
		}
	}

    public void interactionAdd(string type)
	{
        switch (type)
		{
            case "leftStart":
                interactionLeftStart = Time.fixedUnscaledTime;
                interactionEvents++;
                break;
            case "rightStart":
                interactionRightStart = Time.fixedUnscaledTime;
                interactionEvents++;
                break;
            case "leftStop":
                interactionLeftStop = Time.fixedUnscaledTime;
                interactionDuration += (interactionLeftStop - interactionLeftStart);
                break;
            case "rightStop":
                interactionRightStop = Time.fixedUnscaledTime;
                interactionDuration += (interactionRightStop - interactionRightStart);
                break;
		}
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

    IEnumerator updateTime(float waitTime)
	{
		while (true)
		{
            // Only update once per second
            yield return new WaitForSecondsRealtime(waitTime);

            // Find ratio of interacting time in % (hence the *100)
            interactionRatio = (interactionDuration / Time.fixedUnscaledTime)*100;
            Debug.Log(string.Format("Total duration is {0: #.00}s with {1: #.00}s of interactions making a total interaction ratio of {2: #.}%", Time.fixedUnscaledTime, interactionDuration, interactionRatio));
        }
        
    }

    void Update()
    {

    }

    #endregion

}
