using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class GameSetup : MonoBehaviour
	{
		[HideInInspector] public GameObject Container;

		public virtual void build()
		{

		}

		public virtual void remove()
		{
			if (Container != null)
			{
				Destroy(Container);
			}
		}
	}
}

