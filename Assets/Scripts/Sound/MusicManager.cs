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
			public Part[] parts;
			[HideInInspector] public int currentPartIndex = 0;
			[HideInInspector] public int nextPartIndex = 0;
		}

		[System.Serializable]
		public class Part
		{
			public AudioClip[] clips;
			public AudioClip currentClip { get { return clips[currentClipIndex]; } }
			[HideInInspector] public int currentClipIndex = 0;
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
				NextPart(layers[0], true);
			if (Input.GetKeyDown("2"))
				NextPart(layers[0], false);
			if (Input.GetKeyDown("3"))
				NextPart(layers[1], true);
			if (Input.GetKeyDown("4"))
				NextPart(layers[1], false);

			Debug.Log(layers[0].currentPartIndex + " " + layers[0].nextPartIndex + " " + layers[0].parts[layers[0].currentPartIndex].currentClipIndex);
		}

		public void StartMusic()
		{
			StartLayer(layers[startLayerElement]);
		}

		public void EndMusic()
		{
			foreach (AudioSource audio in this.GetComponents<AudioSource>())
			{
				this.StopAllCoroutines();
				audio.Stop();
				audio.clip = null;
				audio.volume = masterVolume;
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

		private void PlayPart(Layer layer, int partIndex)
		{
			layer.source.clip = layer.parts[partIndex].currentClip;
			layer.source.time = 0f;

			layer.currentPartIndex = partIndex;
			StartCoroutine(ILoopPart(layer));

			layer.source.volume = masterVolume;

			if (!layer.source.isPlaying)
				layer.source.Play();
		}

		private void PlayOnce(Layer layer, int partIndex)
		{
			layer.currentPartIndex = partIndex;

			layer.source.volume = masterVolume;

			layer.source.PlayOneShot(layer.parts[partIndex].currentClip);
		}

		private void StartLayer(Layer layer)
		{
			PlayPart(layer, 0);
		}

		public void NextPart(Layer layer, bool wait)
		{
			layer.nextPartIndex = Mathf.Clamp(layer.currentPartIndex + 1, 0, layer.parts.Length - 1);

			if (wait)
			{
				StartCoroutine(IWaitForNextPart(layer));
			}
			else
			{
				StartCoroutine(IFadeout(layer, 0.7f));
				StartCoroutine(IBridgeToNextPart(layer));
			}
		}

		IEnumerator ILoopPart(Layer layer)
		{
			int partIndex = layer.currentPartIndex;
			while (layer.currentPartIndex == partIndex)
			{
				Part currentPart = layer.parts[layer.currentPartIndex];
				int nextClipIndex = (int)Mathf.Repeat(currentPart.currentClipIndex + 1, currentPart.clips.Length);

				yield return new WaitForSeconds(layer.source.clip.length - layer.source.time);

				if (layer.currentPartIndex == partIndex && layer.parts[layer.currentPartIndex].currentClipIndex != nextClipIndex)
				{
					layer.source.clip = currentPart.clips[nextClipIndex];
					layer.source.time = 0f;

					currentPart.currentClipIndex = nextClipIndex;

					if (!layer.source.isPlaying)
						layer.source.Play();
				}
			}
		}

		IEnumerator IWaitForNextPart(Layer layer)
		{
			yield return new WaitForSeconds(layer.source.clip.length - layer.source.time);
			PlayPart(layer, layer.nextPartIndex);
		}

		IEnumerator IBridgeToNextPart(Layer layer)
		{
			int randomBridgeIndex = Random.Range(0, layers[bridgeLayerElement].parts.Length);

			PlayOnce(layers[bridgeLayerElement], randomBridgeIndex);
			yield return new WaitForSeconds(layers[bridgeLayerElement].parts[randomBridgeIndex].clips.Length);
			PlayPart(layer, layer.nextPartIndex);
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
