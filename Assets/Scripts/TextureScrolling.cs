using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class TextureScrolling : MonoBehaviour
	{
		[SerializeField] public float speed = -0.1f;

		void Update()
		{
			this.renderer.material.mainTextureOffset = new Vector2(this.renderer.material.mainTextureOffset.x + (speed / 20f) * Time.deltaTime, 0f);
		}
	}
}
