using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Shuriken : MonoBehaviour
	{

		public float speed = 7f;
		public int damage = 25;

		[HideInInspector] public Vector2 movement = new Vector2(0, 0);
		public float speedAdjustment = 1.05f;
		public float shieldAngleMultiplier = 5f;

		public bool selfCollision = false;

		public float explosionRadius = 2f;
		public float explosionDamageMultiplier = 0.4f;
		public bool explosionDamagerPerDistance = false;

		public Color colorPlayerLeft = Color.red;
		public Color colorPlayerRight = Color.blue;

		[HideInInspector] public GameObject owner;
		[HideInInspector] public GameObject lastHitOwner;

		[HideInInspector] public bool bounceBack = false;

		void Start()
		{
			owner.GetComponentInChildren<PlayerAttack>().shotCount++;
			lastHitOwner = owner;
			colorSetup();
		}

		public void setInitialMovement(int directionX, float movementY)
		{
			// Set initial movement
			movement.x = speed * directionX;
			movement.y = movementY;
			movement = adjustSpeed(movement);
		}

		public int getDirection()
		{
			int direction = 0;

			// Calculate the current movement direction
			if (movement.x < 0)
				direction = -1;
			else if (movement.x > 0)
				direction = 1;

			return direction;
		}

		void colorSetup()
		{
			// Set diifferent Color for different owner
			if (owner.tag == "PlayerLeft")
				this.GetComponentInChildren<SpriteRenderer>().color = colorPlayerLeft;
			else if (owner.tag == "PlayerRight")
				this.GetComponentInChildren<SpriteRenderer>().color = colorPlayerRight;
		}

		void Update()
		{
			Move();
		}

		void Move()
		{
			// Move the shot
			this.transform.position = new Vector3(this.transform.position.x + movement.x * Time.deltaTime, this.transform.position.y + movement.y * Time.deltaTime);
		}

		void OnCollisionEnter2D(Collision2D col)
		{
			// Get Collisions GameObject
			GameObject colObject = col.gameObject;

			if (colObject.tag == "Shuriken" && selfCollision)
			{
				Destroy(col.gameObject);
				Destroy(this.gameObject);
			}
			
			// Collision with Forts
			if (colObject.tag == "FortLeft" || colObject.tag == "FortRight")
				explode(colObject);

			// Collision with StageColliders
			if (colObject.tag == "BoundaryTop")
				movement.y = Mathf.Abs(movement.y) * -1;
			else if (colObject.tag == "BoundaryBottom")
				movement.y = Mathf.Abs(movement.y);
			else if (colObject.tag == "BoundaryLeft" || colObject.tag == "BoundaryRight")
				Destroy(this.gameObject);

			// Collision with Shields
			if (colObject.tag == "Shield" && owner != col.transform.parent.gameObject)
			{
				movement.x = -movement.x;
				movement.y = (colObject.transform.position.y - this.transform.position.y) * (colObject.transform.lossyScale.y * colObject.GetComponent<BoxCollider2D>().size.y) * -1 * shieldAngleMultiplier;
				movement = adjustSpeed(movement);

				lastHitOwner = colObject.transform.parent.gameObject;
				bounceBack = true;
			}
			else if (colObject.tag == "Shield" && owner == col.transform.parent.gameObject && bounceBack)
			{
				Destroy(this.gameObject);
			}
		}

		Vector2 adjustSpeed(Vector2 vector)
		{
			vector.x += (Mathf.Sqrt(Vector2.SqrMagnitude(vector)) - speed) * (Mathf.Sign(vector.x) * -1) * speedAdjustment;
			return vector;
		}

		void explode(GameObject hitObject)
		{
			hitObject.GetComponent<Fort>().TakeDamage(damage);

			Collider2D[] expl = Physics2D.OverlapCircleAll(this.transform.position, explosionRadius);
			foreach (Collider2D col in expl)
			{
				// Check if the Fort isn't the direct hit fort
				if (col.gameObject != hitObject && col.gameObject.GetComponent<Fort>() != null)
				{
					GameObject fort = col.gameObject;

					// Set Damage Per Distance or Damage Multiplier
					if (explosionDamagerPerDistance)
						fort.GetComponent<Fort>().TakeDamage((int)(damage / Vector2.Distance(this.transform.position, fort.transform.position)));
					else
						fort.GetComponent<Fort>().TakeDamage((int)(damage * explosionDamageMultiplier));
				}
			}

			Destroy(this.gameObject);
		}

		void OnDestroy()
		{
			owner.GetComponentInChildren<PlayerAttack>().shotCount--;
		}
	}
}
