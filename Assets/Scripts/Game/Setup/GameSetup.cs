using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameSetup : MonoBehaviour
{
	[HideInInspector]
	public GameObject MainInstance;
	[HideInInspector]
	public List<GameObject> Instances;

	public virtual void build()
	{

	}

	public virtual void postbuild()
	{

	}

	public virtual void remove()
	{
		if (MainInstance != null)
		{
			Destroy(MainInstance);
			MainInstance = null;
		}
		if (Instances != null)
		{
			Instances.Clear();
		}
	}
}
