using UnityEngine;
using System.Collections;

public class SetupBase : MonoBehaviour
{
	[HideInInspector]
	public GameObject MainInstance;

	public virtual void build()
	{

	}

	public virtual void remove()
	{
		if (MainInstance != null)
		{
			Destroy(MainInstance);
			MainInstance = null;
		}
	}
}
