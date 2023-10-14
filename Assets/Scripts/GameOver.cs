using UnityEngine;
using TMPro;

public class GameOver : MonoBehaviour
{
    private LeftBoard leftBoard;
    private RightBoard rightBoard;
    public GameObject gameOverPanel;
    public GameObject leftBoard2;
    public GameObject rightBoard2;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        leftBoard = leftBoard2.GetComponent<LeftBoard>();
        rightBoard = rightBoard2.GetComponent<RightBoard>();
    }

    // Update is called once per frame
    void Update()
    {
        if(leftBoard.IsLeftBoardGameOver2 + rightBoard.IsRightBoardGameOver2 == 2)
        {
            // Both Boards have lost - Game is over
            gameOverPanel.SetActive(true);
            leftBoard.leftBoardPanel.SetActive(false);
            rightBoard.rightBoardPanel.SetActive(false);
        }

        scoreText.text = leftBoard.totalScore.ToString();
    }
}
