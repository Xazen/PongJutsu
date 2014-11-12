using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Item : MonoBehaviour 
	{

		public virtual void content(Collider2D col)
		{
			
		}

		void OnTriggerEnter2D(Collider2D col)
		{
			if (col.GetComponent<Shot>() != null)
			{
				content(col);
			}

			Destroy(this.gameObject);
		}
	}
}

