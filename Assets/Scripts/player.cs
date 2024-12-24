using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class player : MonoBehaviour
{
    private Animator animator;          // Reference to the Animator component.
    public float howHigh = 250;         // The vertical impulse applied when the player jumps.
    private Rigidbody2D rigidbody2D;    // Reference to the Rigidbody2D component.
    public gameManager gm;              // Reference to the GameManager for handling collisions with obstacles.
    private AudioSource audioPlayer;    // Reference to the AudioSource component for playing audio.
    public AudioClip jump;              // AudioClip for the jump sound effect.

    /// <summary>
    /// Called before the first frame update. Initializes player components.
    /// </summary>
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        audioPlayer = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Called once per frame. Handles player input and updates player state.
    /// </summary>
    void Update()
    {
        // Check for jump input and player position
        if (Input.GetKeyDown(KeyCode.Space) && GameObject.Find("King").transform.position.y <= -1.95)
        {
            animator.SetBool("isJumping", true);
            rigidbody2D.AddForce(new Vector2(0, howHigh));
            audioPlayer.clip = jump;
            audioPlayer.Play();
        }
    }
    /// <summary>
    /// Handles collision events with other 2D colliders.
    /// </summary>
    /// <param name="collision">The Collision2D object representing the collision.</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the player collided with the floor
        if (collision.gameObject.tag == "floor") 
        {
            animator.SetBool("isJumping", false);
        }

        // Check if the player collided with an obstacle
        if (collision.gameObject.tag == "obstacle")
        {
            // Call rockCollision method in the GameManager
            gm.rockCollision();
        }
    }
}
