using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System;
using System.Linq;
using static Global;
using static Extension;

namespace Tree
{
	using Rules = Dictionary<char, string>;
	using States = Stack<State>;

	public class State
	{
		public Vector3 position;
		public Quaternion rotation;
		public GameObject instance;
		public State(Transform transform, GameObject instance_)
		{
			position = transform.position;
			rotation = transform.rotation;
			instance = instance_;
		}
	}

	[Serializable] public struct Rule
	{
		public char target;
		public string replacement;
		public Rule(char target, string replacement)
		{
			this.target = target;
			this.replacement = replacement;
		}
	}

	public class Lsystem : MonoBehaviour
	{
		[SerializeField] private int iteration = 4;
		[SerializeField] private float length = .05f;
		[SerializeField] private float width = .2f;
		[SerializeField] private float angle = 30;
		[SerializeField] private GameObject branch;
		[Range(0, 100)] public int speed = 100;
		[Range(0f, 10f)] public float thicknessVariety = 3;
		[Range(0f, 10f)] public int angleVariety = 10;
		[Range(0f, 10f)] public float delay = 0;
		[Range(0f, 1f)] public float age = 0;
		private States states;
		public Rule[] rawRules = new Rule[] { 
			new Rule('x',"[f-[[x]+x]+f[+fx]-x]"),
				new Rule('f',"ff")
		};
		public string axiom = "x";
		private Rules rules;
		private string currentString = "";
		private Vector3 root;
		private int stringLength;
		private float interval;
		private bool isBranching = true;
		private int i;
		void Start()
		{
			i = time.Length;
			time = time.Append(0).ToArray();
			timeLimit = timeLimit.Append(0).ToArray();
			root = transform.position;
			states = new States();
			rules = makeRules();
			parent = gameObject;
			interval = 25f / speed;
			cursor = new GameObject().transform;
			generate();
			grow();
		}
		private Rules makeRules()
		{
			Rules rules = new Rules();
			foreach (var rawRule in rawRules)
				rules.Add(rawRule.target, rawRule.replacement);
			return (rules);
		}

		private bool stop = false;
		void Update()
		{
			if (delay > 0)
				delay -= Time.deltaTime;
			else if (!stop && time[i] < timeLimit[i])
			{
				time[i] += Time.deltaTime;
				age = Mathf.Min(1, time[i]/timeLimit[i]);
			}
			else
			{
				time[i] = timeLimit[i] * age;
				stop = true;
			}
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
			float resistance = angleVariety/10;
			float minWidth = width / (thicknessVariety+1);
			foreach (char c in currentString)
				switch (c)
				{
					case '+':
						cursor.Rotate(randomize(angle, resistance) * (randomBool() ? Vector3.forward : Vector3.back));
						prepareBranch();
						break;
					case '-':
						cursor.Rotate(randomize(angle, resistance) * (randomBool() ? Vector3.right : Vector3.left));
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
						birth = parent?.GetComponent<Twig>()?.endTime ?? birth;
						prepareBranch();
						break;
					case 'f':
						if (isBranching)
						{
							instance = Instantiate(branch);
							twig = instance.GetComponent<Twig>();
							line = instance.GetComponent<LineRenderer>();
							line.SetPosition(0, cursor.position);
							line.SetPosition(1, cursor.position);
							line.SetWidth(minWidth, minWidth);
							birth += interval;
						}
						else
						{
							twig = instance.GetComponent<Twig>();
							line = instance.GetComponent<LineRenderer>();
						}
						instance.transform.SetParent(parent.transform, false);
						cursor.Translate(Vector3.up * length);
						twig.startTime = birth;
						twig.endTime = birth + interval;
						twig.minWidth = minWidth;
						twig.maxWidth = width;
						twig.destination = cursor.position;
						twig.i = i;
						isBranching = false;
						timeLimit[i] = Mathf.Max(twig.endTime, timeLimit[i]);
						parent = instance;
						break;
					default:
						break;
				};
		}
		private void prepareBranch()
		{
			isBranching = true;
		}
		public static float randomize(float value, float resistance)
		{
			return ((value * UnityEngine.Random.value + value * resistance) / (resistance + 1));
		}
	}
}
