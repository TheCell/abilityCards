using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float maxSpeed = 2.0f;
	public float jumpForce = 300.0f;
	private bool facingRight = true;
	
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
	}

	void Update()
	{
		if(grounded && Input.GetButtonDown("Jump"))
		{
			anim.SetBool("ground", false);
			Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
			rb.AddForce(new Vector2(0, jumpForce));
		}
	}

	// Update is called once per frame
	void FixedUpdate ()
	{
		Rigidbody2D rb = gameObject.GetComponent<Rigidbody2D>();
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool("ground", grounded);
		anim.SetFloat("vSpeed", rb.velocity.y);

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
}
