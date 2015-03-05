using UnityEngine;
using System.Collections;

public class TextureScrolling : MonoBehaviour
{
	[SerializeField]
	public float speed = -0.1f;

	void Update()
	{
		this.GetComponent<Renderer>().material.mainTextureOffset = new Vector2(this.GetComponent<Renderer>().material.mainTextureOffset.x + (speed / 20f) * Time.deltaTime, 0f);
	}
}
