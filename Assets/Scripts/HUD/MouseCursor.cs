using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    public static bool mouseVisible;
    [SerializeField] Sprite targetCursor;
    [SerializeField] Sprite castCursor;

    void Start()
    {
        mouseVisible = false;
        gameObject.GetComponent<Image>().enabled = true;
    }

    void Update()
    {
        gameObject.transform.position = Input.mousePosition;

        Cursor.visible = mouseVisible;
    }

    public void ChangeCursor(int x)
    {
        if (x == 1)
        {
            gameObject.GetComponent<Image>().sprite = targetCursor;
        }
        else if (x == 2)
        {
            gameObject.GetComponent<Image>().sprite = castCursor;
        }
    }
}
