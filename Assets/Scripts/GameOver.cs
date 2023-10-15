using UnityEngine;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private LeftBoard leftBoard;
    private RightBoard rightBoard;
    public GameObject gameOverPanel;
    public GameObject leftBoard2;
    public GameObject rightBoard2;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI thisLevelScoreText;
    public TextMeshProUGUI goalText;
    public TextMeshProUGUI levelText;
    private SceneMan sceneMan;
    public GameObject sceneMana;
    public int winCondition;
    public GameObject beginNextLevelButton;
    // public int scoreOnRetry;

    private void Start()
    {
        sceneMan = sceneMana.GetComponent<SceneMan>();
        leftBoard = leftBoard2.GetComponent<LeftBoard>();
        rightBoard = rightBoard2.GetComponent<RightBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        if(leftBoard.thisLevelScore >= winCondition)
        {
            PlayerPrefs.SetInt("Total Score", leftBoard.totalScore);
            // You Win
            beginNextLevelButton.SetActive(true);
        }

        if(leftBoard.IsLeftBoardGameOver2 + rightBoard.IsRightBoardGameOver2 == 2)
        {
            // Both Boards have lost - Game is over
            gameOverPanel.SetActive(true);
            leftBoard.leftBoardPanel.SetActive(false);
            rightBoard.rightBoardPanel.SetActive(false);
            // leftBoard.IsLeftBoardGameOver2 = 0;
            // rightBoard.IsRightBoardGameOver2 = 0;
            // PlayerPrefs.DeleteAll();
        }

        Debug.Log(leftBoard.IsLeftBoardGameOver2);
        Debug.Log(rightBoard.IsRightBoardGameOver2);

        scoreText.text = leftBoard.totalScore.ToString("n0");
        thisLevelScoreText.text = leftBoard.thisLevelScore.ToString("n0");
        goalText.text = "Goal\n" + winCondition.ToString("n0");
        levelText.text = "Level " + sceneMan.sceneIndex;
    }

    IEnumerator sceneTransition()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneMan.nextScene);
    }

    IEnumerator retryScene()
    {
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(sceneMan.sceneIndex);
    }

    public void BeginNextLevel()
    {
        StartCoroutine(sceneTransition());
    }

    public void Retry()
    {
        StartCoroutine(retryScene());
    }
}
