using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "IA" || collision.gameObject.tag == "IA2")
        {
            FSM_IA ia = collision.gameObject.GetComponent<FSM_IA>();
            if (ia._grounded && ia.canJump) ia.Jump();
        }
    }
}
