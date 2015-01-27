using UnityEngine;
using System.Collections;

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

	void Update()
	{
		Debug.Log(Random.Range(0, 100) < randomProbability);
	}

	public void PlayElement(int index)
	{
		this.audio.PlayOneShot(sounds[index]);
	}

	public void PlayRandom()
	{
		if (Random.Range(0, 100) < randomProbability)
			this.audio.PlayOneShot(RandomClip());
	}

	private AudioClip RandomClip()
	{
		return sounds[Random.Range(0, sounds.Length)];
	}
}