using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class UIScript : MonoBehaviour
	{
		[HideInInspector] public Animator ui;

		void Awake()
		{
			ui = GameObject.Find("UI").GetComponent<Animator>();
		}
	}
}
