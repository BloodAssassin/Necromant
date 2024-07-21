using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Avoid : MonoBehaviour
{
    public float separationDistance = 1.0f;
    public float separationForce = 1.0f;
    public float speed = 2.0f;
    private Seeker seeker;
    private Path path;
    private int currentWaypoint = 0;
    private Rigidbody2D rb;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        // Start a new path to the target position
        seeker.StartPath(transform.position, GetTargetPosition(), OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
        else
        {
            Debug.LogError("Path calculation failed: " + p.error);
        }
    }

    void Update()
    {
        if (path == null) return;

        // Follow the path
        if (currentWaypoint < path.vectorPath.Count)
        {
            Vector3 direction = ((Vector3)path.vectorPath[currentWaypoint] - transform.position).normalized;
            Vector3 separation = Vector3.zero;

            // Apply separation force
            foreach (var other in FindObjectsOfType<Guard>())
            {
                if (other != this)
                {
                    float distance = Vector3.Distance(transform.position, other.transform.position);
                    if (distance < separationDistance)
                    {
                        Vector3 dir = transform.position - other.transform.position;
                        separation += dir.normalized / distance;
                    }
                }
            }

            // Combine pathfinding direction with separation force
            Vector3 moveDirection = (direction + separation * separationForce).normalized;
            rb.velocity = moveDirection * speed;

            // Check if we are close to the next waypoint
            if (Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]) < 0.1f)
            {
                currentWaypoint++;
            }
        }
    }

    Vector3 GetTargetPosition()
    {
        // Implement your logic to get the target position
        return Vector3.zero;
    }
}
