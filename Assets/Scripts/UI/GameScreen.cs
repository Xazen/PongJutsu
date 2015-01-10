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
				this.transform.FindChild("ComboCounterLeft").GetComponent<Text>().text = "x" + GameFlow.combosPlayerLeft;
				this.transform.FindChild("ComboCounterRight").GetComponent<Text>().text = "x" + GameFlow.combosPlayerRight;
			}
		}
	}
}
