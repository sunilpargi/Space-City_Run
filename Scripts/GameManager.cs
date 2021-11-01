using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int coinScoreAmount = 5;
    public static GameManager instance { get; set; }

    private bool isGameStarted;
    private Playermotor playerMotor;

    //UI and UI fields
    public Text scoreText, coinText, modifierText, highScoreText;
    public float score, coinScore, modifierScore, heartCount;
    private int lastScore;

    //Death menu
    public Animator deathMenuAnim;
    public Text deadcoinText, deadscoreText;


    public GameObject deathMenu;
    public Animator gameCanvas, menuAnim, diamondAnim;

    public AudioClip clickClip;
    public bool IsDead { set; get; }
    void Awake()
    {
        instance = this;
        modifierScore = 1;
        playerMotor = GameObject.FindGameObjectWithTag("Player").GetComponent<Playermotor>();

        modifierText.text = "x" + modifierScore.ToString("0.0");// two digit
        scoreText.text = "Score:"+score.ToString("0"); // single digit
        coinText.text = coinScore.ToString("0");

        highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
        //if (!isGameStarted)
        //{
        //    AdManager.instance.RequestBanner();
        //}
    }

    private void Update()
    {
       
        if (MobileScript.instance.Tap && !isGameStarted)
        {
            AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);
            // AdManager.instance.HideBanner();
            isGameStarted = true;
            playerMotor.StartRunning();
          //  FindObjectOfType<GlacierSpawner>().IsScrolling = true;
            FindObjectOfType<CameraMotor>().IsMoving = true;
          //  gameCanvas.SetTrigger("Show");
          //  menuAnim.SetTrigger("Hide");
        }
      

        if (isGameStarted && !IsDead)
        {
            score += Time.deltaTime * modifierScore;

            if (lastScore != (int)score)
            {
                lastScore = (int)score;
                scoreText.text = "Score:"+score.ToString("0");

            }           
        }
    }

   
    public void GetCoin()
    {
        diamondAnim.SetTrigger("Collect");
        coinScore++;
        coinText.text = coinScore.ToString("0");
        score += coinScoreAmount;
        scoreText.text = "Score:"+score.ToString("0"); // single digit
    }
  

    public void UpdateModifier(float modifierAmount)
    {
        modifierScore = 1f + modifierAmount;
        modifierText.text = "x" + modifierScore.ToString("0.0");
    }

    public void PlayButton()
    {
        AudioSource.PlayClipAtPoint(clickClip, Camera.main.transform.position);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnDeath()
    {
        AdManager.instance.ShowFullScreenAd();
        IsDead = true;
     //   FindObjectOfType<GlacierSpawner>().IsScrolling = false;
        deadcoinText.text = coinScore.ToString("0");
        deadscoreText.text = score.ToString("0");
        deathMenu.SetActive(true);
        deathMenuAnim.SetTrigger("Show");
    
      //  gameCanvas.SetTrigger("Hide");

        //check if it is a highScore
        if(score > PlayerPrefs.GetInt("HighScore"))
        {
            float s = score;
            if(s % 1 == 0)
            {
                s++;
            }
            PlayerPrefs.SetInt("HighScore", (int)s);
        }
    }
}
