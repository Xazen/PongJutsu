using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SetupBase : MonoBehaviour
{
	[HideInInspector]
	public List<GameObject> Instances = new List<GameObject>();
	
	public virtual void build()
	{

	}

	public virtual void remove()
	{
		if (Instances != null)
		{
			foreach (GameObject instance in Instances)
			{
				if (instance.GetComponent<PhotonView>())
					PhotonNetwork.Destroy(instance);
				else
					Destroy(instance);
			}

			Instances.Clear();
		}
	}
}
