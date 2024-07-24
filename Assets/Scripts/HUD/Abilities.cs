using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    public GameObject basicCast; 
    public Image castBackground; 
    public Image castControl; 
    public Image castSpell;

    [Header("Default")]
    public Sprite defaultBackground;
    public Sprite defaultCastInput;
    public Sprite defaultCast;

    [Header("Inverted")]
    public Sprite invertedBackground;
    public Sprite invertedCastInput;
    public Sprite invertedCast;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Basic Cast
        if (Input.GetKeyDown(Controls.cast))
        {
            SpellCast();
        }
    }

    private void SpellCast()
    {
        castBackground.sprite = invertedBackground;
        castControl.sprite = defaultCastInput;
        castSpell.sprite = invertedCast;

        LeanTween.value(gameObject, 0f, 1f, 0.25f).setOnComplete(() =>
        {
            castBackground.sprite = defaultBackground;
            castControl.sprite = defaultCastInput;
            castSpell.sprite = defaultCast;
        });
    }
}
