using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Player : MonoBehaviour
	{

		public float movementSpeed = 5f;
		public bool smoothInput = false;
	
		public bool flip = false;
		[HideInInspector] public int direction = 1;


		void Start()
		{
			if (flip)
			{
				// Flip the Player
				direction = -1;
				Vector3 scale = this.transform.localScale;
				this.transform.localScale = new Vector3(scale.x * -1, scale.y);
			}
		}

		void Update() 
		{
			Move();
		}

		void Move()
		{
			// Get current position
			Vector2 position = this.transform.position;

			// Set Smooth or Raw input
			if (smoothInput)
			{
				position.y = position.y + movementSpeed * Input.GetAxis(this.tag) * Time.deltaTime;
			}
			else
			{
				position.y = position.y + movementSpeed * Input.GetAxisRaw(this.tag) * Time.deltaTime;
			}

			// Check and Precalculating Collision
			GameObject top = GameObject.FindGameObjectWithTag("BoundaryTop");
			GameObject bottom = GameObject.FindGameObjectWithTag("BoundaryBottom");
			if (position.y > top.transform.position.y - top.GetComponent<BoxCollider2D>().size.y / 1.4f)
			{
				position.y = top.transform.position.y - top.GetComponent<BoxCollider2D>().size.y / 1.4f;
			}
			else if (position.y < bottom.transform.position.y + bottom.GetComponent<BoxCollider2D>().size.y / 1.4f)
			{
				position.y = bottom.transform.position.y + bottom.GetComponent<BoxCollider2D>().size.y / 1.4f;
			}

			// Set the new position
			this.transform.position = new Vector2(position.x, position.y);
		}
	}
}

