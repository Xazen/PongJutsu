using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;

[RequireComponent(typeof(Animator))]
public class AnimatorParameter : MonoBehaviour
{
	public string[] trigger;
}

[CustomEditor(typeof(AnimatorParameter))]
public class b : Editor
{
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();

		AnimatorParameter self = (AnimatorParameter)target;
		if (GUILayout.Button("Get Trigger"))
		{
			List<string> parameterList = new List<string>();
			foreach (AnimatorControllerParameter parameter in self.GetComponent<Animator>().parameters)
			{
				if (parameter.type == AnimatorControllerParameterType.Trigger)
					parameterList.Add(parameter.name);
			}

			self.trigger = parameterList.ToArray();
		}
	}
}

#else

public class AnimatorParameter : MonoBehaviour
{
	public string[] trigger;
}

#endif