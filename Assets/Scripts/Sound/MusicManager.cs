using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class MusicManager : MonoBehaviour
	{
		[SerializeField] private SoundState defaultState = SoundState.MainMenu;
		[SerializeField] private StateClip[] clips;

		public static MusicManager current;

		[System.Serializable]
		private class StateClip
		{
			public AudioClip clip = null;
			public SoundState state = 0;
		}


		void Awake()
		{
			current = this;
			PlayState(defaultState);
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

		public void PlayState(SoundState state)
		{
			PlayClip(getRandomClip(state));
		}

		public void FadeOut(float duration)
		{
			StartCoroutine(IFadeout(duration));
		}

		public void FadeToState(float duration, SoundState state)
		{
			StartCoroutine(IFadeToState(duration, state));
		}

		AudioClip getRandomClip(SoundState state)
		{
			List<StateClip> clipsList = new List<StateClip>(clips);
			StateClip[] stateClips = clipsList.FindAll(x => x.state == state).ToArray();

			return stateClips[Random.Range(0, stateClips.Length - 1)].clip;
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

		IEnumerator IFadeToState(float duration, SoundState state)
		{
			AudioSource audio = this.GetComponent<AudioSource>();

			while (audio.volume > 0f)
			{
				float volume = audio.volume - (1f / duration) * Time.deltaTime;
				audio.volume = Mathf.Clamp(volume, 0f, 1f);
				yield return new WaitForEndOfFrame();
			}

			PlayState(state);
		}
	}
}
public enum SoundState
{
	MainMenu,
	InGame	
}
