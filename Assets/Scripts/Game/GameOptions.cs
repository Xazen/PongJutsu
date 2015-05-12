using UnityEngine;
using System.Collections;
using UnityEngine.Audio;

public class GameOptions : MonoBehaviour
{
	[SerializeField]
	private AudioMixerGroup _effectsMixerGroup;
	private static AudioMixerGroup effectsMixerGroup;

	public static float effectsVolume
	{
		get
		{
			return GetVolume(effectsMixerGroup);
		}
		set
		{
			SetVolume(effectsMixerGroup, value);
			PlayerPrefs.SetFloat("EffectsVolume", value);
		}
	}

	[SerializeField]
	private AudioMixerGroup _musicMixerGroup;
	private static AudioMixerGroup musicMixerGroup;

	public static float musicVolume
	{
		get
		{
			return GetVolume(musicMixerGroup);
		}
		set
		{
			SetVolume(musicMixerGroup, value);
			PlayerPrefs.SetFloat("MusicVolume", value);
		}
	}

	private static void SetVolume(AudioMixerGroup mixerGroup, float volume)
	{
		mixerGroup.audioMixer.SetFloat("Vol_" + mixerGroup.name, VolumeToDecibel(volume));
	}

	private static float GetVolume(AudioMixerGroup mixerGroup)
	{
		float vol = 0f;
		mixerGroup.audioMixer.GetFloat("Vol_" + mixerGroup.name, out vol);
		return DecibelToVolume(vol);
	}

	private static float DecibelToVolume(float decibel)
	{
		return Mathf.Clamp01(Mathf.Pow(10f, (decibel / 20f)));
	}

	private static float VolumeToDecibel(float volume)
	{
		return Mathf.Clamp(Mathf.Log10(volume) * 20f, -80f, 20f);
	}

	void Start()
	{
		effectsMixerGroup = _effectsMixerGroup;
		musicMixerGroup = _musicMixerGroup;

		effectsVolume = PlayerPrefs.GetFloat("EffectsVolume", 1f);
		musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
	}

	void OnApplicationQuit()
	{
		PlayerPrefs.Save();
	}
}
