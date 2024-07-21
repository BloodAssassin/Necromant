using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [Header("Linked Objects")]
    [SerializeField] GameObject player;

    [Header("Properties")]
    [SerializeField] float walkSpeed;
    [SerializeField] float maxHealth;
    [SerializeField] float damage;

    [Header("Attack Points")]
    [SerializeField] Transform leftPoint;
    [SerializeField] Transform rightPoint;

    private Vector3 previousPosition;
    private float pointDistance = Mathf.Infinity;
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
        //Alive
        if (health > 0)
        {
            //Check if Walking
            Walking();

            //Check For Attack
            if (pathFinding.reachedDestination == true)
            {
                Attack();
            }
            else
            {
                animator.SetBool("Attack", false);
            }

            //Shortest Point Distance
            float leftDistance = (leftPoint.transform.position - transform.position).magnitude;
            float rightDistance = (rightPoint.transform.position - transform.position).magnitude;

            if (leftDistance < rightDistance)
            {
                GetComponent<AIDestinationSetter>().target = leftPoint;
            }
            else
            {
                GetComponent<AIDestinationSetter>().target = rightPoint;
            }
        }
        //Dead
        else
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Attack", false);
        }
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

    private void Attack()
    {
        if (pathFinding.reachedDestination == true && player.GetComponent<Player>().health > 0)
        {
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }
}
