using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsScreen : UIBase
{
	[SerializeField]
	Slider effectsVolumeSlider;

	[SerializeField]
	Slider musicVolumeSlider;

	public override void uiEnable()
	{
		base.uiEnable();

		effectsVolumeSlider.value = GameOptions.effectsVolume;
		musicVolumeSlider.value = GameOptions.musicVolume;
	}

	public override void uiUpdate()
	{
		base.uiUpdate();

		if (Input.GetButtonDown("Cancel"))
			click_Back();
	}

	public void slide_MasterVolume(float value)
	{
		GameOptions.effectsVolume = value;
	}

	public void slide_MusicVolume(float value)
	{
		GameOptions.musicVolume = value;
	}

	public void click_Back()
	{
		ui.SetTrigger("Back");
	}
}
