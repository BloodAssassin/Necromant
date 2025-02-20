using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Cinemachine;
using Unity.Mathematics;
using System;
using JetBrains.Annotations;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;

    [Header("Linked Objects")]
    [SerializeField] GameObject glow;
    [SerializeField] GameObject castRenderer;
    [SerializeField] GameObject cursor;
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    [Header("Abilities")]
    [SerializeField] List<GameObject> spells = new List<GameObject>();

    [Header("Layer Masks")]
    [SerializeField] LayerMask ignoreLayer;

    [Header("Attributes")]
    public float maxHealth = 100f;
    public float castDamage = 30f;
    public float movementSpeed;
    public float castRange;
    public float castCooldown;

    private bool castDebounce;
    private Color originalCursorColor;
    private float horizontal, vertical;

    [HideInInspector] public float health;
    [HideInInspector] public bool ableToMove;
    [HideInInspector] public bool inCombat;
    [HideInInspector] public bool doingAction;

    void Start()
    {
        //Assignments
        originalCursorColor = cursor.GetComponent<Image>().color;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        //Default Values
        castRenderer.GetComponent<LineRenderer>().useWorldSpace = true;
        health = maxHealth;

        Color glowColor = glow.GetComponent<SpriteRenderer>().color;
        glow.GetComponent<SpriteRenderer>().color = new Color(glowColor.r, glowColor.g, glowColor.b, 0f);
    }

    void Update()
    {
        //Camera Controls
        CameraMove();

        //Abilities
        Cast();

        if (ableToMove == true && doingAction == false)
        {
            Move();
        }
        else if (doingAction == true)
        {
            StopMove();
        }
    }

    private void FixedUpdate()
    {
        //Player Movement
        Vector2 movement = new Vector2(horizontal, vertical);

        //Fix Diagonal Speed
        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        rb.velocity = movement * movementSpeed;

        //Turn Direction

        //Mouse Oriented
        if (horizontal == 0f && doingAction == false)
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
        else if (rb.velocity != Vector2.zero)
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
            animator.SetBool("Walking", true);
        }
        else if (rb.velocity == Vector2.zero)
        {
            animator.SetBool("Walking", false);
        }
    }

    private void StopMove()
    {
        horizontal = 0f;
        vertical = 0f;
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

    private void CameraMove()
    {
        double moveIndex = 3;
        double movePercentageHorizontal = 0;
        double movePercentageVertical = 0;
        double screenPercent = 0.15;

        //Horizontal
        //Right
        if (Input.mousePosition.x > Screen.width * (1 - screenPercent))
        {
            movePercentageHorizontal = Input.mousePosition.x - Screen.width * (1 - screenPercent);
            movePercentageHorizontal = movePercentageHorizontal / (Screen.width - Screen.width * (1 - screenPercent));

        }
        //Left
        else if (Input.mousePosition.x < Screen.width * screenPercent)
        {
            movePercentageHorizontal = Screen.width * screenPercent - Input.mousePosition.x;
            movePercentageHorizontal = movePercentageHorizontal / (Screen.width * screenPercent);
            movePercentageHorizontal = -movePercentageHorizontal;
        }

        //Vertical
        //Up
        if (Input.mousePosition.y > Screen.height * (1 - screenPercent))
        {
            movePercentageVertical = Input.mousePosition.y - Screen.height * (1 - screenPercent);
            movePercentageVertical = movePercentageVertical / (Screen.height - Screen.height * (1 - screenPercent));

        }
        //Down
        else if (Input.mousePosition.y < Screen.height * screenPercent)
        {
            movePercentageVertical = Screen.height * screenPercent - Input.mousePosition.y;
            movePercentageVertical = movePercentageVertical / (Screen.height * screenPercent);
            movePercentageVertical = -movePercentageVertical;
        }

        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x = (float)(moveIndex * movePercentageHorizontal);
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y = (float)(moveIndex * movePercentageVertical);
    }

    private void Cast()
    {
        Color glowColor = glow.GetComponent<SpriteRenderer>().color;
        //GameObject hitEnemy = null;
        
        float castLenght = Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), gameObject.transform.position);
        float minimumRange = 3f;

        //Can't exceed cast range
        if (castLenght > castRange)
        {
            castLenght = castRange;
        }
        else if (castLenght < minimumRange)
        {
            castLenght = minimumRange;
        }

        //Change Cursor Color On Hover
        RaycastHit2D cursorOver = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (cursorOver.collider != null && cursorOver.transform.tag == "Human")
        {
            if (cursorOver.collider.GetComponent<Guard>() && cursorOver.collider.GetComponent<Guard>().health > 0)
            {
                cursor.GetComponent<MouseCursor>().ChangeCursor(2);
            }
        }
        else 
        {
            cursor.GetComponent<MouseCursor>().ChangeCursor(1);
        }

        //Use
        if (Input.GetKeyDown(Controls.cast) && castDebounce == false)
        {
            //Cooldown
            castDebounce = true;
            LeanTween.value(castRenderer, 0f, 1f, castCooldown).setOnComplete(() => { castDebounce = false; });

            StopMove();
            LeanTween.cancel(glow);

            //Instiantiate Cast Visual
            //GameObject castInstance = Instantiate(castRenderer);
            //castInstance.transform.SetParent(gameObject.transform, false);
            GameObject castInstance = Instantiate(spells[0]);

            //Animation
            animator.SetTrigger("Cast");

            //Cast the Spell
            Vector2 cursorDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - gameObject.transform.position).normalized;
            RaycastHit2D spellHit = Physics2D.Raycast(gameObject.transform.position, cursorDirection, castLenght, ~ignoreLayer);

            //Display Cast Visual
            //castInstance.GetComponent<LineRenderer>().enabled = true;
            //castInstance.GetComponent<LineRenderer>().SetPosition(0, gameObject.transform.position);

            //Cast Rotation
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPosition.z = 0;

            Vector3 direction = mouseWorldPosition - transform.position;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            castInstance.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            //Other Cast Values
            castInstance.transform.position = spells[0].transform.position;
            castInstance.transform.localScale = spells[0].transform.localScale * transform.localScale.y;

            //Activate
            Ray2D ray = new Ray2D(gameObject.transform.position, cursorDirection);
            Vector2 endPoint = ray.GetPoint(castLenght);
            Vector2 startPoint = ray.GetPoint(2.5f);

            castInstance.GetComponent<CastBehavior>().damage = castDamage;
            castInstance.GetComponent<CastBehavior>().endPoint = endPoint;
            castInstance.GetComponent<CastBehavior>().startPoint = startPoint;
            castInstance.SetActive(true);

            //Human Hit
            //if (spellHit.collider != null && spellHit.transform.tag == "Human")
            //{
            //    hitEnemy = spellHit.transform.gameObject;
            //}

            //Cast Fade
            //castInstance.GetComponent<CastBehavior>().SelfDestruct();

            //Rotate Player
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

            //Glow
            //Fade In
            LeanTween.value(glow, glow.GetComponent<SpriteRenderer>().color.a, 1f, 0.1f).setOnUpdate((float val)=>
            {
                glow.GetComponent<SpriteRenderer>().color = new Color(glowColor.r, glowColor.g, glowColor.b, val);

                if (val == 1f)
                {
                    //Wait
                    LeanTween.value(glow, 0f, 1f, 1f).setOnComplete(() =>
                    {
                        //Fade Out
                        LeanTween.value(glow, glow.GetComponent<SpriteRenderer>().color.a, 0f, 0.5f).setOnUpdate((float val) =>
                        {
                            glow.GetComponent<SpriteRenderer>().color = new Color(glowColor.r, glowColor.g, glowColor.b, val);
                        });
                    });
                }
            });
        }
    }
}
