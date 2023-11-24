using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Percy.EnemyVision 
{
    public class PlayerMovement : MonoBehaviour
    {
        public float move_speed = 7f;
        public float move_accel = 40f;
        public float rotate_speed = 150f;

        [Header("Ground")]
        public float gravity = 2f;
        public float ground_dist = 0.2f;
        public LayerMask ground_mask;

        [Header("Hide")]
        public bool can_hide = true;
        public KeyCode hideKey = KeyCode.LeftShift;
        private bool isHiding = false;

        [Header("Sound Effects")]
        [SerializeField] private AudioClip walkSFX;
        [SerializeField] private AudioClip runSFX;
        [SerializeField] private AudioClip crouchSFX;
        [SerializeField] private AudioClip attackSFX;

        [Header("Attack")]
        [SerializeField] private Transform attackPos;
        [SerializeField] private float attackSpeed = 2f;


        private Vector3 current_move = Vector3.zero;
        private Vector3 current_face = Vector3.forward;
        private Rigidbody rigid;
        private Animator animator;
        private Collider collide;
        private VisionTarget vision_target;
        private AudioSource audioSource;

        void Awake()
        {
            rigid = GetComponent<Rigidbody>();
            animator = GetComponentInChildren<Animator>();
            collide = GetComponentInChildren<Collider>();
            vision_target = GetComponent<VisionTarget>();
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            HandleInput();
        }

        void FixedUpdate()
        {
            Vector3 move_dir = Vector3.zero;
            if (Input.GetKey(KeyCode.W))
                move_dir += Vector3.forward;
            if (Input.GetKey(KeyCode.A))
                move_dir += Vector3.left;
            if (Input.GetKey(KeyCode.D))
                move_dir += Vector3.right;
            if (Input.GetKey(KeyCode.S))
                move_dir += Vector3.back;

            
           
            

            // Move
            move_dir = move_dir.normalized * Mathf.Min(move_dir.magnitude, 1f);
            current_move = Vector3.MoveTowards(current_move, move_dir, move_accel * Time.fixedDeltaTime);
            rigid.velocity = current_move * move_speed;

            bool grounded = CheckIfGrounded();
            if (!grounded)
                rigid.velocity += Vector3.down * gravity;

            if (current_move.magnitude > 0.1f)
                current_face = new Vector3(current_move.x, 0f, current_move.z).normalized;

            // Rotate
            Vector3 dir = current_face;
            dir.y = 0f;
            if (dir.magnitude > 0.1f)
            {
                Quaternion target = Quaternion.LookRotation(dir.normalized, Vector3.up);
                Quaternion reachedRotation = Quaternion.RotateTowards(transform.rotation, target, rotate_speed * Time.deltaTime);
                transform.rotation = reachedRotation;
            }

            HandleMovementAnimationsAndSFX();
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(hideKey))
            {
                /*isHiding = !isHiding;
                animator.SetBool("isCrouching", isHiding);
                if (vision_target)
                    vision_target.visible = !isHiding;
                if (collide)
                    collide.enabled = !isHiding;*/
                Debug.Log("bush!!!!!!!!!!!");
            }

            if (Input.GetButtonDown("Jump"))
            {
                Attack();
                Debug.Log("Attack");
            }
        }

        public bool CheckIfGrounded()
        {
            Vector3 origin = transform.position + Vector3.up * ground_dist * 0.5f;
            return Physics.Raycast(origin, Vector3.down, ground_dist, ground_mask);
        }

        private void HandleMovementAnimationsAndSFX()
        {
            bool isMoving = current_move.magnitude > 0.1f;
            animator.SetBool("isMoving", isMoving);
            if (isMoving)
            {
                PlaySFX(walkSFX);
            }
        }

        private void Attack()
        {
           
            animator.SetTrigger("attack");
            PlaySFX(attackSFX);

            foreach(Collider collider in Physics.OverlapSphere(attackPos.position, attackSpeed))
            {
                if (collider.CompareTag("Crop"))
                {
                    Crop crop = collider.GetComponent<Crop>();
                    if (crop != null)
                    {
                        crop.TakeDamage(1);
                    }
                }
            }

        }

        private void PlaySFX(AudioClip clip)
        {
            if (clip != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(clip);  
            }
        }
        public void ToggleCrouch()
        {
            isHiding = !isHiding;
            animator.SetBool("isCrouching", isHiding);
            if (vision_target)
                vision_target.visible = !isHiding;
           
        }
    }
}
