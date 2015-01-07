﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PongJutsu
{
	public class Value : MonoBehaviour
	{
		private static List<Variable> variables = new List<Variable>();

		public static float Store(string name, float value)
		{
			if (!variables.Exists(x => x.name == name))
				variables.Add(new Variable(name, value));
			else
				return Get(name);
				//variables[variables.FindIndex(x => x.name == name)].value = value;
			return value;
		}

		public static float Get(string name)
		{
			return variables.Find(x => x.name == name).value;
		}
	}
	public class Variable
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
