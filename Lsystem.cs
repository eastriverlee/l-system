using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;

namespace Tree
{
	using Rule = Dictionary<char, string>;
	using States = Stack<State>;

	public class State
	{
		public State(Transform transform, GameObject instance_)
		{
			position = transform.position;
			rotation = transform.rotation;
			instance = instance_;
		}
		public Vector3 position;
		public Quaternion rotation;
		public GameObject instance;
	}

	public class Lsystem : MonoBehaviour
	{
		[SerializeField] private int iteration = 4;
		[SerializeField] private float length = .1f;
		[SerializeField] private float width = .1f;
		[SerializeField] private float angle = 30;
		[SerializeField] private GameObject branch;
		[Range(0, 100)] public int speed = 100;
		[Range(0f, 10f)] public float thicknessVariety = 3;
		[Range(0f, 10f)] public int angleVariety = 10;
		private const string axiom = "x";
		private States states;
		private Rule rules;
		private string currentString = "";
		private Vector3 root;
		private int stringLength;
		private float interval;
		private bool isBranching = true;
		void Start()
		{
			root = transform.position;
			states = new States();
			rules = new Rule {
	  			{'x',"[f-[[x]+x]+f[+fx]-x]"},
	    		{'f',"ff"}
			};
			parent = gameObject;
			interval = 25f / speed;
			cursor = new GameObject().transform;
			generate();
			grow();
		}

private bool stop = false;
		void Update()
		{
			if (!stop && Global.time < Global.timeLimit)
				Global.time += Time.deltaTime;
			else
				stop = true;
		}

private GameObject parent;
private GameObject instance;
private LineRenderer line;
private Transform cursor;
private float birth = 0;
private Twig twig;

		private void generate()
		{
			currentString = axiom;
			StringBuilder builder = new StringBuilder();
			for (int i = 0; i < iteration; i++)
			{
				foreach (char c in currentString)
					builder.Append(rules.ContainsKey(c) ? rules[c] : c.ToString());
				currentString = builder.ToString();
				builder = new StringBuilder();
			}
			stringLength = currentString.Length;
		}
		private void grow()
		{
			float resistance = 10f - angleVariety;
			float maxWidth = width * thicknessVariety;
			foreach (char c in currentString)
			switch (c)
			{
				case '+':
					cursor.Rotate(randomize(angle, resistance) * (randBool() ? Vector3.forward : Vector3.back));
					prepareBranch();
					break;
				case '-':
					cursor.Rotate(randomize(angle, resistance) * (randBool() ? Vector3.right : Vector3.left));
					prepareBranch();
					break;
				case '[':
					states.Push(new State(cursor, instance));
					prepareBranch();
					break;
				case ']':
					State state = states.Pop();
					cursor.position = state.position;
					cursor.rotation = state.rotation;
					parent = state.instance;
					birth = parent?.GetComponent<Twig>()?.endTime ?? 0;
					prepareBranch();
					break;
				case 'f':
					if (isBranching)
					{
						instance = Instantiate(branch);
						twig = instance.GetComponent<Twig>();
						instance.transform.SetParent(parent.transform);
						line = instance.GetComponent<LineRenderer>();
						line.SetPosition(0, cursor.position);
						birth += interval;
					}
					else
					{
						twig = instance.GetComponent<Twig>();
						line = instance.GetComponent<LineRenderer>();
					}
					cursor.Translate(Vector3.up * length);
					twig.startTime = birth;
					twig.endTime = birth + interval;
					twig.maxWidth = maxWidth;
					twig.destination = cursor.position;
					isBranching = false;
					Global.timeLimit = Mathf.Max(twig.endTime, Global.timeLimit);
					break;
				default:
					break;
			};
		}
		private void prepareBranch()
		{
			isBranching = true;
		}
		public static bool randBool()
		{
			return (UnityEngine.Random.value > .5);
		}
		public static float randomize(float value, float resistance = 0)
		{
			return ((value * UnityEngine.Random.value + value * resistance) / (resistance + 1));
		}
	}
}