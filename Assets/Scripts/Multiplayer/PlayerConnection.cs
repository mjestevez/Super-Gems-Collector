using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerConnection : NetworkBehaviour
{
    public CharacterControllerMultiplayer player;

    public void SpawnPlayer()
    {
        if (isLocalPlayer)
        {
            CmdSpawnPlayer();
        }
    }

    [Command]
    void CmdSpawnPlayer()
    {
        CharacterControllerMultiplayer character = Instantiate(player,transform.position,transform.rotation);
        if (!hasAuthority)
        {
            character.tag = "Player 2";
        }

        NetworkServer.SpawnWithClientAuthority(character.gameObject, connectionToClient);
    }
}
