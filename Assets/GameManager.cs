using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	BezierCurve insideCurve;
	BezierCurve outsideCurve;
	public float resolution;
	public static int score;
	List<int> scoreList;
	Text scoreText;
	Text timerText;
	GameObject currentPatron;
	GameObject newPatron;
	public GameObject[] patrons;
	bool switching;
	int prevPatron;
	int averageScore;
	float timer;
	string barberRating;
	public GameObject endScreen;
	public GameObject startScreen;
	int numHaircuts;
	bool first;
	bool end;

	void Start () {
		scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
		timerText = GameObject.Find("TimerText").GetComponent<Text>();
		Restart();
	}

	void Restart () {
		endScreen.SetActive(false);
		switching = true;
		timer = 30f;
		numHaircuts = 0;
		first = true;
		scoreList = new List<int>();
		end = false;
	}

	void Update () {
		score = 0;
		if (!switching) {
			CheckHaircut ();
			timer -= Time.deltaTime;
		}
		if (timer <= 0) {
			EndScreen();
		}
		timerText.text = "Time: " + timer.ToString("F0") + "s";
	}

	void CheckHaircut () {
		insideCurve = currentPatron.GetComponentsInChildren<BezierCurve>()[0];
		outsideCurve = currentPatron.GetComponentsInChildren<BezierCurve>()[1];
		float length = 100;
		int layerMask = 1 << 8;
		RaycastHit hit;
		for (int i = 0; i < length; i++) {
			if (Physics.Raycast(insideCurve.GetPointAt(i/length), new Vector3(0f,0f,1f), out hit, Mathf.Infinity, layerMask)) {
				score += 1;
			}
			if (Physics.Raycast(outsideCurve.GetPointAt(i/length), new Vector3(0f,0f,1f), out hit, Mathf.Infinity, layerMask)) {
				score -= 1;
			}
		}
		scoreText.text = score.ToString() + "%";
	}

	public void NewPatron () {
		if (end) {
			Restart();
		}
		startScreen.SetActive(false);
		if (!first) {
			scoreList.Add(score);
			endScreen.SetActive(false);
		}
		first = false;
		numHaircuts += 1;
		GameObject.Find("NextText").GetComponent<Text>().text = "NEXT!";
		switching = true;
		if (currentPatron) {
			currentPatron.GetComponent<Animator>().Play("Exit");
			currentPatron.GetComponentInChildren<LineRenderer>().enabled = false;
		}
		StartCoroutine(WaitForAnimation());
	}

	IEnumerator WaitForAnimation () {
		if (currentPatron) {
			yield return new WaitForSeconds(0.3f);
			Destroy(currentPatron);
		}

		int patronIndex = (int)UnityEngine.Random.Range(0, patrons.Length);
		while (patronIndex == prevPatron) {
			patronIndex = (int)UnityEngine.Random.Range(0, patrons.Length);
		}
		prevPatron = patronIndex;
		newPatron = Instantiate(patrons[patronIndex], new Vector3(-0.25f,-0.5f, 0.87f), Quaternion.identity);
		newPatron.GetComponent<Animator>().Play("Enter");
		currentPatron = newPatron;
		switching = false;
		yield return new WaitForSeconds(0.3f);
		currentPatron.GetComponentInChildren<GuideCurve>().DrawLine();
	}

	void EndScreen () {
		switching = true;
		endScreen.SetActive(true);
		GameObject.Find("NextText").GetComponent<Text>().text = "START!";
		GameObject.Find("AverageText").GetComponent<Text>().text = "Average: " + AverageScore().ToString("F0") + "%";
		GameObject.Find("RatingText").GetComponent<Text>().text = "Barber Rating: " + BarberRating();
		GameObject.Find("HaircutsText").GetComponent<Text>().text = "Haircuts: " + numHaircuts.ToString("F0");
		end = true;
	}

	int AverageScore() {
		int sum = 0;
		for (int i = 0; i < scoreList.Count; i++) {
			sum += scoreList[i];
		}
		return (int)(sum/scoreList.Count);
	}

	string BarberRating () {
		var averageScore = AverageScore();
		if (numHaircuts >= 3) {
			if (averageScore < 50) {
				return "BAD";
			} else {
				return "PRETTY GOOD";
			}
		} else if (numHaircuts >= 5) {
			if (averageScore < 50) {
				return "BAD";
			} else if (averageScore < 60) {
				return "PRETTY GOOD";
			} else if (averageScore < 70) {
				return "GOOD";
			} else if (averageScore < 80) {
				return "PRETTY GOOD";
			} else {
				return "VERY GOOD";
			}
		} else if (numHaircuts >= 7) {
			if (averageScore < 50) {
				return "BAD";
			} else if (averageScore < 60) {
				return "PRETTY GOOD";
			} else if (averageScore < 70) {
				return "GOOD";
			} else if (averageScore < 80) {
				return "PRETTY GOOD";
			} else if (averageScore < 90) {
				return "VERY GOOD";
			} else {
				return "EXCELLENT!";
			}
		} else {
			return "REALLY BAD...";
		}
	}
}
