using Cinemachine.Utility;
using Pathfinding;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [Header("Linked Objects")]
    [SerializeField] GameObject player;
    [SerializeField] CircleCollider2D distanceCollider;

    [Header("Properties")]
    [SerializeField] float walkSpeed;
    [SerializeField] float maxHealth;
    public float health;
    [SerializeField] float damage;

    [Header("Attack Points")]
    [SerializeField] Transform leftPoint;
    [SerializeField] Transform rightPoint;

    private bool dead = false;
    private Vector3 previousPosition;
    private Rigidbody2D rb;
    private Animator animator;
    private AIPath pathFinding;
    private AIDestinationSetter destination;
    private float lastHealth;

    void Start()
    {
        //Assignments
        pathFinding= GetComponent<AIPath>();
        destination= GetComponent<AIDestinationSetter>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetFloat("Health", health);

        //Default Values
        health = maxHealth;
        lastHealth = health;
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
        else if(dead == false && health == 0)
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Attack", false);
            Death();
        }

        //Damaged
        if (lastHealth != health)
        {
            animator.SetFloat("Health", health);
            lastHealth = health;
            Freeze();

            if (health < 0)
            {
                health = 0;
            }
        }
    }

    private void Freeze()
    {
        animator.SetBool("Frozen", true);
        animator.speed = 0;
        pathFinding.maxSpeed = 0;

        LeanTween.value(gameObject, 0f, 1f, 0.25f).setOnComplete(() =>
        {
            animator.speed = 1f;
            pathFinding.maxSpeed = walkSpeed;
            animator.SetBool("Walking", true);

            //Delay
            LeanTween.value(gameObject, 0f, 1f, 0.25f).setOnComplete(() =>
            {
                animator.SetBool("Frozen", false);
            });
        });
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
        else if (animator.speed != 0 && animator.GetBool("Frozen") == false)
        {
            animator.SetBool("Walking", false);
        }

        previousPosition = transform.position;
    }

    private void Attack()
    {
        if (player.GetComponent<Player>().health > 0)
        {
            animator.SetBool("Attack", true);
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    private void StopMoving()
    {
        destination.target = transform;
    }

    private void Death()
    {
        dead = true;
        StopMoving();
        animator.SetTrigger("Death");

        distanceCollider.enabled = false;
    }
}
