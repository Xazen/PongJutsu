using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class Shuriken : MonoBehaviour
	{

		public float speed = 7f;
		[SerializeField] private float speedAdjustment = 1.05f;
		[HideInInspector] public Vector2 movement = new Vector2(0, 0);

		[SerializeField] private bool selfCollision = false;
		[HideInInspector] public bool ignoreSpawnCollision = false;

		public int damage = 25;

		[SerializeField] private GameObject explosion;
		public float explosionRadius = 2f;
		[SerializeField] private float explosionDamageMultiplier = 0.4f;
		[SerializeField] private bool explosionDamagerPerDistance = false;

		public float reflectionDamageMultiplier = 0.8f;

		[SerializeField] private Color shurikenLeftColor = Color.red;
		[SerializeField] private Color shurikenRightColor = Color.blue;

		[SerializeField] private Sprite shurikenLeftSprite;
		[SerializeField] private Sprite shurikenRightSprite;

		[HideInInspector] public GameObject owner;
		[HideInInspector] public GameObject lastHitOwner;

		[HideInInspector] public bool bounceBack = false;

		[SerializeField] private bool resetComboOnDamageDealt = false;
		[SerializeField] private bool resetComboOnDamageTaken = true;

		void Start()
		{
			owner.GetComponentInChildren<PlayerAttack>().shotCount++;

			// Last hit owner might had been set by an item
			if (!lastHitOwner) 
			{
				lastHitOwner = owner;
			}

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

			this.GetComponent<TrailRenderer>().sortingLayerName = this.GetComponentInChildren<SpriteRenderer>().sortingLayerName;
			this.GetComponent<TrailRenderer>().sortingLayerID = this.GetComponentInChildren<SpriteRenderer>().sortingLayerID;
		}

		public void reset()
		{
			speed = Storage.Assign("speed", speed);
			damage = (int)Storage.Assign("damage", (float)damage);
			explosionRadius = Storage.Assign("explosionRadius", explosionRadius);
			explosionDamageMultiplier = Storage.Assign("explosionDamageMultiplier", explosionDamageMultiplier);
			reflectionDamageMultiplier = Storage.Assign("reflectionDamageMultiplier", reflectionDamageMultiplier);
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
				col.GetComponent<Shuriken>().Remove();
				Remove();
			}

			// Collision with Shields
			if (colObject.tag == "Shield" && this.owner != colObject.transform.parent.gameObject)
			{
				damage = (int)(damage * this.reflectionDamageMultiplier);
			}

			// Collision with Forts
			if (colObject.tag == "FortLeft" || colObject.tag == "FortRight")
			{
				if (!colObject.GetComponent<Fort>().isDestroyed)
				{
					if (resetComboOnDamageTaken)
					{
						colObject.GetComponent<Fort>().owner.GetComponent<Player>().resetCombo();
					}

					if (resetComboOnDamageDealt)
					{
						lastHitOwner.GetComponent<Player>().resetCombo();
					}
				}
				Explode(colObject);
			}

			// Collision with StageColliders
			if (colObject.tag == "BoundaryTop")
				movement.y = Mathf.Abs(movement.y) * -1;
			else if (colObject.tag == "BoundaryBottom")
				movement.y = Mathf.Abs(movement.y);
			else if (colObject.tag == "BoundaryLeft" || colObject.tag == "BoundaryRight")
				Remove();
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
				Remove();
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

			Remove();
		}

		public void Remove()
		{
			owner.GetComponentInChildren<PlayerAttack>().shotCount--;
			Destroy(this.gameObject);
		}
	}
}
