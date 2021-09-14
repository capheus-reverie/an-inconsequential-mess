using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class triggerParticle : MonoBehaviour
{
    #region Variables

    [SerializeField] ParticleSystem particle = null;

    #endregion

    #region Unity Methods

    void Update()
    {
        
    }

    public void triggerBurst()
	{
        particle.Play();
	}

    #endregion

}
