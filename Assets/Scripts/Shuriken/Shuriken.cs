﻿using UnityEngine;
using System.Collections;

public class Shuriken : MonoBehaviour
{

	public float speed = 7f;

	[SerializeField]
	private float speedAdjustment = 1.05f;
	[HideInInspector]
	public Vector2 movement = new Vector2(0, 0);

	[SerializeField]
	private bool selfCollision = false;
	[HideInInspector]
	public bool ignoreSpawnCollision = false;

	public int damage = 25;

	[SerializeField]
	private GameObject explosion;
	[SerializeField]
	private GameObject bombExplosion;

	public float explosionRadius = 2f;
	[SerializeField]
	private float explosionDamageMultiplier = 0.4f;
	[SerializeField]
	private bool explosionDamagerPerDistance = false;
	[HideInInspector]
	public bool isBomb = false;

	public float reflectionDamageMultiplier = 0.8f;

	public Color shurikenLeftColor = Color.red;
	public Color shurikenRightColor = Color.blue;

	[SerializeField]
	private Sprite shurikenLeftSprite;
	[SerializeField]
	private Sprite shurikenRightSprite;
	[SerializeField]
	private Sprite shurikenBombSprite;

	[HideInInspector]
	public GameObject owner;
	[HideInInspector]
	public GameObject lastHitOwner;

	[HideInInspector]
	public bool bounceBack = false;

	[SerializeField]
	private bool resetComboOnDamageDealt = false;
	[SerializeField]
	private bool resetComboOnDamageTaken = true;

	[SerializeField]
	private GameObject wallCollision;

	void Start()
	{
		owner.GetComponent<PlayerAttack>().shotCount++;

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
			this.GetComponentInChildren<ParticleSystem>().emissionRate = 1 + Mathf.Min(GameVar.players.left.comboCount, 15);
			this.GetComponentInChildren<ParticleSystem>().startColor = shurikenLeftColor;
		}
		else if (owner.tag == "PlayerRight")
		{
			this.GetComponentInChildren<SpriteRenderer>().sprite = shurikenRightSprite;
			this.GetComponent<TrailRenderer>().renderer.material.color = shurikenRightColor;
			this.GetComponentInChildren<ParticleSystem>().emissionRate = 1 + Mathf.Min(GameVar.players.right.comboCount, 15);
			this.GetComponentInChildren<ParticleSystem>().startColor = shurikenRightColor;
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

	void FixedUpdate()
	{
		// Move the shot
		this.transform.position = new Vector2(this.transform.position.x + movement.x * Time.fixedDeltaTime, this.transform.position.y + movement.y * Time.fixedDeltaTime);
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

				lastHitOwner.GetComponent<Player>().addCombo();
			}
			Explode(colObject);
		}

		// Collision with StageColliders
		if (colObject.tag == "BoundaryTop")
		{
			movement.y = Mathf.Abs(movement.y) * -1;
			Instantiate(wallCollision, this.transform.position, Quaternion.Euler(0f, 0f, 0f));
		}
		else if (colObject.tag == "BoundaryBottom")
		{
			movement.y = Mathf.Abs(movement.y);
			Instantiate(wallCollision, this.transform.position, Quaternion.Euler(0f, 0f, 180f));
		}
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

	public void activateBomb(float damageMultiplier)
	{
		isBomb = true;
		this.GetComponentInChildren<SpriteRenderer>().sprite = shurikenBombSprite;
		damage = Mathf.RoundToInt(damage * damageMultiplier);
	}

	void Explode(GameObject hitObject)
	{
		GameObject explosionAnimation = (GameObject)Instantiate(explosion, this.transform.position, Quaternion.identity);
		explosionAnimation.GetComponent<ShurikenExplosion>().explosionRadius = explosionRadius;
		explosionAnimation.GetComponent<ShurikenExplosion>().direction = Mathf.Sign(movement.x);

		if (!isBomb)
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
		}
		else
		{
			foreach (GameObject fort in hitObject.GetComponent<Fort>().owner.GetComponent<Player>().forts)
			{
				fort.GetComponent<Fort>().TakeDamage(damage);

				if (bombExplosion != null && !fort.GetComponent<Fort>().isDestroyed)
				{
					GameObject f = (GameObject)Instantiate(bombExplosion, fort.transform.position, Quaternion.Euler(bombExplosion.transform.eulerAngles.x, bombExplosion.transform.eulerAngles.y * fort.transform.localScale.x, bombExplosion.transform.eulerAngles.z));
					f.name = fort.name + "(Feedback)";
					f.transform.GetComponent<ItemFeedback>().Setup(fort);
				}
			}
		}

		if (!hitObject.GetComponent<Fort>().isDestroyed)
			GameScore.GetByPlayer(lastHitOwner).forthits += 1;

		Remove();
	}

	public void Remove()
	{
		owner.GetComponent<PlayerAttack>().shotCount--;
		Destroy(this.gameObject);
	}
}