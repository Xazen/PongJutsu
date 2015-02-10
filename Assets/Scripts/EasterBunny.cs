using UnityEngine;
using System.Collections;

namespace Easter
{
	public class EasterBunny : MonoBehaviour
	{
		[SerializeField]
		private GameObject EasterObject;

		void OnEnable()
		{
			if (EasterObject != null)
			{
				EasterObject.GetComponent<Easter>().StopEaster();
				EasterObject.GetComponent<Easter>().ActivateEasterSpawn();
			}
		}
		void OnDisable()
		{
			if (EasterObject != null)
				EasterObject.GetComponent<Easter>().DeactivateEasterSpawn();
		}
	}
}
