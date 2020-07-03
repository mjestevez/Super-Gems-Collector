using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerMultiplayer : MonoBehaviour
{
    public static GameManagerMultiplayer instance = null;

    private int player1Value = 0;
    private int player2Value = 0;
    public Text player1ValueText;
    public Text player2ValueText;
    private float gameTime = 60f;
    public Text gameTimeText;
    public bool start = false;
    public Text startTimer;
    public AudioClip[] audios;
    private AudioSource audioSr;
    public AudioSource musica;
    public GameObject gameplayCanvas;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {

        audioSr = GetComponent<AudioSource>();

    }

    public void StartGameMultiplayer()
    {
        gameplayCanvas.SetActive(true);
        StartCoroutine(StartGame());
    }
    private void Update()
    {
        if (start)
        {
            gameTime -= Time.deltaTime;
            gameTimeText.text = gameTime.ToString("0");

            if (gameTime <= 0)
            {
                StartCoroutine(FinishGame());
                Time.timeScale = 0;
                audioSr.clip = audios[4];
                audioSr.Play();
                musica.Stop();
                startTimer.text = "Finish!";
                startTimer.enabled = true;
                start = false;
            }
        }

    }
    public void AddPlayer1Value(int value)
    {
        player1Value += value;
        player1ValueText.text = "x" + player1Value.ToString("00");
    }
    public void AddPlayer2Value(int value)
    {
        player2Value += value;
        player2ValueText.text = "x" + player2Value.ToString("00");
    }

    IEnumerator StartGame()
    {
        audioSr.clip = audios[0];
        audioSr.Play();
        startTimer.text = "3";
        yield return new WaitForSecondsRealtime(1f);
        audioSr.clip = audios[1];
        audioSr.Play();
        startTimer.text = "2";
        yield return new WaitForSecondsRealtime(1f);
        audioSr.clip = audios[2];
        audioSr.Play();
        startTimer.text = "1";
        yield return new WaitForSecondsRealtime(1f);
        audioSr.clip = audios[3];
        audioSr.Play();
        startTimer.text = "GO!";
        musica.Play();
        yield return new WaitForSecondsRealtime(1f);

        startTimer.enabled = false;
        start = true;
        Time.timeScale = 1;
    }

    IEnumerator FinishGame()
    {

        yield return new WaitForSecondsRealtime(2f);
        if (player1Value == player2Value) MySceneManager.instance.LoadDraw();
        else if (player1Value > player2Value) MySceneManager.instance.LoadPlayer1Win();
        else MySceneManager.instance.LoadPlayer2Win();
    }
    
}
