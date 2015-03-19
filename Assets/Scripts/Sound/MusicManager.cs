using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
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
		[HideInInspector]
		public AudioClip currentClip { get { return clips[currentClipIndex]; } }
		[HideInInspector]
		public int currentClipIndex = 0;
	}

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

	private delegate void Switch();
	private Switch OnSwitch;

	private int currentPartIndex = 0;
	private int nextPartIndex = 0;

	[SerializeField]
	private Layer[] musicLayers;
	[SerializeField]
	private Layer bridgeLayer;

	[SerializeField]
	private int leadingLayerElement = 0;
	[SerializeField]
	private int pauseLayerElement = 0;

	[SerializeField]
	private bool debug = false;

	private static MusicManager _current;
	public static MusicManager current
	{
		get
		{
			if (_current == null)
				_current = FindObjectOfType<MusicManager>();

			return _current;
		}
	}

	public void StartMusic(int partIndex = 0)
	{
		PlayPart(partIndex);
		StartCoroutine(IMusicHandler());
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
		foreach (Layer layer in musicLayers)
		{
			if (layer.parts[nextPartIndex].clips.Length > 0)
			{
				layer.source.clip = layer.parts[partIndex].currentClip;
				layer.source.time = 0f;

				layer.source.volume = 1f;

				if (!layer.source.isPlaying)
					layer.source.Play();
			}
		}

		currentPartIndex = partIndex;
		OnSwitch = SwitchEvent_NextClip;
	}

	public void NextPart(bool instant = false)
	{
		SetNextPart(nextPartIndex + 1, instant);
	}

	public void NextPart(int partIndex, bool instant = false)
	{
		SetNextPart(partIndex, instant);
	}

	private void SetNextPart(int partIndex, bool instant)
	{
		nextPartIndex = Mathf.Clamp(partIndex, 0, maxPartsCount - 1);

		if (instant)
		{
			StartCoroutine(IBridgeToNextPart());
		}
		else
		{
			OnSwitch = SwitchEvent_NextPart;
		}
	}

	private void PlayBridge(int clipIndex)
	{
		bridgeLayer.source.volume = 1f;
		bridgeLayer.source.PlayOneShot(bridgeLayer.parts[0].clips[clipIndex]);
	}

	IEnumerator IMusicHandler()
	{
		while (true)
		{
			float last = leadingLayer.source.time;
			while (leadingLayer.source.time < leadingLayer.source.clip.length && leadingLayer.source.time >= last)
			{
				last = leadingLayer.source.time;

				MusicHandlerDebug();

				yield return null;
			}

			if (OnSwitch != null)
				OnSwitch();

			yield return null;
		}
	}

	private void SwitchEvent_NextClip()
	{
		foreach (Layer layer in musicLayers)
		{
			Part currentPart = layer.parts[currentPartIndex];
			int nextClipIndex = (int)Mathf.Repeat(currentPart.currentClipIndex + 1, currentPart.clips.Length);

			if (nextClipIndex < currentPart.clips.Length)
			{
				layer.source.clip = currentPart.clips[nextClipIndex];
				layer.source.time = 0f;

				currentPart.currentClipIndex = nextClipIndex;

				if (!layer.source.isPlaying)
					layer.source.Play();
			}
		}
	}

	private void SwitchEvent_NextPart()
	{
		PlayPart(nextPartIndex);
	}

	IEnumerator IBridgeToNextPart()
	{
		OnSwitch = null;

		foreach (Layer layer in musicLayers)
			StartCoroutine(IFadeout(layer, 0.7f));

		int randomBridgeIndex = Random.Range(0, bridgeLayer.parts[0].clips.Length);
		PlayBridge(randomBridgeIndex);

		float waitForSeconds = bridgeLayer.parts[0].clips[randomBridgeIndex].length;
		while (waitForSeconds > 0f)
		{
			waitForSeconds -= Time.unscaledDeltaTime;
			yield return new WaitForEndOfFrame();
		}

		PlayPart(nextPartIndex);
	}

	IEnumerator IFadein(Layer layer, float duration)
	{
		AudioSource source = layer.source;

		while (source.volume < 1f)
		{
			float volume = source.volume + (1f / duration) * Time.unscaledDeltaTime;
			source.volume = Mathf.Clamp(volume, 0f, 1f);
			yield return new WaitForEndOfFrame();
		}
	}

	IEnumerator IFadeout(Layer layer, float duration)
	{
		AudioSource source = layer.source;

		while (source.volume > 0f)
		{
			float volume = source.volume - (1f / duration) * Time.unscaledDeltaTime;
			source.volume = Mathf.Clamp(volume, 0f, 1f);
			yield return new WaitForEndOfFrame();
		}
	}

	private void MusicHandlerDebug()
	{
		if (debug)
		{
			if (Input.GetKeyDown("1"))
				NextPart(true);
			if (Input.GetKeyDown("2"))
				NextPart(false);

			Debug.Log(currentPartIndex + " " + nextPartIndex + " " + musicLayers[1].parts[currentPartIndex].currentClipIndex);

			string switchName = "null";

			if (OnSwitch != null)
				switchName = OnSwitch.Method.Name;

			Debug.Log("Time to Switch->" + switchName + ": " + System.Math.Round(leadingLayer.parts[currentPartIndex].currentClip.length - leadingLayer.source.time, 2));
		}
	}
}
