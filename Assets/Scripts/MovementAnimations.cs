using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MovementAnimations : MonoBehaviour
{
    private float speed;
    [SerializeField] Animator animator;
    [SerializeField] Rigidbody rb;
    [SerializeField] FirstPersonController fpc;
    public UIPlayerManager uIPlayerManager;


    bool isGrounded;
    void Update()
    {
        isGrounded = fpc.isGrounded;
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && !uIPlayerManager.setTarget)
        {
            animator.SetTrigger("Jump");
            isGrounded = false;
        }
        if (Input.GetKey(KeyCode.W))
        {
            animator.SetBool("Forward", true);
        }
        else
        {
            animator.SetBool("Forward", false);
        }
        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("Left", true);
        }
        else
        {
            animator.SetBool("Left", false);
        }
        if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("Right", true);
        }
        else
        {
            animator.SetBool("Right", false);
        }
        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("Backward", true);
        }
        else
        {
            animator.SetBool("Backward", false);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
    }
}
