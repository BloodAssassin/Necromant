using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float walkSpeed;
    [SerializeField] float maxHealth;
    [SerializeField] float damage;
    private Vector3 previousPosition;


    private Rigidbody2D rb;
    private Animator animator;
    private AIPath pathFinding;
    [HideInInspector] public float health;

    void Start()
    {
        //Assignments
        pathFinding= GetComponent<AIPath>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        //Default Values
        health = maxHealth;

        previousPosition = transform.position;

        //Pathfinding
        pathFinding.maxSpeed = walkSpeed;
        pathFinding.maxAcceleration = 100f;
        pathFinding.slowWhenNotFacingTarget = false;
    }

    void Update()
    {
        //Check if Walking
        Walking();
    }

    private void Walking()
    {
        if (pathFinding.desiredVelocity.magnitude > 0.1f)
        {
            animator.SetBool("Walking", true);

            //SideSwitching
            //Right
            if (pathFinding.desiredVelocity.x > 0f)
            {
                if (transform.localScale.x < 0f)
                {
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                }
            }
            //Left
            else if (pathFinding.desiredVelocity.x < 0f)
            {
                if (transform.localScale.x > 0f)
                {
                    transform.localScale = new Vector2(-transform.localScale.x, transform.localScale.y);
                }
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        previousPosition = transform.position;
    }
}
