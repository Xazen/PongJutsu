using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;

namespace PongJutsu
{
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

			AnimatorParameter tar = (AnimatorParameter)target;
			if (GUILayout.Button("Get Parameters"))
			{
				AnimatorController animatorController = AnimatorController.GetEffectiveAnimatorController(tar.GetComponent<Animator>());

				List<string> parameter = new List<string>();
				for (int i = 0; i < animatorController.parameterCount; i++)
				{
					if (animatorController.GetParameter(i).type == AnimatorControllerParameterType.Trigger)
						parameter.Add(animatorController.GetParameter(i).name);
				}

				tar.trigger = parameter.ToArray();
			}
		}
	}
}

#else

namespace PongJutsu
{
	public class AnimatorParameter : MonoBehaviour
	{
		public string[] trigger;
	}
}

#endif