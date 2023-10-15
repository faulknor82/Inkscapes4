using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    public GameObject startScreenPanel;
    public GameObject creditsPanel;

    public void PlayButton()
    {
        SceneManager.LoadScene(1);
    }

    public void CreditsButton()
    {
        startScreenPanel.SetActive(false);
        creditsPanel.SetActive(true);
    }

    public void GoBackButton()
    {
        startScreenPanel.SetActive(true);
        creditsPanel.SetActive(false);
    }
}
