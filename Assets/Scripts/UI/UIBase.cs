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

	private float axisDeadzone = 0.8f;

	private static Animator _ui;
	public static Animator ui
	{
		get
		{
			if (_ui == null)
			{
				_ui = GameObject.Find("UI").GetComponent<Animator>();
			}

			return _ui;
		}
	}

	void OnEnable()
	{
		ResetTriggers();

		uiEnable();

		setDefaultSelection();
	}

	public virtual void uiEnable()
	{
		
	}

	void LateUpdate()
	{
		if (interactable)
		{
			if (!getUInputAny())
				EventSystem.current.sendNavigationEvents = true;
			else
				EventSystem.current.sendNavigationEvents = false;

			GameObject lastSelectedGameObject = EventSystem.current.lastSelectedGameObject;
			GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;

			bool selectInMenu = false;

			foreach (Button button in this.GetComponentsInChildren<Button>())
			{
				if (button.gameObject == lastSelectedGameObject || button.gameObject == currentSelectedGameObject)
				{
					selectInMenu = true;
					break;
				}
			}
			foreach (Slider slider in this.GetComponentsInChildren<Slider>())
			{
				if (slider.gameObject == lastSelectedGameObject || slider.gameObject == currentSelectedGameObject)
				{
					selectInMenu = true;
					break;
				}
			}

			if (getUInputDown() && !selectInMenu)
				setDefaultSelection();
			else if (getUInputDown() && currentSelectedGameObject == null)
				EventSystem.current.SetSelectedGameObject(lastSelectedGameObject);
		}

		uiUpdate();
	}

	public virtual void uiUpdate()
	{

	}

	public bool getUInputDown()
	{
		return (Input.GetButtonDown("Vertical") || Mathf.Abs(Input.GetAxisRaw("Vertical")) >= axisDeadzone || Input.GetButtonDown("Horizontal") || Mathf.Abs(Input.GetAxisRaw("Horizontal")) >= axisDeadzone);
	}

	private bool getUInputAny()
	{
		return getUInputDown() || Input.anyKey;
	}

	private void ResetTriggers()
	{
		foreach (string parameter in ui.GetComponent<AnimatorParameter>().trigger)
		{
			ui.ResetTrigger(parameter);
		}
	}

	public void setDefaultSelection()
	{
		if (defaultSelected != null)
		{
			EventSystem.current.SetSelectedGameObject(null);
			EventSystem.current.SetSelectedGameObject(defaultSelected);
		}
	}
}
