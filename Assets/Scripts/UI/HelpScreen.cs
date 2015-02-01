using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PongJutsu
{
	public class HelpScreen : UIScript
	{
		[SerializeField] private Sprite[] helpImage;
		[SerializeField] private GameObject pageIndicator;

		private int currentIndex = 0;

		private bool blockScrolling;

		private Image content;

		void Start()
		{
			for (int i = 0; i < helpImage.Length; i++)
			{
				GameObject indicator = (GameObject)Instantiate(pageIndicator);
				indicator.transform.SetParent(this.transform.FindChild("Indicators"));

				float width = indicator.GetComponent<RectTransform>().rect.width;
				indicator.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * width - (width * helpImage.Length) / 2f, 0f);
			}

			setPageIndex(currentIndex);
		}

		public override void uiEnable()
		{
			base.uiEnable();

			if (content == null)
				content = this.transform.FindChild("Content").GetComponent<Image>();

			currentIndex = 0;
			content.sprite = helpImage[currentIndex];			
		}

		public override void uiUpdate()
		{
			base.uiUpdate();

			if (blockScrolling && !getUInputDown())
				blockScrolling = false;

			if (Input.GetAxisRaw("Horizontal") < 0f && !blockScrolling)
				next();
			else if (Input.GetAxisRaw("Horizontal") > 0f && !blockScrolling)
				back();

			if (Input.GetButtonDown("Cancel"))
				ui.SetTrigger("Back");
		}

		public void back()
		{
			currentIndex = (int)Mathf.Repeat(currentIndex + 1, helpImage.Length);
			setPageIndex(currentIndex);		
		}

		public void next()
		{
			currentIndex = (int)Mathf.Repeat(currentIndex - 1, helpImage.Length);
			setPageIndex(currentIndex);
		}

		public void setPageIndex(int index)
		{
			content.sprite = helpImage[index];

			foreach (Toggle indicator in this.transform.FindChild("Indicators").GetComponentsInChildren<Toggle>())
			{
				indicator.isOn = false;
			}
			this.transform.FindChild("Indicators").GetChild(index).GetComponent<Toggle>().isOn = true;

			blockScrolling = true;
		}
	}
}
