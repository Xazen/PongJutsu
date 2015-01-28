using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PongJutsu
{
	public class UIScript : MonoBehaviour
	{
		[SerializeField] private GameObject defaultSelected;
		private bool hasButtons = false;

		public static Animator ui;

		void Awake()
		{
			if (ui == null)
				ui = GameObject.Find("UI").GetComponent<Animator>();

			if (this.GetComponentsInChildren<Button>().Length > 0)
				hasButtons = true;
		}

		void OnEnable()
		{
			ResetTriggers();
			setDefaultSelection();
		}

		void ResetTriggers()
		{
			foreach (string parameter in ui.GetComponent<AnimatorParameter>().trigger)
			{
				ui.ResetTrigger(parameter);
			}
		}

		void Update()
		{
			UIpdate();
		}

		public virtual void UIpdate()
		{
			if (hasButtons)
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

				if (getInputUI() && !selectInMenu)
					setDefaultSelection();
				else if (getInputUI() && EventSystem.current.currentSelectedGameObject == null)
					EventSystem.current.SetSelectedGameObject(EventSystem.current.lastSelectedGameObject);
			}
		}

		private bool getInputUI()
		{
			return (Input.GetButtonDown("Vertical") || Input.GetAxisRaw("Vertical") != 0f || Input.GetButtonDown("Horizontal") || Input.GetAxisRaw("Horizontal") != 0f);
		}

		internal void setDefaultSelection()
		{
			if (defaultSelected != null && hasButtons)
			{
				EventSystem.current.SetSelectedGameObject(null);
				EventSystem.current.SetSelectedGameObject(defaultSelected);
			}
		}
	}
}
