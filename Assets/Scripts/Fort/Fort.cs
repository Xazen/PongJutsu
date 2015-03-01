using UnityEngine;
using System.Collections;

public class Fort : MonoBehaviour
{

	public int maxHealth = 100;
	public bool mirror = false;

	[HideInInspector] public int health;

	public AnimatorOverrideController FortLeftController;
	public AnimatorOverrideController FortRightController;

	public bool disableAtDestroy = false;
	public bool removeAtDestroy = false;

	[HideInInspector] public bool isDestroyed = false;

	[HideInInspector] public GameObject owner;

	public void Setup()
	{
		if (mirror)
		{
			// Mirror the Fort
			Vector3 scale = this.transform.localScale;
			this.transform.localScale = new Vector3(scale.x * -1, scale.y);
		}

		// Set different forts
		if (this.tag == "FortLeft")
		{
			this.GetComponent<Animator>().runtimeAnimatorController = FortLeftController;
			owner = GameObject.FindGameObjectWithTag("PlayerLeft");
		}
		else if (this.tag == "FortRight")
		{
			this.GetComponent<Animator>().runtimeAnimatorController = FortRightController;
			owner = GameObject.FindGameObjectWithTag("PlayerRight");
		}

		// Intit values
		health = maxHealth;
		UpdateHealthValues();
	}

	void UpdateHealthValues()
	{
		this.GetComponentInChildren<Healthbar>().updateHealthbar(health);
		this.GetComponent<Animator>().SetInteger("Health", health);
	}

	public void TakeHeal(int heal)
	{
		if (!isDestroyed)
		{
			health += heal;
			health = Mathf.Clamp(health, 0, maxHealth);

			UpdateHealthValues(); 
		}
	}

	public void TakeDamage(int damage)
	{
		if (!isDestroyed)
		{
			GameScore.GetByEnemyPlayer(owner).dealtdamageRound += damage + Mathf.Min(health - damage, 0);

			health -= damage;
			health = Mathf.Clamp(health, 0, maxHealth);

			UpdateHealthValues();

			if (health <= 0)
				DestroyFort();
		}
	}

	void DestroyFort()
	{
		isDestroyed = true;

		if (this.tag == "FortLeft")
			GameObject.FindGameObjectWithTag("PlayerRight").GetComponent<Player>().addCombo();
		else if (this.tag == "FortRight")
			GameObject.FindGameObjectWithTag("PlayerLeft").GetComponent<Player>().addCombo();

		if (removeAtDestroy)
		{
			// Destroy Fort
			Destroy(this.gameObject);
		}
		else if (disableAtDestroy)
		{
			// Disable Fort
			this.collider2D.enabled = false;
		}
	}
}
