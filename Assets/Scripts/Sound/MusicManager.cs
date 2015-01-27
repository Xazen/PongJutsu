using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class MusicManager : MonoBehaviour
	{
		[SerializeField] private float masterVolume = 1f;
		[SerializeField] private Layer[] layers;
		[SerializeField] private Layer peakLayer;

		public static MusicManager current;

		[System.Serializable]
		public class Layer
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
			checkTrigger();
		}

		void checkTrigger()
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

			if (Input.GetKeyDown("1"))
				PlayPeakLayer();
			else if (Input.GetKeyDown("2"))
				StopPeakLayer();

			if (Input.GetKeyDown("3"))
				Play(layers[1], layers[1].clips[0]);
			else if (Input.GetKeyDown("4"))
				MuteLayer(layers[1]);

			if (Input.GetKeyDown("5"))
				NextClip(layers[0]);
		}

		public void StartMusic()
		{
			PlaySolo(layers[0], layers[0].clips[0]);
		}

		public void EndMusic()
		{
			foreach (AudioSource audio in this.GetComponents<AudioSource>())
			{
				audio.Stop();
				audio.clip = null;
				audio.mute = false;
			}
		}

		public void PauseMusic()
		{
			foreach (AudioSource source in this.GetComponents<AudioSource>())
			{
				if (layers[0].source != source)
					StartCoroutine(IFadeout(source, 1f));
			}
		}

		public void ResumeMusic()
		{
			foreach (AudioSource source in this.GetComponents<AudioSource>())
			{
				if (layers[0].source != source)
					StartCoroutine(IFadein(source, 1f));
			}
		}

		public void PlayPeakLayer()
		{
			StartCoroutine(IFadeout(layers[0].source, 0.5f));
			StartCoroutine(IFadeout(layers[1].source, 0.5f));
			Play(peakLayer, peakLayer.clips[0]);
		}

		public void StopPeakLayer()
		{
			StartCoroutine(IFadein(layers[0].source, 1f));
			StartCoroutine(IFadein(layers[1].source, 1f));
			StartCoroutine(IFadeout(peakLayer.source, 1f));
		}

		private void Play(Layer layer, AudioClip clip)
		{
			layer.source.clip = clip;

			layer.source.mute = false;

			if (!layer.source.isPlaying)
				layer.source.Play();
		}

		private void PlaySolo(Layer layer, AudioClip clip)
		{
			foreach (Layer l in layers)
			{
				if (l != layer)
					StopLayer(l);
			}

			layer.source.clip = clip;

			if (!layer.source.isPlaying)
				layer.source.Play();
		}

		public void NextClip(Layer layer)
		{
			StartCoroutine(WaitForNextClip(layer));
		}

		IEnumerator WaitForNextClip(Layer layer)
		{
			yield return new WaitForSeconds(layer.source.clip.length - layer.source.time);
			Play(layer, layer.clips[1]);
		}

		private void StopLayer(Layer layer)
		{
			layer.source.Stop();
		}

		private void MuteLayer(Layer layer)
		{
			layer.source.mute = true;
		}

		IEnumerator IFadein(AudioSource source, float duration)
		{
			source.Play();
			while (source.volume < 1f)
			{
				float volume = source.volume + (1f / duration) * Time.unscaledDeltaTime;
				source.volume = Mathf.Clamp(volume, 0f, 1f);
				yield return new WaitForEndOfFrame();
			}
		}

		IEnumerator IFadeout(AudioSource source, float duration)
		{
			while (source.volume > 0f)
			{
				float volume = source.volume - (1f / duration) * Time.unscaledDeltaTime;
				source.volume = Mathf.Clamp(volume, 0f, 1f);
				yield return new WaitForEndOfFrame();
			}
			source.Pause();
		}
	}
}
