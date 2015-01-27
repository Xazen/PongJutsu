using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class MusicManager : MonoBehaviour
	{
		[SerializeField] private float masterVolume = 1f;
		[SerializeField] private Layer[] layers;

		public static MusicManager current;

		[System.Serializable]
		private class Layer
		{
			public AudioSource source;
			public AudioClip[] clips;
		}

		void Awake()
		{
			if (current == null)
				current = this;
		}

		void Update()
		{
			checkMute();
		}

		void checkMute()
		{
			if (Input.GetKeyDown("f1"))
			{
				if (this.GetComponent<AudioSource>().mute)
					this.GetComponent<AudioSource>().mute = false;
				else
					this.GetComponent<AudioSource>().mute = true;
			}

			if (Input.GetKeyDown("f2"))
			{
				if (AudioListener.pause)
					AudioListener.pause = false;
				else
					AudioListener.pause = true;
			}
		}

		public void FadeOut(float duration)
		{
			StartCoroutine(IFadeout(duration));
		}

		void PlayClip(AudioClip clip)
		{
			this.GetComponent<AudioSource>().clip = clip;
			this.GetComponent<AudioSource>().volume = 1f;
			audio.time = 0f;

			if (!audio.isPlaying)
				audio.Play();
		}

		IEnumerator IFadeout(float duration)
		{
			AudioSource audio = this.GetComponent<AudioSource>();

			while (audio.volume > 0f)
			{
				float volume = audio.volume - (1f / duration) * Time.deltaTime;
				audio.volume = Mathf.Clamp(volume, 0f, 1f);
				yield return new WaitForEndOfFrame();
			}
		}
	}
}
