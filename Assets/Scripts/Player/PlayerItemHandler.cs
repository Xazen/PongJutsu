using UnityEngine;
using System.Collections;

public class PlayerItemHandler : PlayerBase
{
	bool isSlow = false;
	float slowDuration;

	public void Slow(Slow slow)
	{
		StartCoroutine("ISlow", slow);
	}

	IEnumerator ISlow(Slow slow)
	{
		slowDuration = slow.duration;
		if (isSlow)
			yield break;

		isSlow = true;
		float originalSpeed = PlayerMovement.maxMovementSpeed;
		PlayerMovement.maxMovementSpeed *= slow.speedMuliplier;

		while (slowDuration > 0f)
		{
			slowDuration -= Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
		}

		PlayerMovement.maxMovementSpeed = originalSpeed;
		isSlow = false;
	}

	// - - - - - - - - - - - - - - - - - - - - -

	bool isInverted = false;
	float invertDuration;

	public void Inverter(Inverter inverter)
	{
		StartCoroutine("IInvert", inverter);
	}

	IEnumerator IInvert(Inverter inverter)
	{
		invertDuration = inverter.duration;
		if (isInverted)
			yield break;

		PlayerMovement.invertDirection = true;
		isInverted = true;

		while (invertDuration > 0f)
		{
			invertDuration -= Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
		}

		PlayerMovement.invertDirection = false;
		isInverted = false;
	}

	// - - - - - - - - - - - - - - - - - - - - -

	bool isExpanded = false;
	float expandDuration;

	public void ShieldExpander(ShieldExpander expander)
	{
		StartCoroutine("IExpand", expander);
	}

	IEnumerator IExpand(ShieldExpander expander)
	{
		expandDuration = expander.duration;
		if (isExpanded)
			yield break;

		BoxCollider2D colliderRef = PlayerShield.GetComponent<BoxCollider2D>();
		Transform transformRef = PlayerShield.expanderReference.transform;
		Vector2 initScaleExpander = transformRef.localScale;
		Vector2 initSizeCollider = colliderRef.size;
		transformRef.localScale = new Vector2(transformRef.localScale.x, expander.scaleMultiplier / 1.5f);
		colliderRef.size = new Vector2(colliderRef.size.x, colliderRef.size.y * expander.scaleMultiplier);
		isExpanded = true;

		while (expandDuration > 0f)
		{
			expandDuration -= Time.fixedDeltaTime;
			yield return new WaitForFixedUpdate();
		}

		transformRef.localScale = initScaleExpander;
		colliderRef.size = initSizeCollider;
		isExpanded = false;
	}
}
