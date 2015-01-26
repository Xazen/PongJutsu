using UnityEngine;
using System.Collections;

public class ShurikenSonic : MonoBehaviour 
{
	public void setColor(Color color)
	{
		this.GetComponent<SpriteRenderer>().color = color;
	}

	void ae_Remove()
	{
		Destroy(this.gameObject);
	}
}
