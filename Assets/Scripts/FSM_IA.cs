using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    Looking,
    Moving,
    Climbing
}

public enum Floor
{
    P0,
    P1,
    P2,
    P3
}

public class FSM_IA : MonoBehaviour
{
    public Floor floor;
    public State currentState;
    public List<Gem> gems;
    public Gem focusGem=null;
    public bool _facingRight = true;
    public float maxSpeed = 15f;
    public bool canJump = true;
    public float jumpForce = 1700f;
    public bool _grounded;
    public Transform groundCheck;
    private float groundRadious = 0.2f;
    public LayerMask whatIsGround;
    public bool goDown = false;
    public List<Vector2> climbPoints;
    public Vector2 focusClimb;
    public bool climbing = false;
    public bool canMove = true;
    private float timer = 3f;
    private float counter = 0;

    private Animator _anim;
    private AudioSource audioSr;

    private void Start()
    {
        audioSr = GetComponentInChildren<AudioSource>();
        _anim = GetComponentInChildren<Animator>();
        if (!_facingRight)
        {
            Flip();
            _facingRight = !_facingRight;
        }
    }

    private void Update()
    {
        if(canJump && _grounded) _anim.SetBool("Jump", false);
        switch (currentState)
        {
            case State.Looking:
                focusGem=LookingBestGem();
                if(focusGem) currentState = State.Moving;
                else currentState = State.Climbing;
                break;
            case State.Moving:
                if(canMove)Move();
                if (CheckFinalMove()) currentState = State.Looking;
                break;
            case State.Climbing:
                focusClimb = LookingClimbPoint();
                if(canMove) Climb();
                if (CheckFinalClimb()) currentState = State.Looking;
                break;
        }
    }

    #region Looking
        private Gem LookingBestGem()
        {
            int n = CheckPossibleGems();
            List<Gem> selectedGems = new List<Gem>();
            Gem g = null;
            int value = 0;
            for (int i = 0; i < n; i++)
            {
                if (!gems[i].cooldown && gems[i].value >= value)
                {
                    selectedGems.Add(gems[i]);
                    value = gems[i].value;
                }
            }
            if (selectedGems.Count > 0) g = selectedGems[UnityEngine.Random.Range(0, selectedGems.Count)];
            return g;
        }

        private int CheckPossibleGems()
        {
            float h = transform.position.y;
            int n = 0;
            switch (floor)
            {
                case Floor.P0:
                    n = 1;
                    break;
                case Floor.P1:
                    n = 3;
                    break;
                case Floor.P2:
                    n = 4;
                    break;
                case Floor.P3:
                    n = 6;
                    break;
            }
            return n;
        }
    #endregion

    #region Moving
    private void Move()
        {

            if (focusGem.transform.position.y - transform.position.y < 3)
            {
                if (Mathf.Abs(focusGem.transform.position.x - transform.position.x) < 0.1f && floor == Floor.P0 && _grounded && canJump)
                {
                    canMove = false;
                    Jump();
                }
                float move = 0;
                _grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadious, whatIsGround);

                if (focusGem.transform.position.x < transform.position.x) move = -1;
                else if (focusGem.transform.position.x > transform.position.x) move = 1;
                else move = 0;

                if (focusGem.transform.position.y < transform.position.y) goDown = true;
                else goDown = false;


                int direction = _facingRight ? 1 : -1;
                transform.Translate(move * maxSpeed * direction * Time.deltaTime, 0, 0);
                if ((move > 0 && !_facingRight) || (move < 0 && _facingRight))
                {
                    Flip();
                }
                if (_anim != null)
                {
                    _anim.SetFloat("Speed", Mathf.Abs(move));
                }
            }
            else
            {
                focusGem = null;
            }

        }
        private bool CheckFinalMove()
        {
            if (!focusGem || focusGem.cooldown) return true;
            else if (Vector2.Distance(transform.position, focusGem.transform.position) < 0.5f) return true;
            else if (counter >= timer)
            {
                counter = 0;
                return true;
            }
            else
            {
                counter += Time.deltaTime;
                return false;
            }
        }
    #endregion

    #region Climbing
    private bool CheckFinalClimb()
    {
        if (Vector2.Distance(transform.position, focusClimb) < 0.5f)
        {
            climbing = false;
            return true;
        }
        else return false;
    }

    private void Climb()
    {
        if (Mathf.Abs(focusClimb.x - transform.position.x) < 0.1f && floor == Floor.P0 && _grounded && canJump)
        {
            canMove = false;
            Jump();
        }
        float move = 0;
        _grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadious, whatIsGround);

        if (focusClimb.x < transform.position.x) move = -1;
        else if (focusClimb.x > transform.position.x) move = 1;
        else move = 0;

        if (focusClimb.y < transform.position.y) goDown = true;
        else goDown = false;

        int direction = _facingRight ? 1 : -1;
        transform.Translate(move * maxSpeed * direction * Time.deltaTime, 0, 0);
        if ((move > 0 && !_facingRight) || (move < 0 && _facingRight))
        {
            Flip();
        }
    }

    private Vector2 LookingClimbPoint()
    {

        int n = -1;
        switch (floor)
        {
            case Floor.P0:
                n = 0;
                break;
            case Floor.P1:
                n = 1;
                break;
            case Floor.P2:
                n = 2;
                break;
            case Floor.P3:
                n = 2;
                break;
        }
        climbing = true;
        return climbPoints[n];
        
    }

    #endregion


    public void Jump()
    {
        _grounded = false;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
        if(canMove)
            Invoke(nameof(JumpAnimator), 0.4f);
        else
            Invoke(nameof(JumpAnimator), 0.8f);
        canJump = false;
        _anim.SetBool("Jump", true);
        audioSr.Play();

    }

    private void JumpAnimator()
    {
        canMove = true;
        canJump = true;
    }
    private void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(new Vector3(0, 180, 0), Space.World);
    }

    
}
