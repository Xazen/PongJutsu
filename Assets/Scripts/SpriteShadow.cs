using UnityEngine;
using System.Collections;

namespace Pongjutsu
{
	public class SpriteShadow : MonoBehaviour
	{

		[SerializeField] private float heightAboveGround = 0.1f;

		[SerializeField] private Color shadowColor = new Color(0f, 0f, 0f, 0.235f);

		[SerializeField] private bool AddAsChild = false;

		[SerializeField] private bool updateSprite = false;
		[SerializeField] private Sprite customSprite;

		private float heightMultiplier = 0.45f;

		private GameObject shadow;

		void Awake()
		{
			if (this.GetComponent<SpriteRenderer>().sprite != null)
			{
				shadow = new GameObject();
				shadow.name = "Shadow";

				if (this.transform.parent != null)
					shadow.transform.position = new Vector2(this.transform.parent.position.x * (1f - heightAboveGround * heightMultiplier), this.transform.parent.position.y * (1f - heightAboveGround));
				else
					shadow.transform.position = new Vector2(this.transform.position.x * (1f - heightAboveGround * heightMultiplier), this.transform.position.y * (1f - heightAboveGround));

				if (AddAsChild)
				{
					shadow.transform.parent = this.transform;
					shadow.transform.position = new Vector2(shadow.transform.position.x, shadow.transform.position.y + this.transform.localPosition.y);
					shadow.transform.localScale = new Vector3(1f, 1f, 1f);
					shadow.transform.localRotation = Quaternion.identity;
				}
				else
				{
					shadow.transform.parent = this.transform.parent;
					shadow.transform.localScale = this.transform.localScale;
				}

				Sprite shadowSprite;
				if (customSprite != null)
					shadowSprite = customSprite;
				else
					shadowSprite = this.GetComponent<SpriteRenderer>().sprite;

				shadow.AddComponent<SpriteRenderer>();
				shadow.GetComponent<SpriteRenderer>().sortingLayerID = this.GetComponent<SpriteRenderer>().sortingLayerID;
				shadow.GetComponent<SpriteRenderer>().sortingOrder = this.GetComponent<SpriteRenderer>().sortingOrder - 1;
				shadow.GetComponent<SpriteRenderer>().sprite = shadowSprite;
				shadow.GetComponent<SpriteRenderer>().color = shadowColor;
			}
		}

		void LateUpdate()
		{
			if (this.transform.parent != null)
				shadow.transform.position = new Vector2(this.transform.parent.position.x * (1f - heightAboveGround * heightMultiplier), this.transform.parent.position.y * (1f - heightAboveGround));
			else
				shadow.transform.position = new Vector2(this.transform.position.x * (1f - heightAboveGround * heightMultiplier), this.transform.position.y * (1f - heightAboveGround));

			if (AddAsChild)
				shadow.transform.position = new Vector2(shadow.transform.position.x, shadow.transform.position.y + this.transform.localPosition.y);
			else
				shadow.transform.rotation = this.transform.rotation;

			if (updateSprite)
				shadow.GetComponent<SpriteRenderer>().sprite = this.GetComponent<SpriteRenderer>().sprite;
		}
	}
}
