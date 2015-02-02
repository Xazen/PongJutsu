using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class PlayerScore : MonoBehaviour
	{
		private int reflections = 0;
		private int catches = 0;
		private int itemhits = 0;
		private int forthits = 0;

		void Update()
		{
			//Debug.Log("r:" + reflections + "; c:" + catches + "; i:" + itemhits + "; f:" + forthits);
		}

		public void plusRefelect()
		{
			reflections += 1;
		}

		public void plusCatch()
		{
			catches += 1;
		}

		public void plusItemHit()
		{
			itemhits += 1;
		}

		public void plusFortHit()
		{
			forthits += 1;
		}
	}
}
