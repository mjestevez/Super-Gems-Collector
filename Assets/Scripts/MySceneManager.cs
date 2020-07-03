using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager instance = null;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex > 4)
        {
            Time.timeScale = 1;
            Invoke("BackMenu", 4.5f);
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void HvsH()
    {
        SceneManager.LoadScene("HvsH");
    }
    public void HvsHMulti()
    {
        SceneManager.LoadScene("HvsH(Multiplayer)");
    }
    public void HvsIA()
    {
        SceneManager.LoadScene("HvsIA");
    }
    public void IAvsIA()
    {
        SceneManager.LoadScene("IAvsIA");
    }
    

    public void LoadPlayer1Win()
    {
        SceneManager.LoadScene("Player1Win");
    }
    public void LoadPlayer2Win()
    {
        SceneManager.LoadScene("Player2Win");
    }
    public void LoadDraw()
    {
        SceneManager.LoadScene("Draw");
    }

    public void BackMenu()
    {
        SceneManager.LoadScene(0);
    }
}
