using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Destructor : MonoBehaviour
	{
		[SerializeField]
		private bool destructByParticlesystem = false;

		void Awake()
		{
			if (destructByParticlesystem && this.GetComponent<ParticleSystem>() != null)
				Destroy(this.gameObject, this.GetComponent<ParticleSystem>().duration + this.GetComponent<ParticleSystem>().startLifetime);
		}

		void ae_Remove()
		{
			Destroy(this.gameObject);
		}
	}
}
