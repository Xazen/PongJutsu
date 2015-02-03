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
		[SerializeField] private int bridgeLayerElement = 0;

		[System.Serializable]
		public class Layer
		{
			public AudioSource source;
			public AudioClip[] clips;
			[HideInInspector]
			public int currentClipIndex = 0;
			[HideInInspector]
			public int nextClipIndex = 0;
		}

		[SerializeField] private Layer[] layers;

		public static MusicManager current;

		void Awake()
		{
			if (current == null)
				current = this;
		}

		void Update()
		{
			if (Input.GetKeyDown("1"))
				NextClipInLayer(layers[0], true);
			if (Input.GetKeyDown("2"))
				NextClipInLayer(layers[1], true);
			if (Input.GetKeyDown("3"))
				NextClipInLayer(layers[0], false);
			if (Input.GetKeyDown("4"))
				NextClipInLayer(layers[1], false);	
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
			foreach (Layer layer in layers)
			{
				if (layers[pauseLayerElement].source != layer.source)
					StartCoroutine(IFadeout(layer, 0.7f));
			}
		}

		public void ResumeMusic()
		{
			foreach (Layer layer in layers)
			{
				if (layers[pauseLayerElement].source != layer.source)
					StartCoroutine(IFadein(layer, 1f));
			}
		}

		private void Play(Layer layer, int clipIndex)
		{
			layer.source.clip = layer.clips[clipIndex];

			layer.source.mute = false;
			layer.source.volume = masterVolume;

			if (!layer.source.isPlaying)
				layer.source.Play();
		}

		private void PlayOnce(Layer layer, int clipIndex)
		{
			layer.source.mute = false;
			layer.source.volume = masterVolume;

			layer.source.PlayOneShot(layer.clips[clipIndex]);
		}

		private void StartLayer(Layer layer)
		{
			Play(layer, 0);
		}

		public void NextClipInLayer(Layer layer, bool wait)
		{
			layer.nextClipIndex = Mathf.Clamp(layer.currentClipIndex + 1, 0, layer.clips.Length - 1);

			if (wait)
			{
				StartCoroutine(IWaitForNextClip(layer));
			}
			else
			{
				StartCoroutine(IFadeout(layer, 1f));
				StartCoroutine(IPeakTransition(layer));
			}
		}

		IEnumerator IWaitForNextClip(Layer layer)
		{
			yield return new WaitForSeconds(layer.source.clip.length - layer.source.time);
			Play(layer, layer.nextClipIndex);
		}

		IEnumerator IPeakTransition(Layer layer)
		{
			PlayOnce(layers[bridgeLayerElement], 0);
			yield return new WaitForSeconds(layers[bridgeLayerElement].clips[0].length);
			Play(layer, layer.nextClipIndex);
		}

		IEnumerator IFadein(Layer layer, float duration)
		{
			AudioSource source = layer.source;

			source.Play();
			while (source.volume < masterVolume)
			{
				float volume = source.volume + (masterVolume / duration) * Time.unscaledDeltaTime;
				source.volume = Mathf.Clamp(volume, 0f, masterVolume);
				yield return new WaitForEndOfFrame();
			}
		}

		IEnumerator IFadeout(Layer layer, float duration)
		{
			AudioSource source = layer.source;

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
