using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastBehavior : MonoBehaviour
{
    public float damage;
    public float speed;
    [SerializeField] GameObject visual;
    private  Animator animator;
    private bool hit = false;
    [HideInInspector] public Vector2 startPoint;
    [HideInInspector] public Vector2 endPoint;

    private void Start()
    {
        animator = GetComponent<Animator>();
        transform.position = startPoint;
    }

    private void Update()
    {
        if (hit == false)
        {
            transform.position = Vector2.MoveTowards(transform.position, endPoint, speed * Time.deltaTime);
        }

        if (hit == true && animator.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if ((Vector2)transform.position == endPoint)
        {
            SelfDestruct();
        }
    }

    private void SelfDestruct()
    {
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        animator.SetTrigger("Hit");
        hit = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Human" && collision.GetComponent<Guard>().health > 0)
        {
            collision.GetComponent<Guard>().health -= damage;
            SelfDestruct();
        }
    }
}
