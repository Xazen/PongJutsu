﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class MusicManager : MonoBehaviour
	{
		private float _masterVolume = 1f;
		public float masterVolume
		{
			get 
			{
				return _masterVolume;
			}
			set
			{
				_masterVolume = value;

				foreach (Layer layer in layers)
				{
					if (layer.source.volume > 0f)
						layer.source.volume = _masterVolume;
				}
			}
		}

		[SerializeField] private int startLayerElement = 0;
		[SerializeField] private int pauseLayerElement = 0;
		[SerializeField] private int peakLayerElement = 0;

		[SerializeField] private Layer[] layers;

		public static MusicManager current;

		[System.Serializable]
		public class Layer
		{
			public AudioSource source;
			public AudioClip[] clips;
			[HideInInspector] public int currentClipIndex = 0;
			[HideInInspector] public int nextClipIndex = 0;
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
				NextClipInLayer(layers[0]);
		}

		public void StartMusic()
		{
			StartLayer(layers[startLayerElement]);
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
				if (layers[pauseLayerElement].source != source)
					StartCoroutine(IFadeout(source, 0.7f));
			}
		}

		public void ResumeMusic()
		{
			foreach (AudioSource source in this.GetComponents<AudioSource>())
			{
				if (layers[pauseLayerElement].source != source)
					StartCoroutine(IFadein(source, 1f));
			}
		}

		public void PlayPeakLayer()
		{
			StartCoroutine(IFadeout(layers[0].source, 0.5f));
			StartCoroutine(IFadeout(layers[1].source, 0.5f));
			Play(layers[peakLayerElement], layers[peakLayerElement].clips[0]);
		}

		public void StopPeakLayer()
		{
			StartCoroutine(IFadein(layers[0].source, 1f));
			StartCoroutine(IFadein(layers[1].source, 1f));
			StartCoroutine(IFadeout(layers[peakLayerElement].source, 1f));
		}

		private void Play(Layer layer, AudioClip clip)
		{
			layer.source.clip = clip;

			layer.source.mute = false;
			layer.source.volume = masterVolume;

			if (!layer.source.isPlaying)
				layer.source.Play();
		}

		private void StartLayer(Layer layer)
		{
			Play(layer, layer.clips[0]);
		}

		public void NextClipInLayer(Layer layer)
		{
			if (layer.currentClipIndex != layer.nextClipIndex)
			{
				layer.nextClipIndex = Mathf.Clamp(layer.currentClipIndex + 1, 0, layer.clips.Length - 1);
			}
			else
			{
				layer.nextClipIndex = Mathf.Clamp(layer.currentClipIndex + 1, 0, layer.clips.Length - 1);
				StartCoroutine(WaitForNextClip(layer));
			}
		}

		IEnumerator WaitForNextClip(Layer layer)
		{
			yield return new WaitForSeconds(layer.source.clip.length - layer.source.time);
			Play(layer, layer.clips[layer.nextClipIndex]);
		}

		IEnumerator IFadein(AudioSource source, float duration)
		{
			source.Play();
			while (source.volume < masterVolume)
			{
				float volume = source.volume + (masterVolume / duration) * Time.unscaledDeltaTime;
				source.volume = Mathf.Clamp(volume, 0f, masterVolume);
				yield return new WaitForEndOfFrame();
			}
		}

		IEnumerator IFadeout(AudioSource source, float duration)
		{
			while (source.volume > 0f)
			{
				float volume = source.volume - (masterVolume / duration) * Time.unscaledDeltaTime;
				source.volume = Mathf.Clamp(volume, 0f, masterVolume);
				yield return new WaitForEndOfFrame();
			}
			source.Pause();
		}
	}
}