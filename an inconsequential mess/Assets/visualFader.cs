using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class visualFader : MonoBehaviour
{
    #region Variables
    public CanvasGroup canvasToFade;
    private float initialAlpha = 1f;
    #endregion

    #region Unity Methods

    public void startFade()
	{
        if (canvasToFade.alpha != 1f)
		{
            initialAlpha = canvasToFade.alpha;
		}
		else
		{
            initialAlpha = 1f;
		}

        StartCoroutine(FadeCoroutine(initialAlpha));
	}

    IEnumerator FadeCoroutine(float start)
	{
        do
        {
            canvasToFade.alpha = canvasToFade.alpha-0.0001f;
            yield return null;
        } while (canvasToFade.alpha > 0);

        Debug.Log("Canvas Faded");
        Manager.instance.deactivateLoadingScreen();
	}

    #endregion

}
