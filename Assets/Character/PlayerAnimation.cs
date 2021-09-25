using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;

    private int horizontal = Animator.StringToHash("Horizontal");
    private int vertical = Animator.StringToHash("Vertical");

    public float snappedVertical = 0f;
    public float snapppedHorizontal = 0f;
    private Rigidbody playerRigidbody;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void OnAnimatorMove()
    {
        playerRigidbody.drag = 0;
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = 0;
        Vector3 velocity = deltaPosition / Time.deltaTime;
        playerRigidbody.velocity = velocity;
    }

    public void UpdateAnimatorValues(float horizontalMovment, float verticalMovement)
    {
        // Snapped Vertical
        if (verticalMovement > 0 && verticalMovement < 0.55f)
        {
            snappedVertical = 0.5f;
        }
        else if (verticalMovement > 1)
        {
            snappedVertical = 2f;
        }
        else if (verticalMovement >= 0.55f)
        {
            snappedVertical = 1f;
        }
        else if (verticalMovement < 0 && verticalMovement > -0.55f)
        {
            snappedVertical = -0.5f;
        }
        else if (verticalMovement <= -0.55f)
        {
            snappedVertical = -1f;
        }
        else
        {
            snappedVertical = 0;
        }

        // Snappped Horizontal
        if (horizontalMovment > 0 && horizontalMovment < 0.55f)
        {
            snapppedHorizontal = 0.5f;
        }
        else if (horizontalMovment > 0.55f)
        {
            snapppedHorizontal = 1f;
        }
        else if (horizontalMovment < 0 && horizontalMovment > -0.55f)
        {
            snapppedHorizontal = -0.5f;
        }
        else if (horizontalMovment < -0.55f)
        {
            snapppedHorizontal = -1f;
        }
        else
        {
            snapppedHorizontal = 0;
        }
        animator.SetFloat(horizontal, snapppedHorizontal, 0.1f, Time.deltaTime);
        animator.SetFloat(vertical, snappedVertical, 0.1f, Time.deltaTime);
    }
}
