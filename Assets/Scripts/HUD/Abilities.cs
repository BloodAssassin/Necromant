using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Abilities : MonoBehaviour
{
    public Player player;

    [Header("Basic Cast")]
    public GameObject basicCast; 
    public Image castBackground; 
    public Image castBar;
    public Image castControl; 
    public Image castSpell;

    [Header("Reanimation")]
    public GameObject reanimate;
    public Image reanimateBackground;
    public Image reanimatetBar;
    public Image reanimateControl;
    public Image reanimateSpell;

    [Header("Default")]
    public Sprite defaultCast;
    public Sprite defaultReanimate;

    [Header("Inverted")]
    public Sprite invertedCast;
    public Sprite invertedReanimate;

    float spellCooldown;
    float reanimateCooldown;

    void Start()
    {
        spellCooldown = player.castCooldown;
        reanimateCooldown = 2f;
    }

    // Update is called once per frame
    void Update()
    {
        //Basic Cast
        if (Input.GetKeyDown(Controls.cast) && castBar.fillAmount == 0f)
        {
            SpellCast();
        }

        //Reanimate
        if (Input.GetKeyDown(Controls.reanimate) && reanimatetBar.fillAmount == 0f)
        {
            Reanimate();
        }
    }

    private void SpellCast()
    {
        castSpell.sprite = invertedCast;
        castBar.fillAmount = 1f;
        castControl.color = new Color(0.7f, 0.7f, 0.7f);

        //Button Up
        LeanTween.value(gameObject, 0f, 1f, 0.2f).setOnComplete(() =>
        {
            castControl.color = new Color(1f, 1f, 1f);
        });


        //Return to Default Icon
        LeanTween.value(gameObject, 0f, 1f, spellCooldown).setOnComplete(() =>
        {
            castSpell.sprite = defaultCast;
        });

        //Cooldown
        LeanTween.value(gameObject, 1f, 0f, 0.25f).setOnComplete(() =>
        {
            LeanTween.value(gameObject, 1f, 0f, spellCooldown - 0.25f).setOnUpdate((float val) =>
            {
                castBar.fillAmount = val;
            });
        });
    }

    private void Reanimate()
    {
        reanimateSpell.sprite = invertedReanimate;
        reanimatetBar.fillAmount = 1f;
        reanimateControl.color = new Color(0.7f, 0.7f, 0.7f);

        //Button Up
        LeanTween.value(gameObject, 0f, 1f, 0.2f).setOnComplete(() =>
        {
            reanimateControl.color = new Color(1f, 1f, 1f);
        });

        //Return to Default Icon
        LeanTween.value(gameObject, 0f, 1f, reanimateCooldown).setOnComplete(() =>
        {
            reanimateSpell.sprite = defaultReanimate;
        });

        //Cooldown
        LeanTween.value(gameObject, 1f, 0f, 0.25f).setOnComplete(() =>
        {
            LeanTween.value(gameObject, 1f, 0f, reanimateCooldown - 0.25f).setOnUpdate((float val) =>
            {
                reanimatetBar.fillAmount = val;
            });
        });
    }
}
