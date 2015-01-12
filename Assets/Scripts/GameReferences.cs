using UnityEngine;
using System.Collections;

public class GameReferences : MonoBehaviour 
{
	[SerializeField] private GameObject _shuriken;
	public static GameObject shuriken { get; private set; }

	void Awake()
	{
		shuriken = _shuriken;
	}
}
