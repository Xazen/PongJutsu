using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class SoundManager : MonoBehaviour
	{

		public Clip[] clips;

		[System.Serializable]
		public class Clip
		{
			public AudioClip clip;
			public SoundState state;
		}

		public static void changeState(SoundState state)
		{
			Debug.Log("play state: " + state);
		}
	}
}

public enum SoundState
{
	MainMenu,
	InGame	
}
