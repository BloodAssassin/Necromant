using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject blackFade;

    void Start()
    {
        blackFade.GetComponent<CanvasGroup>().alpha = 1f;

        LeanTween.value(gameObject, blackFade.GetComponent<CanvasGroup>().alpha, 0f, 0.2f).setOnUpdate((float val) =>
        {
            blackFade.GetComponent<CanvasGroup>().alpha = val;
        });
    }

   
    void Update()
    {
        
    }

    public void HuntingGrounds()
    {
        LeanTween.value(gameObject, blackFade.GetComponent<CanvasGroup>().alpha, 1f, 0.2f).setOnUpdate((float val) =>
        {
            blackFade.GetComponent<CanvasGroup>().alpha = val;

            if (val == 1f)
            {
                SceneManager.LoadScene("HuntingGround");
            }
        });
    }
}
