using UnityEngine;
using System.Collections;

public class Divider : ItemBase
{
	public GameObject shotObject;
	public int splits = 2;
	public float angularDistance = 1f;
	public float damagePercentage = 0.5f;

	public override void OnActivation(Shuriken shuriken)
	{
		// Split shuriken
		for (int i = 0; splits > i; i++)
		{
			GameObject shotInstance = (GameObject)Instantiate(shotObject, shuriken.transform.position, Quaternion.identity);

			// Calculate y-movement
			float movementY;
			if (splits % 2 == 0)
				movementY = i * angularDistance - angularDistance * (splits / 2f) + angularDistance / 2f;
			else
				movementY = i * angularDistance - angularDistance * ((splits - 1f) / 2f);

			// Init values
			PlayerAttack playerAttack = shuriken.owner.GetComponent<PlayerAttack>();

			shotInstance.GetComponent<Shuriken>().owner = shuriken.owner;
			shotInstance.GetComponent<Shuriken>().lastHitOwner = shuriken.lastHitOwner;
			shotInstance.GetComponent<Shuriken>().speed *= playerAttack.speedMultiplier;
			shotInstance.GetComponent<Shuriken>().damage = (int)(shuriken.damage * playerAttack.damageMultiplier * damagePercentage);
			shotInstance.GetComponent<Shuriken>().setInitialMovement((int)Mathf.Sign(shuriken.movement.x), shuriken.movement.y + movementY);
			shotInstance.GetComponent<Shuriken>().bounceBack = shuriken.bounceBack;
			shotInstance.GetComponent<Shuriken>().ignoreSpawnCollision = true;
		}

		shuriken.Remove();

		base.OnActivation(shuriken);
	}
}
