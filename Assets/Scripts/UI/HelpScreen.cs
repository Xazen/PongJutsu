using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PongJutsu
{
	public class HelpScreen : UIScript
	{
		[SerializeField] private Sprite[] helpImage;

		private int currentIndex = 0;

		private Image content;

		public override void uiEnable()
		{
			base.uiEnable();

			if (content == null)
				content = this.transform.FindChild("Content").GetComponent<Image>();

			content.sprite = helpImage[currentIndex];
		}

		public override void uiUpdate()
		{
			base.uiUpdate();

			if (Input.anyKeyDown)
				ui.SetTrigger("Back");
		}

		public void back()
		{
			currentIndex = (int)Mathf.Repeat(currentIndex - 1, helpImage.Length);
			content.sprite = helpImage[currentIndex];
		}

		public void next()
		{
			currentIndex = (int)Mathf.Repeat(currentIndex + 1, helpImage.Length);
			content.sprite = helpImage[currentIndex];
		}
	}
}
