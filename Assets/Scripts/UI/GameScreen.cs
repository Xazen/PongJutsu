using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PongJutsu
{
	public class GameScreen : UIScript
	{
		[SerializeField]
		private GameObject comboCounterLeft;
		[SerializeField]
		private GameObject comboCounterRight;

		public override void uiUpdate()
		{
			base.uiUpdate();

			if (GameVar.players.left.reference == null && GameVar.players.right.reference == null)
			{
				setComboCouter(0, 0);
			}
			else if (GameManager.allowInput)
			{
				setComboCouter(GameVar.players.left.comboCount, GameVar.players.right.comboCount);
			}
		}

		void setComboCouter(int left, int right)
		{
			comboCounterLeft.GetComponent<Text>().text = "x" + left;
			comboCounterRight.GetComponent<Text>().text = "x" + right;

			comboCounterLeft.transform.GetChild(0).particleSystem.emissionRate = left;
			comboCounterRight.transform.GetChild(0).particleSystem.emissionRate = right;
		}
	}
}
