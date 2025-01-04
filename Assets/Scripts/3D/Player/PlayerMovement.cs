using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public AudioSource playerAudio;
    public AudioClip stepSound;
    private Animator Animator;
    private PlayerManager Player_Manager;

    public float speed = 8;
    public float bob = 0.5f;
    public float sway = 5f;
    public Transform pivot;
    public Transform cameraParent;
    private float moveTime;
    private int i = 0;
    private bool swayLeft = true;
    private bool bobDown = true;

    private float gravity = -9.81f * 2;
    public Transform groundCheck;
    private float groundDistance = 0.4f;
    public LayerMask groundMask;
    Vector3 velocity;
    bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        Animator = gameObject.GetComponent<Animator>();
        Player_Manager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
        Animator.SetFloat("Healthiness", Player_Manager.healthiness);
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        if (x != 0 || z != 0)
        {
            Quaternion defaultRotation = Quaternion.Euler(0f, 0f, 0f);
            Quaternion swayLeftRotation = Quaternion.Euler(sway, 0f, sway);
            Quaternion swayRightRotation = Quaternion.Euler(sway, 0f, -sway);

            moveTime += Time.deltaTime * (speed);
            if (bobDown == true)
            {
                cameraParent.localPosition = new Vector3(0, Mathf.SmoothStep(1.75f, 1.75f - bob, moveTime), 0);
                if (swayLeft == true)
                {
                    pivot.localRotation = Quaternion.Slerp(defaultRotation, swayLeftRotation, moveTime);
                }
                else
                {
                    pivot.localRotation = Quaternion.Slerp(defaultRotation, swayRightRotation, moveTime);
                }
            }
            else
            {
                cameraParent.localPosition = new Vector3(0, Mathf.SmoothStep(1.75f - bob, 1.75f, moveTime), 0);
                if (swayLeft == true)
                {
                    pivot.localRotation = Quaternion.Slerp(swayLeftRotation, defaultRotation, moveTime);
                }
                else
                {
                    pivot.localRotation = Quaternion.Slerp(swayRightRotation, defaultRotation, moveTime);
                }
            }

        }
        while (moveTime >= 1)
        {
            moveTime -= 1;
            if (bobDown)
            {
                playerAudio.PlayOneShot(stepSound);
            }
            bobDown = !bobDown;
            i += 1;
        }
        while (i >= 2)
        {
            swayLeft = !swayLeft;
            i -= 2;
        }



    }
}
