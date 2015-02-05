using UnityEngine;
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

				foreach (Layer layer in musicLayers)
				{
					if (layer.source.volume > 0f)
						layer.source.volume = _masterVolume;
				}
			}
		}

		[SerializeField] private int pauseLayerElement = 0;

		private int currentPartIndex = 0;
		private int nextPartIndex = 0;
		private int maxPartsCount
		{
			get
			{
				int maxParts = 0;
				foreach (Layer layer in musicLayers)
				{
					if (maxParts < layer.parts.Length)
						maxParts = layer.parts.Length;
				}
				return maxParts;
			}
		}

		[SerializeField] private int leadingLayerElement = 0;
		private Layer leadingLayer
		{
			get
			{
				if (musicLayers[leadingLayerElement].parts[currentPartIndex].clips.Length > 0)
				{
					return musicLayers[leadingLayerElement];
				}
				else
				{
					foreach (Layer layer in musicLayers)
					{
						if (layer.parts[currentPartIndex].clips.Length > 0)
							return layer;
					}
					return null;
				}
			}
		}

		[System.Serializable]
		public class Layer
		{
			public AudioSource source;
			public Part[] parts;

			public void Reset()
			{
				source.Stop();
				source.clip = null;

				foreach (Part part in parts)
					part.currentClipIndex = 0;
			}
		}

		[System.Serializable]
		public class Part
		{
			public AudioClip[] clips;
			[HideInInspector] public AudioClip currentClip { get { return clips[currentClipIndex]; } }
			[HideInInspector] public int currentClipIndex = 0;
		}

		[SerializeField] private Layer[] musicLayers;
		[SerializeField] private Layer bridgeLayer;


		public static MusicManager current;

		void Awake()
		{
			if (current == null)
				current = this;
		}

		void Update()
		{
			if (Input.GetKeyDown("1"))
				NextPart(true);
			if (Input.GetKeyDown("2"))
				NextPart(false);

			Debug.Log(currentPartIndex + " " + nextPartIndex + " " + musicLayers[1].parts[currentPartIndex].currentClipIndex);
		}

		public void StartMusic()
		{
			PlayPart(0);
		}

		public void StopMusic()
		{
			this.StopAllCoroutines();

			currentPartIndex = 0;
			nextPartIndex = 0;

			foreach (Layer layer in musicLayers)
				layer.Reset();
		}

		public void PauseMusic()
		{
			foreach (Layer layer in musicLayers)
			{
				if (musicLayers[pauseLayerElement].source != layer.source)
					StartCoroutine(IFadeout(layer, 0.7f));
			}
		}

		public void ResumeMusic()
		{
			foreach (Layer layer in musicLayers)
			{
				if (musicLayers[pauseLayerElement].source != layer.source)
					StartCoroutine(IFadein(layer, 1f));
			}
		}

		private void PlayPart(int partIndex)
		{
			currentPartIndex = partIndex;

			foreach (Layer layer in musicLayers)
			{
				if (layer.parts[nextPartIndex].clips.Length > 0)
				{
					layer.source.clip = layer.parts[partIndex].currentClip;
					layer.source.time = 0f;

					StartCoroutine(ILoopPart(layer));

					layer.source.volume = masterVolume;

					if (!layer.source.isPlaying)
						layer.source.Play();
				}
			}
		}

		private void PlayOnce(Layer layer, int partIndex)
		{
			currentPartIndex = partIndex;

			layer.source.volume = masterVolume;

			layer.source.PlayOneShot(layer.parts[partIndex].currentClip);
		}

		public void NextPart(bool wait)
		{
			if (currentPartIndex >= nextPartIndex)
			{
				nextPartIndex = Mathf.Clamp(currentPartIndex + 1, 0, maxPartsCount - 1);

				if (wait)
				{
					StartCoroutine(IWaitForNextPart());
				}
				else
				{
					StartCoroutine(IBridgeToNextPart());
				}
			}
			else
			{
				nextPartIndex = Mathf.Clamp(nextPartIndex + 1, 0, maxPartsCount - 1);
			}
		}

		IEnumerator ILoopPart(Layer layer)
		{
			int  partIndex = currentPartIndex;
			while (currentPartIndex == partIndex)
			{
				Part currentPart = layer.parts[currentPartIndex];
				int nextClipIndex = (int)Mathf.Repeat(currentPart.currentClipIndex + 1, currentPart.clips.Length);

				yield return new WaitForSeconds(layer.source.clip.length - layer.source.time);

				if (currentPartIndex == partIndex && layer.parts[currentPartIndex].currentClipIndex != nextClipIndex)
				{
					layer.source.clip = currentPart.clips[nextClipIndex];
					layer.source.time = 0f;

					currentPart.currentClipIndex = nextClipIndex;

					if (!layer.source.isPlaying)
						layer.source.Play();
				}
			}
		}

		IEnumerator IWaitForNextPart()
		{
			yield return new WaitForSeconds(leadingLayer.source.clip.length - leadingLayer.source.time);
			PlayPart(nextPartIndex);
		}

		IEnumerator IBridgeToNextPart()
		{
			foreach (Layer layer in musicLayers)
				StartCoroutine(IFadeout(layer, 0.7f));

			int randomBridgeIndex = Random.Range(0, bridgeLayer.parts.Length);
			PlayOnce(bridgeLayer, randomBridgeIndex);

			yield return new WaitForSeconds(bridgeLayer.parts[randomBridgeIndex].clips.Length);
			PlayPart(nextPartIndex);
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
