using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PongJutsu
{
	public class OptionsScreen : UIScript 
	{
		public override void uiEnable()
		{
			base.uiEnable();

			this.transform.FindChild("sl_MasterVolume").GetComponent<Slider>().value = GameOptions.masterVolume;
			this.transform.FindChild("sl_MusicVolume").GetComponent<Slider>().value = GameOptions.musicVolume;
		}

		public override void uiUpdate()
		{
			base.uiUpdate();

			if (Input.GetButtonDown("Cancel"))
				ui.SetTrigger("Back");
		}

		public void slide_MasterVolume(float value)
		{
			GameOptions.masterVolume = value;
		}

		public void slide_MusicVolume(float value)
		{
			GameOptions.musicVolume = value;
		}
	}
}
