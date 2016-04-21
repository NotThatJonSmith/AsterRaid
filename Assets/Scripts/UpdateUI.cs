﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UpdateUI : MonoBehaviour {

    public GameObject Base1;
    public GameObject Base2;
	public Slider Base1HP;
	public Slider Base2HP;

	public int maxCountdown;
	int currCountdown;
	float loadLevelTime;

    // Use this for initialization
    void Start () {

		currCountdown = maxCountdown;
		loadLevelTime = Time.unscaledTime;

		Base1HP.maxValue = Base1.GetComponent<BaseHealth> ().maxHealth;
		Base2HP.maxValue = Base2.GetComponent<BaseHealth> ().maxHealth;

	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.GM != null)
        {
            if (GameManager.GM.currstate == GameManager.State.Countdown)
            {
                maxCountdown = GameManager.GM.gameStartCountdown;
                countdown();
            }

            // Show WIN UI element
            if (GameManager.GM.base_team1.GetComponent<BaseHealth>().isDestroyed)
            {
                displayWin(2);
            }
            else if (GameManager.GM. base_team2.GetComponent<BaseHealth>().isDestroyed)
            {
                displayWin(1);
            }

            if(GameManager.GM.currstate == GameManager.State.EndGame)
            {
                transform.Find("RestartText").gameObject.SetActive(true);
            }
        }

		Base1HP.value = Base1.GetComponent<BaseHealth> ().currHealth;
		Base2HP.value = Base2.GetComponent<BaseHealth> ().currHealth;

		if (currCountdown > 0) {
			countdown ();
		}
    }

	public void displayWin(int winningTeam) {
		GameObject wintext = transform.Find ("WinText").gameObject;
		wintext.SetActive (true);

		if (winningTeam == 1) {
			wintext.GetComponent<Text> ().color = Color.blue;
		} else if (winningTeam == 2) {
			wintext.GetComponent<Text> ().color = Color.red;
		}

		wintext.GetComponent<Text>().text = "Team " + winningTeam + " WINS";
	}

	public void countdown() {
		currCountdown = (int)Mathf.Ceil (maxCountdown - (Time.unscaledTime - loadLevelTime));

        transform.Find("CountdownText").gameObject.SetActive(true);
        transform.Find ("CountdownText").gameObject.GetComponent<Text> ().text = currCountdown.ToString();

		if (currCountdown <= 0) {
            GameManager.GM.StartTheGame();
			transform.Find ("CountdownText").gameObject.SetActive (false);
		}
	}
}
