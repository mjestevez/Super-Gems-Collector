using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Type
{
    Player1,
    Player2,
    IA1,
    IA2
}

public class Platform : MonoBehaviour
{
    public Type typePlayer;
    private PlatformEffector2D effector;
    private string player1Vertical="Vertical";
    private string player2Vertical="Vertical2";
    private FSM_IA IAPlayer1=null;
    private FSM_IA IAPlayer2 = null;

    // Start is called before the first frame update
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
        if (GameObject.FindGameObjectWithTag("IA"))
            IAPlayer1 = GameObject.FindGameObjectWithTag("IA").GetComponent<FSM_IA>();
        if (GameObject.FindGameObjectWithTag("IA2"))
            IAPlayer2 = GameObject.FindGameObjectWithTag("IA2").GetComponent<FSM_IA>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(typePlayer == Type.Player1){
            if (Input.GetAxisRaw(player1Vertical) < 0)
            {
                effector.rotationalOffset = 180;
            }
            if (Input.GetAxisRaw(player1Vertical) >= 0f)
            {
                effector.rotationalOffset = 0;
            }
        }
        else if (typePlayer == Type.Player2)
        {
            if (Input.GetAxisRaw(player2Vertical) < 0)
            {
                effector.rotationalOffset = 180;
            }
            if (Input.GetAxisRaw(player2Vertical) >= 0f)
            {
                effector.rotationalOffset = 0;
            }
        }
        else if(typePlayer == Type.IA1 && IAPlayer1)
        {
            if (IAPlayer1.goDown)
            {
                effector.rotationalOffset = 180;
            }
            else
            {
                effector.rotationalOffset = 0;
            }
        }else if (typePlayer == Type.IA2 && IAPlayer2)
        {
            if (IAPlayer2.goDown)
            {
                effector.rotationalOffset = 180;
            }
            else
            {
                effector.rotationalOffset = 0;
            }
        }
        

    }
}
