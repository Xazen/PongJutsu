using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class GameSetup : MonoBehaviour
	{
		[HideInInspector] public GameObject MainInstance;

		public virtual void build()
		{

		}

		public virtual void remove()
		{
			if (MainInstance != null)
			{
				DestroyImmediate(MainInstance);
			}
		}
	}
}

