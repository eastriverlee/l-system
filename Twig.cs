using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Twig : MonoBehaviour
{
	[HideInInspector] public LineRenderer line;
    [HideInInspector] public float maxWidth = .02f;
    [HideInInspector] public float thickness = 1f;
    private Vector3 source;
    [HideInInspector] public Vector3 destination;
    [HideInInspector] public float startTime;
    [HideInInspector] public float endTime;
    private float duration;
	private Renderer renderer;
	void Start()
	{
		line = GetComponent<LineRenderer>();
        thickness = Mathf.Max(maxWidth * (Global.timeLimit-endTime)/Global.timeLimit, line.startWidth); 
        source = line.GetPosition(0);
        duration = endTime - startTime;
		renderer = GetComponent<Renderer>();
	}

private float progress = 0;
	void Update()
	{
        renderer.enabled = line.GetPosition(0) != line.GetPosition(1);
		determineGrowth();
	}
	void determineGrowth()
	{     
		progress = Mathf.Clamp((Global.time - startTime) / duration, 0, 1);
		line.SetPosition(1, Vector3.Lerp(source, destination, progress));
		float width = Mathf.Lerp(line.startWidth, thickness, progress);
		line.startWidth = width;
		line.endWidth = width;
	}
}