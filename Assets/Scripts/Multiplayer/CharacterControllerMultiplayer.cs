using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;

public class CharacterControllerMultiplayer : NetworkBehaviour
{
    [Header("Movement")]
    public bool canMove = true;
    public float maxSpeed = 15f;
    public bool lookToMouse;
    public string movementAxis;
    public bool _facingRight = true;
    public float move = 0;

    [Header("Jump")]
    public bool CanJump;
    public string jumpAxis;
    private bool _jumpKeyPressed;
    public float jumpForce = 1700f;
    public bool hasDoubleJump;
    private bool _doubleJump;
    public bool _grounded;
    public Transform groundCheck;
    private float groundRadious = 0.2f;
    public LayerMask whatIsGround;
    private float counter;
    private AudioSource audioSr;

    [Header("Multiplayer")]
    [Range(0,5)]
    public float fixedDistanceMax = 0.5f;
    [Range(0, 0.5f)]
    public float fixedDistanceMin = 0.01f;

    // others
    private Animator _anim;

    // Use this for initialization
    void Start()
    {
        audioSr = GetComponentInChildren<AudioSource>();
        _anim = GetComponentInChildren<Animator>();
        //if (!_facingRight)
        //{
        //    Flip();
        //    _facingRight = !_facingRight;
        //}
        StartCoroutine(SetConfigPlayer());
    }

    // Update is called once per frame
    private void Update()
    {
        if (hasAuthority)
        {
            if (CanJump)
            {
                CheckJump();
            }
            if (canMove)
            {
                CheckMovement();
            }
        }
    }
    void FixedUpdate()
    {
        if (hasAuthority)
        {
            if (CanJump)
            {
                _grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadious, whatIsGround);
            }
        }
        
    }

    private IEnumerator SetConfigPlayer()
    {
        while (!GameManagerMultiplayer.instance.start)
        {
            if (!hasAuthority)
            {
                gameObject.tag = "Player 2";
                GetComponentInChildren<SpriteRenderer>().color = Color.green;
                if (isClient)
                    _facingRight = false;
            }
            else
            {
                gameObject.tag = "Player 1";
                GetComponentInChildren<SpriteRenderer>().color = Color.white;
                if (isServer)
                    _facingRight = false;
            }
            Debug.Log("Se esta haciendo");
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }
        
    }

    private void CheckMovement()
    {
        move = 0;
        move = Input.GetAxisRaw(movementAxis);

        int direction = _facingRight ? 1 : -1;

        transform.Translate(move * maxSpeed * direction * Time.deltaTime, 0, 0);

        if (!lookToMouse && ((move > 0 && !_facingRight) || (move < 0 && _facingRight)))
        {
            Flip();
        }

        if (lookToMouse)
        {
            float mouseXPos = Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
            if ((transform.position.x < mouseXPos && !_facingRight) || (transform.position.x > mouseXPos && _facingRight))
            {
                Flip();
            }
        }
        else if ((move > 0 && !_facingRight) || (move < 0 && _facingRight))
        {
            Flip();
        }

        if (_anim != null)
        {
            _anim.SetFloat("Speed", Mathf.Abs(move));
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        transform.Rotate(new Vector3(0, 180, 0), Space.World);
    }

    private void CheckJump()
    {
        if ((_grounded || (hasDoubleJump && !_doubleJump)) && Input.GetAxis(jumpAxis) > 0)
        {
            if (_jumpKeyPressed == false)
            {
                _jumpKeyPressed = true;
                _grounded = false;
                GetComponent<Rigidbody2D>().AddForce(new Vector2(0, jumpForce));
                _anim.SetBool("Jump", true);
                audioSr.Play();
                counter = 0;
                if (!_doubleJump && !_grounded)
                {
                    _doubleJump = true;
                }
            }
        }

        if (_grounded)
        {
            _doubleJump = false;

        }

        if (Input.GetAxis(jumpAxis) == 0 && _jumpKeyPressed)
        {
            _jumpKeyPressed = false;
        }

        if (_grounded && counter > 0.5f)
        {
            _anim.SetBool("Jump", false);
        }
        counter += Time.fixedDeltaTime;
    }

    public bool HasAuthority()
    {
        return hasAuthority;
    }

    public void LerpPosition(Vector3 desiredPosition, float smooth)
    {
        float distance = Vector3.Distance(transform.position, desiredPosition);
        if (distance >= fixedDistanceMax || distance <= fixedDistanceMin)
            transform.position = desiredPosition;
        else
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smooth * Time.deltaTime);
    }

    public void IsLookingRight(bool facingRight)
    {
            if (facingRight)
                transform.rotation = Quaternion.Euler(Vector3.zero);
            else
                transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
    }

    public void SetAnimator(bool jumping, bool moving)
    {
        _anim.SetBool("Jump", jumping);
        if(moving)
            _anim.SetFloat("Speed", 1);
        else
            _anim.SetFloat("Speed", 0);
    }

}
