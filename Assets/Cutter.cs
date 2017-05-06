using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlanarMeshGenerator;

public class Cutter : MonoBehaviour {

	float depth;
	public GameObject meshObj;
	Mesh mesh;
	Collider collider;
	bool cutting;
	Vector3[] verts;

	void Start () {
		cutting = false;
		collider = GetComponent<SphereCollider>();
		mesh = meshObj.GetComponent<MeshFilter>().mesh;
		depth = Camera.main.transform.position.z;
	}

	void Update () {
		Movement();
		verts = mesh.vertices;
		if (cutting) {
			for(int i = 0; i < verts.Length; i++) {
				// if (collider.bounds.Contains(verts[i])) {
				// 	verts[i] = collider.ClosestPoint(meshObj.gameObject.transform.position);
				// }
				if (Vector3.Distance(verts[i], transform.position) < ((SphereCollider)collider).radius) {
					verts[i] = collider.ClosestPoint(verts[i]);
				}
			}
			mesh.vertices = verts;
		}

		if (Input.GetMouseButtonDown(0)) {
			cutting = true;
		}
		if (Input.GetMouseButtonUp(0)) {
			cutting = false;
		}
	}

	void Movement () {
		Vector3 mousePos = Input.mousePosition;
    Vector3 wantedPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth));
    transform.position = wantedPos;
	}
}
