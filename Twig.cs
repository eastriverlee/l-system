using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Twig : MonoBehaviour
{
	public LineRenderer line;
	public Vector3 root;
    public float maxWidth = .02f;
    public int depth = 0;
    public float thickness = 1f;
    private Vector3 source;
    public Vector3 destination;
    public float startTime;
    public float endTime;
    private float duration;
	void Start()
	{
		line = GetComponent<LineRenderer>();
        thickness = maxWidth * (Global.timeLimit-endTime)/Global.timeLimit; 
        source = line.GetPosition(0);
        duration = endTime - startTime;
	}

private bool hasStarted = false;
private float progress = 0;
	void Update()
	{
        if (startTime < Global.time && progress < 1)
        {
            if (!hasStarted)
            {
                line.SetPosition(line.positionCount++, source);
                hasStarted = true;
            }
            progress = Mathf.Max(0, (Global.time - startTime)) / duration;
            line.SetPosition(1, Vector3.Lerp(source, destination, progress));
        }
        if (hasStarted && line.startWidth < thickness)
        {
			line.startWidth += 0.0001f;
			line.endWidth += 0.0001f;
        }
	}
}