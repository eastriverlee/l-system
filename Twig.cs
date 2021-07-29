using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;
using static Extension;
public class Twig : MonoBehaviour
{
	[HideInInspector] public LineRenderer line;
    [HideInInspector] public float minWidth = .02f;
    [HideInInspector] public float thickness = 1f;
    [HideInInspector] public Vector3 destination;
    [HideInInspector] public float startTime;
    [HideInInspector] public float endTime;
    [HideInInspector] public float maxWidth;
    [HideInInspector] public int i;
    private float duration;
    public Twig parent = null;
	private Renderer renderer;
    private Vector3 source;
	private Vector3 startOffset = Vector3.zero;
	public Vector3 endOffset = Vector3.zero;
	private float length;
	void Start()
	{
		float depth = (timeLimit[i]-endTime)/timeLimit[i];
		line = GetComponent<LineRenderer>();
        thickness = Mathf.Max(maxWidth*depth, minWidth); 
        source = line.GetPosition(0);
        duration = endTime - startTime;
		renderer = GetComponent<Renderer>();
		parent = gameObject.transform.parent.GetComponent<Twig>();
		length = (destination - source).magnitude;
	}

	private float progress = 0;
	void Update()
	{
		renderer.enabled = startTime <= time[i];
		startOffset = parent?.endOffset ?? startOffset;
		line.SetPosition(0, source + startOffset);
		determineGrowth();
	}
	void determineGrowth()
	{
		progress = Mathf.Clamp((time[i] - startTime) / duration, 0, 1);
		line.SetPosition(1, Vector3.Lerp(source, destination + endOffset, progress));
		float currentWidth = Mathf.Lerp(line.startWidth, thickness, progress);
		line.startWidth = currentWidth;
		line.endWidth = currentWidth;
	}
}
