using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class UIScript : MonoBehaviour
	{
		[HideInInspector] public GameManager game;
		[HideInInspector] public Animator ui;

		void Awake()
		{
			game = GameObject.FindObjectOfType<GameManager>();
			ui = GameObject.Find("UI").GetComponent<Animator>();
		}
	}
}
