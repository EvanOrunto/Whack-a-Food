using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerX : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private Button restartButton;

    [SerializeField] private List<GameObject> targetPrefabs;

    private int score;
    private float timeRemaining;
    private float spawnRate = 1.5f;
    private bool isGameActive;

    private float spaceBetweenSquares = 2.5f;
    private float minValueX = -3.75f; //  x value of the center of the left-most square
    private float minValueY = -3.75f; //  y value of the center of the bottom-most square

    // Start the game, remove title screen, reset score, and adjust spawnRate based on difficulty button clicked


    public void StartGame(int difficulty) {
        spawnRate /= difficulty;
        isGameActive = true;
        score = 0;
        timeRemaining = 60;

        StartCoroutine(SpawnTarget());
        UpdateScore(0);
        titleScreen.SetActive(false);
    }
    private void Update() {
        TimerCountDown();
    }

    // While game is active spawn a random target
    IEnumerator SpawnTarget() {
        while (isGameActive) {
            yield return new WaitForSeconds(spawnRate);
            int index = Random.Range(0, targetPrefabs.Count);

            if (isGameActive) {
                Instantiate(targetPrefabs[index], RandomSpawnPosition(), targetPrefabs[index].transform.rotation);
            }

        }
    }

    // Generate a random spawn position based on a random index from 0 to 3
    Vector3 RandomSpawnPosition() {
        float spawnPosX = minValueX + (RandomSquareIndex() * spaceBetweenSquares);
        float spawnPosY = minValueY + (RandomSquareIndex() * spaceBetweenSquares);

        Vector3 spawnPosition = new Vector3(spawnPosX, spawnPosY, 0);
        return spawnPosition;

    }

    // Generates random square index from 0 to 3, which determines which square the target will appear in
    int RandomSquareIndex() {
        return Random.Range(0, 4);
    }

    // Update score with value from target clicked
    public void UpdateScore(int scoreToAdd) {
        score += scoreToAdd;
        scoreText.text = "score: " + score;
    }

    private void UpdateTimerText() {
        int time = Mathf.RoundToInt(timeRemaining);
        timerText.text = "Time: " + time;
    }

    private void TimerCountDown() {
        if (timeRemaining > 0 && isGameActive) {
            timeRemaining -= Time.deltaTime;
            UpdateTimerText();
        } else if (timeRemaining < 0) {
            timeRemaining = 0;
            UpdateTimerText();
            GameOver();
        }
    }

    // Stop game, bring up game over text and restart button
    public void GameOver() {
        gameOverText.gameObject.SetActive(true);
        restartButton.gameObject.SetActive(true);
        isGameActive = false;
    }

    // Restart game by reloading the scene
    public void RestartGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public bool IsGameActive() {
        return isGameActive;
    }

}
