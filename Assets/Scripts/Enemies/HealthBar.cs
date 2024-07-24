using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Guard guard;
    [SerializeField] GameObject background;
    [SerializeField] GameObject bar;

    private float maxHealth;
    private float health;
    private float barSize;
    private float scale;
    private float lastHealth;

    void Start()
    {
        background.SetActive(false);
        bar.SetActive(false);

        maxHealth = guard.maxHealth;
        health = guard.health;

        barSize = background.transform.localScale.x; //Full size
        scale = health/maxHealth * barSize; //Current size

        bar.transform.localScale = new Vector2(scale, bar.transform.localScale.y);
    }

    void Update()
    {
        if (guard.health != health)
        {
            health = guard.health;
            HealthChanged();
        }
    }

    private void HealthChanged()
    {
        scale = health / maxHealth * barSize; //Current size

        bar.transform.localScale = new Vector2(scale, bar.transform.localScale.y);

        if (health == 0 || health == maxHealth)
        {
            background.SetActive(false);
            bar.SetActive(false);
        }
        else if (bar.activeInHierarchy == false)
        {
            background.SetActive(true);
            bar.SetActive(true);
        }
    }
}
