using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menus : MonoBehaviour
{
    public AudioClip clickClip;
    public Playermotor playerController;
    public GameObject pauseMenu;
    public Animator achievementAnim;
    public Animator gameOverAnim;
    public Text currentCoin, allCoin, currentHeart, allHeart, currentScore, highScore, currentMultipier, highMultipier;
    public static Menus instance;
    void Start()
    {
        MakeInstance();
    }

    public void MakeInstance()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public void PauseGame()
    {
        if (!playerController.isDead)
        {
            AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
           // LevelManager.instance.isPaused = true;
        }

    }

    public void ResumeGame()
    {
        AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
      //  LevelManager.instance.isPaused = false;
    }

    public void RestartGame()
    {
        AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
    }

    public void QuitGame()
    {
        AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);
        SceneManager.LoadScene(0);
    }

    public void ShowAchievement()
    {
        gameOverAnim.SetTrigger("Hide");

        //GetData
        currentCoin.text = GameManager.instance.coinScore.ToString();
        currentHeart.text = GameManager.instance.heartCount.ToString();
        currentScore.text = GameManager.instance.score.ToString();
        currentMultipier.text = GameManager.instance.modifierScore.ToString();

        allCoin.text = PlayerPrefs.GetInt("CollectedCoins").ToString();
        allHeart.text = PlayerPrefs.GetInt("CollectedHeart").ToString();
        highScore.text = PlayerPrefs.GetInt("HighestScore").ToString();
        highMultipier.text = PlayerPrefs.GetInt("HighestMultipier").ToString();

        achievementAnim.SetTrigger("Show");
    }

    public void AudioOnOff()
    {
        if (GameManager.instance.GetComponent<AudioSource>().volume == 0)
        {
            GameManager.instance.GetComponent<AudioSource>().volume = 1;
        }
        else
        {
            GameManager.instance.GetComponent<AudioSource>().volume = 0;
        }
        AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);
    }

    public void StartGame()
    {
        AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);
        SceneManager.LoadScene(1);
    }

    public void MainMenu()
    {
        AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);
        SceneManager.LoadScene(0);
    }
}
