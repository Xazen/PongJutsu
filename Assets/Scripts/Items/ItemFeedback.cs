using UnityEngine;
using System.Collections;

public class ItemFeedback : MonoBehaviour 
{
	private GameObject objectReference;

	public void Setup(GameObject reference)
	{
		objectReference = reference;

		Destroy(this.gameObject, getParticleDuration());
	}

	public void Setup(GameObject reference, float duration)
	{
		objectReference = reference;

		if (duration > 0f)
			Destroy(this.gameObject, duration);
	}

	private float getParticleDuration()
	{
		ParticleSystem[] particleSystems = this.GetComponentsInChildren<ParticleSystem>();

		float maxParticleDuration = 0f;

		foreach (ParticleSystem ps in particleSystems)
		{
			float realParticleDuration = ps.duration + ps.startLifetime;

			if (realParticleDuration > maxParticleDuration)
				maxParticleDuration = realParticleDuration;
		}

		return maxParticleDuration;
	}

	void LateUpdate () 
	{
		this.transform.position = objectReference.transform.position;
	}
}
