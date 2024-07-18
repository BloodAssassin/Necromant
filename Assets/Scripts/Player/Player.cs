using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public float movementSpeed;
    public bool ableToMove;
    private float horizontal, vertical;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (ableToMove == true)
        {
            Move();
        }
    }

    private void FixedUpdate()
    {
        //Player Movement
        rb.velocity = new Vector2 (horizontal, vertical) * movementSpeed;

        //Turn Direction

        //Mouse Oriented
        if (rb.velocity == Vector2.zero)
        {
            //Turn Right
            if (Input.mousePosition.x < Screen.width / 2f && gameObject.transform.localScale.x > 0)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
            }
            //Turn Left
            else if (Input.mousePosition.x >= Screen.width / 2f && gameObject.transform.localScale.x < 0)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
            }
        }

        //Walking Oriented
        if (rb.velocity != Vector2.zero)
        {
            //Turn Left
            if (horizontal > 0 && gameObject.transform.localScale.x < 0)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
            }
            //Turn Right
            else if (horizontal < 0 && gameObject.transform.localScale.x > 0)
            {
                gameObject.transform.localScale = new Vector2(gameObject.transform.localScale.x * -1, gameObject.transform.localScale.y);
            }
        }

        //Walking Animation
        if (rb.velocity != Vector2.zero)
        {
            anim.SetBool("Walking", true);
        }
        else if (rb.velocity == Vector2.zero)
        {
            anim.SetBool("Walking", false);
        }
    }

    private void Move()
    {
        horizontal = 0f;
        vertical = 0f;

        //Vertical Movement
        if (Input.GetKey(Controls.moveUp))
        {
            vertical = 1f;
        }
        else if (Input.GetKey(Controls.moveDown))
        {
            vertical = -1f;
        }

        //Horizontal Movement
        if (Input.GetKey(Controls.moveRight))
        {
            horizontal = 1f;
        }
        else if (Input.GetKey(Controls.moveLeft))
        {
            horizontal = -1f;
        }
    }
}
