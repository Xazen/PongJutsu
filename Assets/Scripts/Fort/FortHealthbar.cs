using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Fort))]
public class FortHealthbar : MonoBehaviour
{
	[SerializeField]
	private GameObject healthbarReference;
	[SerializeField]
	private SpriteRenderer barReference;

	[SerializeField]
	private Color colorFullHealth = Color.green;
	[SerializeField]
	private Color colorMediumHealth = Color.Lerp(Color.red, Color.yellow, 0.5f);
	[SerializeField]
	private Color colorLowHealth = Color.red;

	[SerializeField]
	private bool removeAtDestroy = true;

	private Vector2 initScale;

	void Awake()
	{
		initScale = barReference.transform.localScale;
	}

	public void updateHealthbar(int health)
	{
		// Set new healtbar scale 
		barReference.transform.localScale = new Vector2(initScale.x, initScale.y * (health / (float)GetComponent<Fort>().maxHealth));

		// Lerp three colors
		if (health > 50)
			barReference.color = Color.Lerp(colorMediumHealth, colorFullHealth, (float)health / (GetComponent<Fort>().maxHealth / 2f) - 1f);
		else if (health <= 50)
			barReference.color = Color.Lerp(colorLowHealth, colorMediumHealth, (float)health / (GetComponent<Fort>().maxHealth / 2f));


		if (health <= 0 && removeAtDestroy)
		{
			// remove healthbar
			Destroy(healthbarReference);
		}
	}
}
