using UnityEngine;
using System.Collections;

public class SoundPool : MonoBehaviour 
{
	[SerializeField] private bool playRandomOnAwake = false;

	[SerializeField] private AudioClip[] sounds;

	void Awake()
	{
		if (playRandomOnAwake)
			PlayRandom();
	}

	public void PlayElement(int index)
	{
		this.audio.PlayOneShot(sounds[index]);
	}

	public void PlayRandom()
	{
		this.audio.PlayOneShot(RandomClip());
	}

	private AudioClip RandomClip()
	{
		return sounds[Random.Range(0, sounds.Length)];
	}
}