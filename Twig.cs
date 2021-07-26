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
	void Start()
	{
		line = GetComponent<LineRenderer>();
        thickness = Mathf.Max(maxWidth * (Global.timeLimit-endTime)/Global.timeLimit, line.startWidth); 
        source = line.GetPosition(0);
        duration = endTime - startTime;
	}

private bool hasStarted = false;
private float progress = 0;
	void Update()
	{
        if (startTime < Global.time && Global.time < endTime)
        {
            if (!hasStarted)
            {
                line.SetPosition(line.positionCount++, source);
                hasStarted = true;
            }
            progress = Mathf.Max(0, (Global.time - startTime)) / duration;
            line.SetPosition(1, Vector3.Lerp(source, destination, progress));
			float width = Mathf.Lerp(line.startWidth, thickness, progress);
			line.startWidth = width;
			line.endWidth = width;
        }
	}
}