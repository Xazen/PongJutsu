using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PongJutsu
{
	public class GameScreen : UIScript
	{
		void Update()
		{
			if (GameManager.allowInput)
			{
				this.transform.FindChild("ComboCounterLeft").GetComponent<Text>().text = "x" + GameVar.players.left.comboCount;
				this.transform.FindChild("ComboCounterRight").GetComponent<Text>().text = "x" + GameVar.players.right.comboCount;
			}
		}
	}
}
