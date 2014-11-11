using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Shot : MonoBehaviour
	{

		public int speed;
		public int damage;
		private Vector2 movement;

		public Color color1;
		public Color color2;

		[HideInInspector] public GameObject owner;

		[HideInInspector] public int direction;
		
		private bool bounceBack = false;


		void Start()
		{
			colorSetup();

			// Set initial movement
			movement.x = speed * direction;
		}

		void colorSetup()
		{
			// Set diifferent Color for different owner
			if (owner.tag == "PlayerLeft")
			{
				this.GetComponentInChildren<SpriteRenderer>().color = color1;
			}
			else if (owner.tag == "PlayerRight")
			{
				this.GetComponentInChildren<SpriteRenderer>().color = color2;
			}
		}

		void Update()
		{
			// Move the shot
			this.transform.position = new Vector3(this.transform.position.x + movement.x * Time.deltaTime, this.transform.position.y + movement.y * Time.deltaTime);
		}

		void OnCollisionEnter2D(Collision2D col)
		{
			// Get Collisions GameObject
			GameObject colObject = col.gameObject;

			// Collision with Forts
			if (colObject.tag == "FortLeft" || colObject.tag == "FortRight")
			{
				colObject.GetComponent<Fort>().TakeDamage(damage);
				Destroy(this.gameObject);
			}

			// Collision with StageColliders
			if (colObject.tag == "BoundaryTop")
			{
				movement.y = Mathf.Abs(movement.y) * -1;
			}
			else if (colObject.tag == "BoundaryBottom")
			{
				movement.y = Mathf.Abs(movement.y);
			}
			else if (colObject.tag == "BoundaryLeft" || colObject.tag == "BoundaryRight")
			{
				Destroy(this.gameObject);
			}

			// Collision with Players
			if ((colObject.tag == "PlayerLeft" || colObject.tag == "PlayerRight") && owner != col.transform.parent.gameObject)
			{
				movement.x *= -1;
				movement.y = ((colObject.transform.position.y - this.transform.position.y) / colObject.transform.lossyScale.y) * -2;

				bounceBack = true;
			}
			else if ((colObject.tag == "PlayerLeft" || colObject.tag == "PlayerRight") && owner == col.transform.parent.gameObject && bounceBack)
			{
				Destroy(this.gameObject);
			}
		}

		void OnDestroy()
		{
			owner.GetComponentInChildren<PlayerAttack>().shotCount--;
		}
	}
}
