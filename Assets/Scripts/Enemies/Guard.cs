using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float walkSpeed;
    [SerializeField] float maxHealth;
    [SerializeField] float damage;

    [HideInInspector] public float health;

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        
    }
}
