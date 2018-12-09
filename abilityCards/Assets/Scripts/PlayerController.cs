using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float maxSpeed = 2.0f;
	private float jumpForce = 200.0f;
	private bool facingRight = true;

	private bool jumpPressed = false;
	private bool doubleJumped = false;
	private float jumpStarted = 0.0f;
	private float jumpCheckDelay = 0.07f;

	// ground check
	private bool grounded = false;
	public Transform groundCheck;
	private float groundRadius = 0.165f;
	public LayerMask whatIsGround;

	private Animator anim;

	// Use this for initialization
	void Start ()
	{
		anim = GetComponent<Animator>();
		jumpStarted = Time.realtimeSinceStartup;
	}

	void Update()
	{
		JumpIfGroundedAndCommandGiven();
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool("ground", grounded);
		anim.SetFloat("vSpeed", rb.velocity.y);

		if (grounded)
		{
			resetDoubleJump();
		}

		float move = Input.GetAxis("Horizontal");
		anim.SetFloat("speed", Mathf.Abs(move));

		rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);

		if(move > 0.0f && !facingRight)
		{
			Flip();
		}
		else if(move < 0.0f && facingRight)
		{
			Flip();
		}
	}

	private void Flip()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	private void JumpIfGroundedAndCommandGiven()
	{
		updateJumpPressed();

		if (grounded && Input.GetButtonDown("Jump"))
		{
			Jump();
			jumpStarted = Time.realtimeSinceStartup;
			Debug.Log("Jump");
		}

		if (!grounded && jumpPressed && !doubleJumped && ((jumpStarted + this.jumpCheckDelay) < Time.realtimeSinceStartup))
		{
			Jump();
			doubleJumped = true;
			Debug.Log("Double Jump");
		}
	}

	private void updateJumpPressed()
	{
		if (Input.GetButtonDown("Jump"))
		{
			this.jumpPressed = true;
		}

		if (Input.GetButtonUp("Jump"))
		{
			this.jumpPressed = false;
		}
	}

	private void resetDoubleJump()
	{
		this.doubleJumped = false;
	}

	private void Jump()
	{
		Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
		rb.AddForce(new Vector2(0, jumpForce));
	}
}
