using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace PongJutsu
{
	public class GameScreen : UIScript
	{
		[SerializeField] private GameObject comboCounterLeft;
		[SerializeField] private GameObject comboCounterRight;

		[SerializeField] private int startEmissionRate = 4;
		[SerializeField] private float multiplyEmissionRate = 10f;
		[SerializeField] private int maxComboEmission = 15;

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

			foreach (ParticleSystem particleSystem in comboCounterLeft.GetComponentsInChildren<ParticleSystem>())
			{
				if (left > 0)
					particleSystem.emissionRate = startEmissionRate + Mathf.Min(left-1, maxComboEmission) * multiplyEmissionRate;
				else
					particleSystem.emissionRate = 0;
			}

			foreach (ParticleSystem particleSystem in comboCounterRight.GetComponentsInChildren<ParticleSystem>())
			{
				if (right > 0)
					particleSystem.emissionRate = startEmissionRate + Mathf.Min(right-1, maxComboEmission) * multiplyEmissionRate;
				else
					particleSystem.emissionRate = 0;
			}
		}
	}
}
