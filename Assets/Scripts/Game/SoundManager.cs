using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class SoundManager : MonoBehaviour
	{
		[SerializeField] private SoundState defaultState = SoundState.MainMenu;
		[SerializeField] private StateClip[] clips;

		[System.Serializable]
		public class StateClip
		{
			public AudioClip clip;
			public SoundState state;
		}


		void Awake()
		{
			SetState(defaultState, 0f);
		}

		void Update()
		{
			checkMute();

			if (Input.GetKeyDown("f3"))
			{
				SetState(SoundState.InGame, 2f);
			}
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

		public void SetState(SoundState state, float fadeoutDuration)
		{
			AudioSource audio = this.GetComponent<AudioSource>();
			List<StateClip> clipsList = new List<StateClip>(clips);

			if (audio.clip == null)
			{
				Play(getRandomClip(state));
			}
			else
			{
				StartCoroutine(FadeToClip(fadeoutDuration, getRandomClip(state)));
			}
		}

		AudioClip getRandomClip(SoundState state)
		{
			List<StateClip> clipsList = new List<StateClip>(clips);
			StateClip[] stateClips = clipsList.FindAll(x => x.state == state).ToArray();

			return stateClips[Random.Range(0, stateClips.Length - 1)].clip;
		}

		void Play(AudioClip clip)
		{
			this.GetComponent<AudioSource>().clip = clip;
			this.GetComponent<AudioSource>().volume = 1f;
			audio.time = 0f;

			if (!audio.isPlaying)
				audio.Play();
		}

		IEnumerator FadeToClip(float duration, AudioClip clip)
		{
			AudioSource audio = this.GetComponent<AudioSource>();

			while (audio.volume > 0f)
			{
				float volume = audio.volume - (1f / duration) * Time.deltaTime;
				audio.volume = Mathf.Clamp(volume, 0f, 1f);
				yield return new WaitForEndOfFrame();
			}

			Play(clip);
		}
	}
}
public enum SoundState
{
	MainMenu,
	InGame	
}
