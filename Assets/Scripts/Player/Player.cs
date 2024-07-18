using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float movementSpeed;
    public bool ableToMove;
    private float horizontal, vertical;

    void Start()
    {
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
