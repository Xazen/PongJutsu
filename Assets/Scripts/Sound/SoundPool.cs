using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class SoundPool : MonoBehaviour 
{
	[SerializeField] private bool playRandomOnAwake = false;
	[SerializeField] private int randomProbability = 100;

	[SerializeField] private AudioClip[] sounds;

	void Awake()
	{
		if (playRandomOnAwake)
			PlayRandom();
	}

	public void PlayElement(int index)
	{
		this.GetComponent<AudioSource>().PlayOneShot(sounds[index]);
	}

	public void PlayRandom()
	{
		if (Random.Range(0, 100) < randomProbability)
			this.GetComponent<AudioSource>().PlayOneShot(RandomClip());
	}

	private AudioClip RandomClip()
	{
		return sounds[Random.Range(0, sounds.Length)];
	}
}