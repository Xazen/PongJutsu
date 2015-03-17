using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsScreen : UIBase
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
			click_Back();
	}

	public void slide_MasterVolume(float value)
	{
		GameOptions.masterVolume = value;
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
