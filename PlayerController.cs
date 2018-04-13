using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private const float SPEED = 10;
    private const float JUMP_FORCE = 18;
    private const float GROUND_RADIUS = 0.2f;

    // INPUT
    public InputSystem inputSystem;
    private float inputHorizontal;
    private float inputVertical;

    // MOVEMENT
    private Rigidbody2D rigidBody2D;
    private float tempMove;

    // DUCKING
    private float time;
    private float duckTime = 2f;
    private bool isDucking = false;
    private Vector3 normalScale;

    // GROUNDCHECK
    public LayerMask whatIsGround;  // declare in inspector
    private Transform groundCheck;

    // ANIMATIONS
    public Animator face;         // declare in inspector
    public Animator limbs;        // declare in inspector

    private Vector2 startPosition;
    private bool freezeControls = false;


    private void Awake()
    {
        // Get inputsystem depending on device of player
        inputSystem = InputSystem.GetInputSystem();
        Debug.Log(inputSystem);

        groundCheck = transform.GetChild(0); // TEMP HACK!
        rigidBody2D = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        normalScale = transform.localScale;
    }

    private void Update()
    {
        if (InputMobile.instance != null) // TEMP HACK!
        {
            InputMobile.instance.GetTouch();
        }
        // Check axes of inputsystem
        inputHorizontal = inputSystem.GetAxis(GameAction.MOVE_HORIZONTAL);
        inputVertical = inputSystem.GetAxis(GameAction.JUMP);
        //CHECK FOR INPUT
        inputSystem.CheckInput();
        // Time starts counting when player ducks
        time += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        // If player isn't in UI panel, player can move
        if (!freezeControls)
        {
            MoveHorizontal();
            // Player can only jump or duck if he touches a ground object
            if (CheckIfGrounded() == true)
            {
                Jump();
                Duck();
            }
        }
    }

    private bool CheckIfGrounded()
    {
        // Check if player touches a ground object. If yes, return true.
        return (Physics2D.OverlapCircle(groundCheck.position, GROUND_RADIUS, whatIsGround));
    }

    private void MoveHorizontal()
    {
        rigidBody2D.velocity = new Vector2(inputHorizontal * SPEED, rigidBody2D.velocity.y);
        if (inputHorizontal >= 0.1)  // right
        {
            limbs.SetBool("isWalking", true);
            FlipPlayer(true);
        }
        else if (inputHorizontal <= -0.1) // left
        {
            limbs.SetBool("isWalking", true);
            FlipPlayer(false);
        }
        else
        {
            limbs.SetBool("isWalking", false);
        }
    }

    private void Jump()
    {
        if (inputVertical >= 0.01) {
            rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, JUMP_FORCE);
            tempMove = inputHorizontal;
            limbs.SetTrigger("jumpTrigger");
            //koppierite vincent
            if (rigidBody2D.velocity.y < 0 && rigidBody2D.velocity.y > -5f) {
                rigidBody2D.velocity = new Vector2(rigidBody2D.velocity.x, rigidBody2D.velocity.y * (1.5f * Time.deltaTime));
                }
            }
    }

    private void Duck()
    {
        if (inputVertical <= -0.01)
        {
            // Check if the player isn't already ducking
            if (!isDucking)
            {
                // Change player's scale
                // TEMP HACK: Waiting for animations
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * 0.6f, transform.localScale.z);
                isDucking = true;
                time = 0;
            }
        }

        // If two seconds are over or the player jumps
        if (isDucking && time >= duckTime || inputVertical >= 0.01)
        {
            // Change player's scale back
            // TEMP HACK: Waiting for animations
            transform.localScale = normalScale;
            isDucking = false;
        }
    }

    private void FlipPlayer(bool facingRight)
    {
        if (facingRight && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (!facingRight && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);

        }

    }

    // Called from Killzone
    public void Respawn()
    {
        transform.position = startPosition;
    }

    // Called from AchievementPanel
    public void ControlSwitch(bool freeze)
    {
        freezeControls = freeze;
    }
}