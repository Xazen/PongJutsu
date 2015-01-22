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

		[HideInInspector] public Animator ui;

		void Awake()
		{
			ui = GameObject.Find("UI").GetComponent<Animator>();

			if (this.GetComponentsInChildren<Button>().Length > 0)
				hasButtons = true;
		}

		void OnEnable()
		{
			setDefaultSelection();
		}

		void Update()
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
				removeSelection();
				EventSystem.current.SetSelectedGameObject(defaultSelected);
			}
		}

		private void removeSelection()
		{
			EventSystem.current.SetSelectedGameObject(null);
		}
	}
}
