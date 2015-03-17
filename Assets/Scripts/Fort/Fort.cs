using UnityEngine;
using System.Collections;

[RequireComponent(typeof(FortHealthbar))]
public class Fort : MonoBehaviour
{
	Faction _faction;
	public Faction faction
	{
		get
		{
			return _faction;
		}
		set
		{
			_faction = value;

			transform.rotation = _faction.Rotation2D();

			if (_faction == Faction.Left)
			{
				GetComponent<Animator>().runtimeAnimatorController = FortLeftController;
				owner = GameObject.FindGameObjectWithTag("PlayerLeft");
			}
			else if (_faction == Faction.Right)
			{
				GetComponent<Animator>().runtimeAnimatorController = FortRightController;
				owner = GameObject.FindGameObjectWithTag("PlayerRight");
			}
		}
	}

	public int maxHealth = 100;
	private int _health;
	[HideInInspector]
	public int health
	{
		get
		{
			return _health;
		}
		set
		{
			_health = Mathf.Clamp(value, 0, maxHealth);

			GetComponent<FortHealthbar>().updateHealthbar(_health);
			GetComponent<Animator>().SetInteger("Health", _health);

			if (_health <= 0)
				DestroyFort();
		}
	}

	[SerializeField]
	private AnimatorOverrideController FortLeftController;
	[SerializeField]
	private AnimatorOverrideController FortRightController;

	[SerializeField]
	private bool disableAtDestroy = false;
	[SerializeField]
	private bool removeAtDestroy = false;

	[HideInInspector]
	public bool isDestroyed = false;

	[HideInInspector]
	public GameObject owner;

	void Start()
	{
		health = maxHealth;
	}

	public void TakeHeal(int heal)
	{
		if (!isDestroyed)
			health += heal;
	}

	public void TakeDamage(int damage)
	{
		if (!isDestroyed)
		{
			GameScore.GetByEnemyPlayer(owner).dealtdamageRound += damage + Mathf.Min(health - damage, 0);
			health -= damage;
		}
	}

	void DestroyFort()
	{
		isDestroyed = true;

		if (faction == Faction.Left)
			GameObject.FindGameObjectWithTag("PlayerRight").GetComponent<Player>().addCombo();
		else if (faction == Faction.Right)
			GameObject.FindGameObjectWithTag("PlayerLeft").GetComponent<Player>().addCombo();

		if (removeAtDestroy)
		{
			// Destroy Fort
			Destroy(this.gameObject);
		}
		else if (disableAtDestroy)
		{
			// Disable Fort
			this.GetComponent<Collider2D>().enabled = false;
		}
	}
}
