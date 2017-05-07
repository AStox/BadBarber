﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public BezierCurve insideCurve;
	public BezierCurve outsideCurve;
	public float resolution;
	public static int score;
	Text scoreText;
	public GameObject currentPatron;
	GameObject newPatron;
	public GameObject[] patrons;

	void Start () {
		scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
	}

	void Update () {
		score = 0;
		CheckHaircut ();
	}

	void CheckHaircut () {
		float length = 100;
		RaycastHit hit;
		for (int i = 0; i < length; i++) {
			if (Physics.Raycast(insideCurve.GetPointAt(i/length), new Vector3(0f,0f,1f), out hit)) {
				score += 1;
			}
			if (Physics.Raycast(outsideCurve.GetPointAt(i/length), new Vector3(0f,0f,1f), out hit)) {
				score -= 1;
			}
		}
		scoreText.text = score.ToString() + "%";
	}

	public void NewPatron () {
		if (currentPatron) {
			currentPatron.GetComponent<Animator>().Play("Exit");
			StartCoroutine(WaitForAnimation());
		}

	}

	IEnumerator WaitForAnimation () {
		yield return new WaitForSeconds(0.5f);
		Destroy(currentPatron);
		newPatron = Instantiate(patrons[(int)UnityEngine.Random.Range(0, patrons.Length)], new Vector3(-0.25f,-0.5f, 0.87f), Quaternion.identity);
		newPatron.GetComponent<Animator>().Play("Enter");
		currentPatron = newPatron;
		GameObject.Find("cutter").GetComponent<Cutter>().meshObj = GameObject.Find("Sphere_007");
		GameObject.Find("cutter").GetComponent<Cutter>().Start();
	}
}
