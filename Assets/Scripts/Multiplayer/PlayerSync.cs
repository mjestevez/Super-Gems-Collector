using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSync : NetworkBehaviour
{
    public float smoothFactor = 10f;
    public float threshold = 0.1f;
    public bool gameStart;

    private Vector3 m_LastPosition;
    private bool m_facingRight;
    private bool l_moving, l_jumping;

    private CharacterControllerMultiplayer m_Player;

    private void Awake()
    {
        m_Player = GetComponent<CharacterControllerMultiplayer>();
    }

    private void Start()
    {
        gameStart = GameManagerMultiplayer.instance.start;
    }

    private void Update()
    {
        SyncPlayer();
        if(!gameStart)
            gameStart = GameManagerMultiplayer.instance.start;
    }

    private void SyncPlayer()
    {
        if (m_Player.HasAuthority())
            SendData();
        else
            SyncOtherPlayer();
    }

    private void SyncOtherPlayer()
    {
        if(gameStart)
            m_Player.LerpPosition(m_LastPosition, smoothFactor);
        m_Player.IsLookingRight(m_facingRight);
        m_Player.SetAnimator(l_jumping, l_moving);
    }
    private void SendData()
    {
        bool moving = true;
        bool jumping = false;

        if (m_Player.move == 0)
            moving = false;
        if (!m_Player._grounded)
            jumping = true;

        CmdSendData(m_Player.transform.position, m_Player._facingRight, moving, jumping);

    }

    [Command]
    private void CmdSendData(Vector3 position, bool facingRight, bool moving, bool jumping)
    {
        m_LastPosition = position;
        l_moving = moving;
        l_jumping = jumping;
        m_facingRight = facingRight;

        RpcReceiveData(m_LastPosition, m_facingRight, l_moving, l_jumping);
    }
    [ClientRpc]
    private void RpcReceiveData(Vector3 position, bool facingRight, bool moving, bool jumping)
    {
        if(!m_Player.HasAuthority())
        {
            m_LastPosition = position;
            l_moving = moving;
            l_jumping = jumping;
            m_facingRight = facingRight;
        }
    }
}
