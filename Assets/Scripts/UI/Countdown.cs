using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Countdown : UIScript
{
	private string defaultRoundText = "Round";

	public override void uiEnable()
	{
		base.uiEnable();

		this.GetComponent<SoundPool>().PlayElement(GameMatch.getRound());
		this.transform.FindChild("Round").GetComponent<Text>().text = defaultRoundText + " " + (GameMatch.getRound() + 1);
	}
}
