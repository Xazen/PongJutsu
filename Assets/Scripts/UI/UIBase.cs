using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIBase : MonoBehaviour
{
	[SerializeField]
	private GameObject defaultSelected;
	[SerializeField]
	private bool interactable = false;
	private bool hasInteractable = false;
	private bool allowInput = true;

	public static Animator ui;

	void Awake()
	{
		if (ui == null)
			ui = GameObject.Find("UI").GetComponent<Animator>();

		if (this.GetComponentsInChildren<Button>().Length > 0 || this.GetComponentsInChildren<Slider>().Length > 0)
			hasInteractable = true;
	}

	void OnEnable()
	{
		uiEnable();

		if (getUInputAny() || !interactable)
		{
			allowInput = false;
			EventSystem.current.sendNavigationEvents = false;
		}
	}

	public virtual void uiEnable()
	{
		ResetTriggers();
		setDefaultSelection();
	}

	void LateUpdate()
	{
		if (interactable && !allowInput)
		{
			if (!getUInputAny())
				allowInput = true;
			else
				EventSystem.current.sendNavigationEvents = false;
		}

		if (allowInput || !interactable)
			uiUpdate();
	}

	public virtual void uiUpdate()
	{
		if (hasInteractable)
		{
			bool selectInMenu = false;
			foreach (Button button in this.GetComponentsInChildren<Button>())
			{
				if (button.gameObject == EventSystem.current.lastSelectedGameObject || button.gameObject == EventSystem.current.currentSelectedGameObject)
				{
					selectInMenu = true;
					break;
				}
			}
			foreach (Slider slider in this.GetComponentsInChildren<Slider>())
			{
				if (slider.gameObject == EventSystem.current.lastSelectedGameObject || slider.gameObject == EventSystem.current.currentSelectedGameObject)
				{
					selectInMenu = true;
					break;
				}
			}

			if (getUInputDown() && !selectInMenu)
				setDefaultSelection();
			else if (getUInputDown() && EventSystem.current.currentSelectedGameObject == null)
				EventSystem.current.SetSelectedGameObject(EventSystem.current.lastSelectedGameObject);
		}
	}

	public bool getUInputDown()
	{
		return (Input.GetButtonDown("Vertical") || Input.GetAxisRaw("Vertical") != 0f || Input.GetButtonDown("Horizontal") || Input.GetAxisRaw("Horizontal") != 0f);
	}

	private bool getUInputAny()
	{
		return getUInputDown() || Input.anyKey;
	}

	void ResetTriggers()
	{
		foreach (string parameter in ui.GetComponent<AnimatorParameter>().trigger)
		{
			ui.ResetTrigger(parameter);
		}
	}

	public void setDefaultSelection()
	{
		if (defaultSelected != null && hasInteractable)
		{
			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(defaultSelected);
		}
	}
}
