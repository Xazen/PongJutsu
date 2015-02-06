using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class GameOptions : MonoBehaviour
	{
		[SerializeField]
		private float defaultMasterVolume = 0.8f;
		public static float masterVolume { get { return AudioListener.volume; } set { AudioListener.volume = value; PlayerPrefs.SetFloat("MasterVolume", value); PlayerPrefs.Save(); } }

		[SerializeField]
		private float defaultMusicVolume = 1f;
		public static float musicVolume { get { return MusicManager.current.masterVolume; } set { MusicManager.current.masterVolume = value; PlayerPrefs.SetFloat("MusicVolume", value); PlayerPrefs.Save(); } }

		void Awake()
		{
			masterVolume = PlayerPrefs.GetFloat("MasterVolume", defaultMasterVolume);
			musicVolume = PlayerPrefs.GetFloat("MusicVolume", defaultMusicVolume);
		}
	}
}
