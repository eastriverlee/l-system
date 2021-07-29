using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extension
{
	public static Vector3 randomVector3(float magnitude)
	{
		return new Vector3(UnityEngine.Random.Range(-magnitude,magnitude), UnityEngine.Random.Range(-magnitude,magnitude), UnityEngine.Random.Range(-magnitude,magnitude));
	}
	public static bool randomBool()
	{
		return UnityEngine.Random.value > .5;
	}
	public static bool percent(float probability)
	{
		return UnityEngine.Random.value < probability/100f;
	}
}