using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class UIManager : NetworkBehaviour
{
    public static UIManager Instance;

    public Text playersInfo;
    public Text buttonPlayText;
    public Button buttonStart;
    public GameObject canvasMultiplayer;

    [SyncVar]
    public int amountOfPlayers = 0;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (isServer)
        {
            if(amountOfPlayers != NetworkManager.singleton.numPlayers)
            {
                amountOfPlayers = NetworkManager.singleton.numPlayers;
            }
            if (amountOfPlayers >= 2)
                buttonStart.interactable=true;

        }
        else
        {
            buttonPlayText.text = "Esperando Contrincante";
        }
    }

    private void OnEnable()
    {
        StartCoroutine(UpdateCoroutine());
    }

    private IEnumerator UpdateCoroutine()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(1f);
            UpdatePlayersCount();
        }
    }

    public void UpdatePlayersCount()
    {
        if (amountOfPlayers < 2)
        {
            playersInfo.text = "Buscando Oponente";
        }
        else
        {
            playersInfo.text = "Oponente Encontrado";
        }
    }

    public void EnableMenu()
    {
        Debug.Log("Hola");
        canvasMultiplayer.SetActive(true);
    }

    public void DisableMenu()
    {
        
        canvasMultiplayer.SetActive(false);
        Debug.Log("Adios");
        
    }

}
