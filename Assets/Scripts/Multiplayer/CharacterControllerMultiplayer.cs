using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CharacterControllerMultiplayer : NetworkBehaviour
{
    [Header("Movement")]
    public bool canMove = true;
    public float maxSpeed = 15f;
    public bool lookToMouse;
    public string movementAxis;
    public bool _facingRight = true;

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

    // others
    private Animator _anim;

    // Use this for initialization
    void Start()
    {
        audioSr = GetComponentInChildren<AudioSource>();
        _anim = GetComponentInChildren<Animator>();
        if (!_facingRight)
        {
            Flip();
            _facingRight = !_facingRight;
        }

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

    private void CheckMovement()
    {
        float move = 0;

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
}
