using UnityEngine;
using System.Collections;

namespace PongJutsu
{
	public class AnimationTest : MonoBehaviour
	{

		public GameObject shotObject;
		Animator animator;

		void Start()
		{
			animator = this.GetComponent<Animator>();
		}

		void Update() 
		{
			if (Input.GetButtonDown("PlayerLeft shoot"))
			{
				animator.SetTrigger("Shoot");
			}
		}

		public void Shoot()
		{
			GameObject shotInstance = (GameObject)Instantiate(shotObject, this.transform.position, new Quaternion());
			shotInstance.GetComponent<Shuriken>().setInitialMovement(1, 0);
		}
	}

}
