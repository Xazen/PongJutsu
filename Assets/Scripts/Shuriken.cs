using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Shuriken : MonoBehaviour
	{

		public float speed = 7f;
		public float speedAdjustment = 1.05f;
		[HideInInspector] public Vector2 movement = new Vector2(0, 0);

		public bool selfCollision = false;
		[HideInInspector] public bool ignoreSpawnCollision = false;

		public int damage = 25;

		public GameObject explosion;
		public float explosionRadius = 2f;
		public float explosionDamageMultiplier = 0.4f;
		public bool explosionDamagerPerDistance = false;

		public float reflectionDamageMultiplier = 0.8f;

		public Color shurikenLeftColor = Color.red;
		public Color shurikenRightColor = Color.blue;

		public Sprite shurikenLeftSprite;
		public Sprite shurikenRightSprite;

		[HideInInspector] public GameObject owner;
		[HideInInspector] public GameObject lastHitOwner;

		[HideInInspector] public bool bounceBack = false;

		void Awake()
		{
			speed *= GameFlow.shurikenSpeedMultiplier;
		}

		void Start()
		{
			owner.GetComponentInChildren<PlayerAttack>().shotCount++;
			lastHitOwner = owner;

			// Set different Color for different owner
			if (owner.tag == "PlayerLeft")
			{
				this.GetComponentInChildren<SpriteRenderer>().sprite = shurikenLeftSprite;
				this.GetComponent<TrailRenderer>().renderer.material.color = shurikenLeftColor;
			}
			else if (owner.tag == "PlayerRight")
			{
				this.GetComponentInChildren<SpriteRenderer>().sprite = shurikenRightSprite;
				this.GetComponent<TrailRenderer>().renderer.material.color = shurikenRightColor;
			}
		}

		public void setInitialMovement(int directionX, float movementY)
		{
			// Set initial movement
			movement.x = speed * directionX;
			movement.y = movementY;
			this.adjustSpeed();
		}

		void Update()
		{
			// Move the shot
			this.transform.position = new Vector2(this.transform.position.x + movement.x * Time.deltaTime, this.transform.position.y + movement.y * Time.deltaTime);
		}

		void OnTriggerEnter2D(Collider2D col)
		{
			// Get Collisions GameObject
			GameObject colObject = col.gameObject;

			// Collision with Shuriken
			if (colObject.tag == "Shuriken" && selfCollision && !ignoreSpawnCollision)
			{
				Destroy(col.gameObject);
				Destroy(this.gameObject);
			}

			// Collision with Shields
			if (colObject.tag == "Shield" && this.owner != colObject.transform.parent.gameObject)
			{
				damage = (int)(damage * this.reflectionDamageMultiplier);
			}

			// Collision with Forts
			if (colObject.tag == "FortLeft" || colObject.tag == "FortRight")
				Explode(colObject);

			// Collision with StageColliders
			if (colObject.tag == "BoundaryTop")
				movement.y = Mathf.Abs(movement.y) * -1;
			else if (colObject.tag == "BoundaryBottom")
				movement.y = Mathf.Abs(movement.y);
			else if (colObject.tag == "BoundaryLeft" || colObject.tag == "BoundaryRight")
				Destroy(this.gameObject);
		}

		// make sure that the shot doesn't stuck in the Boundarys
		void OnTriggerStay2D(Collider2D col)
		{
			GameObject colObject = col.gameObject;

			if (colObject.tag == "BoundaryTop")
			{
				if (this.transform.position.y + this.GetComponent<CircleCollider2D>().radius > colObject.transform.position.y - colObject.GetComponent<BoxCollider2D>().size.y / 2f)
					movement.y = Mathf.Abs(movement.y) * -1;
					this.transform.position = new Vector2(this.transform.position.x, colObject.transform.position.y - colObject.GetComponent<BoxCollider2D>().size.y / 2f - this.GetComponent<CircleCollider2D>().radius * 1.15f);
			}
			else if (colObject.tag == "BoundaryBottom")
			{
				if (this.transform.position.y - this.GetComponent<CircleCollider2D>().radius > colObject.transform.position.y + colObject.GetComponent<BoxCollider2D>().size.y / 2f)
					movement.y = Mathf.Abs(movement.y);
					this.transform.position = new Vector2(this.transform.position.x, colObject.transform.position.y + colObject.GetComponent<BoxCollider2D>().size.y / 2f + this.GetComponent<CircleCollider2D>().radius * 1.15f);
			}
			else if (colObject.tag == "BoundaryLeft" || colObject.tag == "BoundaryRight")
			{
				Destroy(this.gameObject);
			}
		}

		// prevent self collision on spawn
		void OnTriggerExit2D(Collider2D col)
		{
			if (col.gameObject.tag == "Shuriken")
				ignoreSpawnCollision = false;
		}

		public void adjustSpeed()
		{
			movement.x += (Mathf.Sqrt(Vector2.SqrMagnitude(movement)) - speed) * (Mathf.Sign(movement.x) * -1) * (speedAdjustment * 1.08f);
		}

		void Explode(GameObject hitObject)
		{
			hitObject.GetComponent<Fort>().TakeDamage(damage);

			GameObject explosionAnimation = (GameObject)Instantiate(explosion, this.transform.position, Quaternion.identity);
			explosionAnimation.GetComponent<ShurikenExplosion>().explosionRadius = explosionRadius;
			explosionAnimation.GetComponent<ShurikenExplosion>().direction = Mathf.Sign(movement.x);

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
