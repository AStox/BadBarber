using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour {

	float timer;
	Animator anim;
	MeshCollider collider;
	public GameObject meshObj;
	Mesh mesh;

	void Start () {
		timer = 0;
		anim = GetComponent<Animator>();
		collider = meshObj.GetComponent<MeshCollider>();
		mesh = meshObj.GetComponent<MeshFilter>().mesh;
		GameObject.Find("cutter").GetComponent<Cutter>().meshObj = meshObj;
	}

	void Update () {
		float val = Random.value;
		if (timer > 2 && val > 0.99f) {
			// anim.playbackTime = 0;
			// anim.StartPlayback();
			anim.Play("Blink", -1, 0f);
			timer = 0;
		}
		timer += Time.deltaTime;
		RecalculateCollider();
	}

	void RecalculateCollider () {
		collider.sharedMesh = mesh;
	}
}
