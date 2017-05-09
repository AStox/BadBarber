using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuideCurve : MonoBehaviour {

	BezierCurve curve;
	Vector3[] points;
	LineRenderer line;

	public void DrawLine () {
		curve = GetComponent<BezierCurve>();
		line = GetComponent<LineRenderer>();
		int length = 100;
		line.positionCount = length;
		points = new Vector3[length];
		for (int i = 0; i < length; i++) {
			points[i] = curve.GetPointAt((float)i/(float)length);
		}
		line.SetPositions(points);
		line.materials[0].mainTextureScale = new Vector3(20, 1, 1);
	}


	void Update () {

	}
}
