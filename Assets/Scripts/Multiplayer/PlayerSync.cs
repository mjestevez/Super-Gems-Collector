using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSync : NetworkBehaviour
{
    public float smoothFactor = 10f;
    public float threshold = 0.1f;

    private Vector3 m_LastPosition;

    private CharacterControllerMultiplayer m_Player;

    private void Awake()
    {
        
    }
}
