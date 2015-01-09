using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Value : MonoBehaviour
{
	private static List<Variable> variables = new List<Variable>();

	public static float Assign(string name, float value)
	{
		if (!variables.Exists(x => x.name == name))
		{
			variables.Add(new Variable(name, value));
			return value;
		}
		else
		{
			return Get(name);
		}
	}

	public static float Get(string name)
	{
		return variables.Find(x => x.name == name).value;
	}

	private class Variable
	{
		public Variable(string _name, float _value)
		{
			name = _name;
			value = _value;
		}

		public string name { get; set; }
		public float value { get; set; }
	}
}
