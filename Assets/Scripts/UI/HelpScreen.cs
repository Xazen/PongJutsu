using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HelpScreen : UIBase
{
	[SerializeField]
	private Sprite[] helpImage;
	[SerializeField]
	private GameObject pageIndicator;

	private int _currentPageIndex = 0;
	private int currentPageIndex { get { return _currentPageIndex; } set { _currentPageIndex = value; setPageIndex(value); } }

	private bool blockScrolling;

	private Image content;

	public override void uiEnable()
	{
		base.uiEnable();

		if (content == null)
		{
			content = this.transform.FindChild("Content").GetComponent<Image>();
			setupPageIndicators();
		}

		currentPageIndex = 0;
	}

	private void setupPageIndicators()
	{
		for (int i = 0; i < helpImage.Length; i++)
		{
			GameObject indicator = (GameObject)Instantiate(pageIndicator);
			indicator.transform.SetParent(this.transform.FindChild("Indicators"));

			float width = indicator.GetComponent<RectTransform>().rect.width;
			indicator.GetComponent<RectTransform>().anchoredPosition = new Vector2(i * width - (width * helpImage.Length) / 2f, 0f);
		}
	}

	public override void uiUpdate()
	{
		base.uiUpdate();

		if (blockScrolling && !getUInputDown())
			blockScrolling = false;

		if (Input.GetAxisRaw("Horizontal") > 0f && !blockScrolling)
			nextPage();
		else if (Input.GetAxisRaw("Horizontal") < 0f && !blockScrolling)
			backPage();

		if (Input.GetButtonDown("Cancel"))
			click_Back();
	}

	public void nextPage()
	{
		currentPageIndex = (int)Mathf.Repeat(currentPageIndex + 1, helpImage.Length);
	}

	public void backPage()
	{
		currentPageIndex = (int)Mathf.Repeat(currentPageIndex - 1, helpImage.Length);
	}

	public void click_Back()
	{
		ui.SetTrigger("Back");
	}

	private void setPageIndex(int index)
	{
		content.sprite = helpImage[index];

		foreach (Toggle indicator in this.transform.FindChild("Indicators").GetComponentsInChildren<Toggle>())
		{
			if (indicator.transform.GetSiblingIndex() == index)
				indicator.isOn = true;
			else
				indicator.isOn = false;
		}

		blockScrolling = true;
	}
}
