using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using Cinemachine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    [SerializeField] GameObject glow;
    [SerializeField] GameObject cursor;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    public float movementSpeed;
    public bool ableToMove;
    public bool inCombat;
    private Color originalCursorColor;
    private float horizontal, vertical;

    [HideInInspector] public bool doingAction;

    void Start()
    {
        originalCursorColor = cursor.GetComponent<Image>().color;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Camera Controls
        CameraMove();

        if (ableToMove == true && doingAction == false)
        {
            Move();

            if (inCombat == true)
            {
                Cast();
            }
        }
        else if (doingAction == true)
        {
            StopMove();
        }
    }

    private void FixedUpdate()
    {
        //Player Movement
        rb.velocity = new Vector2 (horizontal, vertical) * movementSpeed;

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
        double moveIndex = 2;
        double movePercentageHorizontal = 0;
        double movePercentageVertical = 0;
        double screenPercent = 0.2;

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

        print(movePercentageHorizontal);

        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x = (float)(moveIndex * movePercentageHorizontal);
        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y = (float)(moveIndex * movePercentageVertical);
    }

    private void Cast()
    {
        Color glowColor = glow.GetComponent<SpriteRenderer>().color;
        GameObject hitEnemy = null;

        //Change Cursor Color On Hover
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (hit.collider != null && hit.transform.tag == "Human")
        {
            cursor.GetComponent<MouseCursor>().ChangeCursor(2);
            hitEnemy = hit.transform.gameObject;
        }
        else 
        {
            cursor.GetComponent<MouseCursor>().ChangeCursor(1);
        }

        //Click
        if (Input.GetKeyDown(Controls.cast))
        {
            StopMove();
            LeanTween.cancel(glow);

            //Animation
            animator.SetTrigger("Cast");

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
                    if (hitEnemy != null)
                    {
                        Destroy(hitEnemy);
                    }

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
