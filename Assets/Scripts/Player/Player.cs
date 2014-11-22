using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Player : MonoBehaviour
	{

		public float movementSpeed = 5f;
		public bool smoothInput = false;

		public Sprite ninjaLeftSprite;
		public Sprite ninjaRightSprite;

		public Sprite shieldLeftSprite;
		public Sprite shieldRightSprite;

		public bool mirror = false;
		[HideInInspector] public int direction = 1;


		void Start()
		{
			if (mirror)
			{
				// Mirror the Player
				direction = -1;
				Vector3 scale = this.transform.localScale;
				this.transform.localScale = new Vector3(scale.x * -1, scale.y);
			}

			// set different sprites for each player
			if (this.tag == "PlayerLeft")
			{
				this.transform.FindChild("Ninja").GetComponent<SpriteRenderer>().sprite = ninjaLeftSprite;
				this.transform.FindChild("Shield").GetComponent<SpriteRenderer>().sprite = shieldLeftSprite;
			}
			else if (this.tag == "PlayerRight")
			{
				this.transform.FindChild("Ninja").GetComponent<SpriteRenderer>().sprite = ninjaRightSprite;
				this.transform.FindChild("Shield").GetComponent<SpriteRenderer>().sprite = shieldRightSprite;
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

			// Smooth or Raw input
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

