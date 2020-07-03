using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MultiplayerManager : NetworkBehaviour
{
    public static MultiplayerManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void StartGame()
    {
        if (isServer)
        {
            CmdStartGame();
        }
    }

    private List<PlayerConnection> GetCurrentPlayers()
    {
        PlayerConnection[] foundPlayers = FindObjectsOfType<PlayerConnection>();
        return new List<PlayerConnection>(foundPlayers);
    }

    private void SpawnPlayers(List<PlayerConnection> players)
    {
        foreach(PlayerConnection player  in players)
        {
            player.SpawnPlayer();
        }
    }

    private void InitPlay()
    {
        List<PlayerConnection> currentPlayers = GetCurrentPlayers();
        SpawnPlayers(currentPlayers);
        UIManager.Instance.DisableMenu();
        GameManagerMultiplayer.instance.StartGameMultiplayer();
    }

    [Command]
    private void CmdStartGame()
    {
        RpcStartGame();
    }
    [ClientRpc]
    private void RpcStartGame()
    {
        InitPlay();
    }
}
