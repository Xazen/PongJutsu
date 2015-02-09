using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class AudioVolumeControl : MonoBehaviour
	{
		private float sourceVolume;

		void Start()
		{
			sourceVolume = this.GetComponent<AudioSource>().volume;
		}

		void Update() 
		{
			this.GetComponent<AudioSource>().volume = sourceVolume * MusicManager.current.masterVolume;
		}
	}
}
