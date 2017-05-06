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

				// if (Vector3.Distance(verts[i], transform.position) < 0.25f) {
					RaycastHit hit;
					if (Physics.Raycast(meshObj.transform.position, verts[i] - meshObj.transform.position, out hit, Vector3.Distance(meshObj.transform.position, verts[i]))) {
						verts[i] = hit.point;
					// }
					// float distance = Vector3.Distance(verts[i], meshObj.transform.position);
					// Vector3 target1 = transform.position + ((verts[i] - transform.position).normalized * 0.25f);
					// Vector3 target2 = transform.position + ((transform.position - verts[i]).normalized * 0.25f);
					// float newDist1 = Vector3.Distance(target1, meshObj.transform.position);
					// float newDist2 = Vector3.Distance(target2, meshObj.transform.position);
					// if (newDist1 < distance) {
					// 	verts[i] = target1;
					// } else if (newDist2 < distance) {
					// 	verts[i] = target2;
					// }
					// verts[i] = transform.position + ((verts[i] - transform.position).normalized * 0.25f);
				}
			}
			mesh.vertices = verts;
			mesh.RecalculateBounds();
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

	public void OnDrawGizmos () {
		for(int i = 0; i < verts.Length; i++) {
			Gizmos.DrawSphere(verts[i], 0.1f);
		}
	}
}
