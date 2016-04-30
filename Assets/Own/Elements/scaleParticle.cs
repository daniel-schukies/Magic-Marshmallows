using System;
using UnityEngine;


public class scaleParticle : MonoBehaviour
{
	// a simple script to scale the size, speed and lifetime of a particle system
	
	public float multiplier = 1;
	public float lastMultiplier = 1;

	void Start(){
		multiplier = transform.localScale.x;
	}


	public void Update()
	{
		multiplier = transform.localScale.x;
		if (multiplier != lastMultiplier) {
			scale();
		}
	}


	private void scale()
	{
		var systems = GetComponentsInChildren<ParticleSystem>();

		//Debug.Log ("Test§§)");
		// reset scale as on start
		foreach (ParticleSystem system in systems)
		{
			system.startSize /= lastMultiplier;
			system.startSpeed /= lastMultiplier;
			system.startLifetime /= Mathf.Lerp(lastMultiplier, 1, 0.5f);
		}

		// set new scale

		foreach (ParticleSystem system in systems)
		{
			system.startSize *= multiplier;
			system.startSpeed *= multiplier;
			system.startLifetime *= Mathf.Lerp(multiplier, 1, 0.5f);
			//system.Clear();
			//system.Play();
		}

		lastMultiplier = multiplier;
	}
}

