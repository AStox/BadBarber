using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour {

	float depth;
	// public GameObject meshObj;
	Mesh mesh;
	Collider collider;
	bool cutting;
	Vector3[] verts;
	Vector3 center;
	public Animator anim;

	public void Start () {
		cutting = false;
		collider = GetComponent<MeshCollider>();
		depth = Camera.main.transform.position.z;
	}

	void Update () {
		Movement();
		if (cutting) {
			foreach (GameObject hair in GameObject.FindGameObjectsWithTag("Hair")) {
				mesh = hair.GetComponent<MeshFilter>().mesh;
				center = mesh.bounds.center;
				verts = mesh.vertices;
				for(int i = 0; i < verts.Length; i++) {
					RaycastHit hit;
					if (Physics.Raycast(center, hair.transform.TransformPoint(verts[i]) - center, out hit, Vector3.Distance(center, hair.transform.TransformPoint(verts[i])))) {
						verts[i] = hair.transform.InverseTransformPoint(hit.point);
					}
				}
				mesh.vertices = verts;
				// mesh.RecalculateBounds();
			}
		}

		if (Input.GetMouseButtonDown(0)) {
			cutting = true;
		}
		if (Input.GetMouseButtonUp(0)) {
			cutting = false;
			float val = Random.value;
			// if (val > 0.75f) {
			// 	anim.Play("Mouth", 1, 0f);
			// }
		}
	}

	void Movement () {
		Vector3 mousePos = Input.mousePosition;
    Vector3 wantedPos = Camera.main.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, depth));
    transform.position = wantedPos;
	}



	// public void OnDrawGizmos () {
	// 	for(int i = 0; i < verts.Length; i++) {
	// 		Gizmos.DrawSphere(meshObj.transform.TransformPoint(verts[i]), 0.1f);
	// 	}
	// }
}
