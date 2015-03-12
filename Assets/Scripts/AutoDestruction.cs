using UnityEngine;
using System.Collections;

public class AutoDestruction : MonoBehaviour
{
	private enum DestructionMethode
	{
		None,
		ParticleSystem,
		Animator
	}

	[SerializeField]
	private DestructionMethode destructionBy = DestructionMethode.None;

	void Start()
	{
		if (destructionBy == DestructionMethode.ParticleSystem && GetComponent<ParticleSystem>())
		{
			Destroy(this.gameObject, this.GetComponent<ParticleSystem>().duration + this.GetComponent<ParticleSystem>().startLifetime);
		}
		else if (destructionBy == DestructionMethode.Animator && GetComponent<Animator>())
		{
			StartCoroutine("WaitForAnimationStart");
		}
	}

	IEnumerator WaitForAnimationStart()
	{
		while(GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length < 1)
		{
			yield return new WaitForEndOfFrame();
		}

		Destroy(this.gameObject, GetComponent<Animator>().GetCurrentAnimatorClipInfo(0)[0].clip.length);
	}
}
