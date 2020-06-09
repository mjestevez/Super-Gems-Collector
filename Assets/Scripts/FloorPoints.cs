using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPoints : MonoBehaviour
{
    public Floor floor;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "IA" || collision.gameObject.tag == "IA2")
        {
            FSM_IA ia = collision.gameObject.GetComponent<FSM_IA>();
            ia.floor = floor;
        }
    }
}
