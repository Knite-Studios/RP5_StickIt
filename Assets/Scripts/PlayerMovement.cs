using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 2.0f;
    [SerializeField] private float runSpeed = 5.0f;
    [SerializeField] private float crouchSpeed = 1.0f;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip walkSFX;
    [SerializeField] private AudioClip runSFX;
    [SerializeField] private AudioClip crouchSFX;
    [SerializeField] private AudioClip attackSFX;
    [SerializeField] private AudioClip interactSFX;

    [Header("Interaction")]
    [SerializeField] private LayerMask cropLayer;
    [SerializeField] private LayerMask exitLayer;

    private CharacterController controller;
    private Animator animator;
    private AudioSource audioSource;
    private Vector3 moveDirection = Vector3.zero;
    private bool isCrouching = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        MovePlayer();
        HandleInput();
    }

    private void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        moveDirection = new Vector3(horizontal, 0, vertical).normalized;

        if (moveDirection != Vector3.zero)
        {
            float speed = isCrouching ? crouchSpeed : (Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed);
            controller.Move(moveDirection * speed * Time.deltaTime);
            transform.forward = moveDirection;

            // Handle movement animations and SFX
            HandleMovementAnimationsAndSFX(speed);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }

    private void HandleInput()
    {
        // Crouch
        if (Input.GetKeyDown(KeyCode.C))
        {
            isCrouching = !isCrouching;
            animator.SetBool("isCrouching", isCrouching);
            PlaySFX(crouchSFX);
        }
        // Attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
        // Interact (for collectibles, etc.)
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }
    private void Attack()
    {
        // Attack logic here
        animator.SetTrigger("attack");
        PlaySFX(attackSFX);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f))
        {
            Crop crop = hit.collider.GetComponent<Crop>();
            if (crop != null)
            {
                crop.TakeDamage(1); // Assuming each attack deals 1 damage
            }
        }
    }
    private void Interact()
    {
        // Interact logic here
        animator.SetTrigger("interact");
        PlaySFX(interactSFX);
        // Check for exit interaction
    }

    private void HandleMovementAnimationsAndSFX(float speed)
    {
        animator.SetBool("isMoving", true);
        if (speed == runSpeed)
        {
            PlaySFX(runSFX);
        }
        else if (speed == walkSpeed)
        {
            PlaySFX(walkSFX);
        }
    }

    private void PlaySFX(AudioClip clip)
    {
        if (clip != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Additional methods for AI interaction, crop damage, etc.
}
