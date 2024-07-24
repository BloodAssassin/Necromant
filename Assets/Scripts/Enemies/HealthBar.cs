using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] Guard guard;
    [SerializeField] GameObject character;
    [SerializeField] GameObject flash;
    [SerializeField] GameObject background;
    [SerializeField] GameObject bar;

    private float maxHealth;
    private float health;
    private float barSize;
    private float scale;
    private float lastHealth;

    void Start()
    {
        foreach (Transform item in gameObject.transform)
        {
            item.gameObject.SetActive(false);
        }

        maxHealth = guard.maxHealth;
        health = guard.health;

        barSize = background.transform.localScale.x; //Full size
        scale = health/maxHealth * barSize; //Current size

        bar.transform.localScale = new Vector2(scale, bar.transform.localScale.y);
    }

    void Update()
    {
        //Damaged
        if (guard.health != health)
        {
            health = guard.health;
            HealthChanged();
        }

        //Update Flash
        flash.GetComponent<SpriteRenderer>().sprite = character.GetComponent<SpriteRenderer>().sprite;
        flash.transform.localScale = character.transform.localScale;

        //Enable/Disable Healthbar
        if (health == 0 && bar.transform.localScale.x == 0 || health == maxHealth && bar.transform.localScale.x == barSize)
        {
            foreach (Transform item in gameObject.transform)
            {
                item.gameObject.SetActive(false);
            }
        }
        else if (bar.activeInHierarchy == false)
        {
            foreach (Transform item in gameObject.transform)
            {
                item.gameObject.SetActive(true);
            }
        }
    }

    private void HealthChanged()
    {
        scale = health / maxHealth * barSize; //Current size

        //First Bar
        LeanTween.value(gameObject, bar.transform.localScale.x, scale, 0.2f).setOnUpdate((float val) =>
        {
            bar.transform.localScale = new Vector2(val, bar.transform.localScale.y);
        });

        //Delay
        LeanTween.value(gameObject, background.transform.localScale.x, scale, 0.25f).setOnComplete(() =>
        {
            //Second Bar
            LeanTween.value(gameObject, background.transform.localScale.x, scale, 0.1f).setOnUpdate((float val) =>
            {
                background.transform.localScale = new Vector2(val, background.transform.localScale.y);
            });
        });

        //Flash
        Color flashColor = flash.GetComponent<SpriteRenderer>().color;
        
        LeanTween.value(gameObject, flash.GetComponent<SpriteRenderer>().color.a, 1f, 0.1f).setOnUpdate((float val) =>
        {
            flash.GetComponent<SpriteRenderer>().color = new Color(flashColor.r, flashColor.g, flashColor.b, val);

            if (val == 1f)
            {
                LeanTween.value(gameObject, flash.GetComponent<SpriteRenderer>().color.a, 0f, 0.1f).setOnUpdate((float val) =>
                {
                    flash.GetComponent<SpriteRenderer>().color = new Color(flashColor.r, flashColor.g, flashColor.b, val);
                });
            }
        });
    }
}
