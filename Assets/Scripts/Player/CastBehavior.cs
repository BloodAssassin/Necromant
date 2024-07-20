using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastBehavior : MonoBehaviour
{
    public void SelfDestruct()
    {
        //Cast Fade
        LeanTween.value(gameObject, 0f, 1f, 0.6f).setOnComplete(() =>
        {
            Destroy(gameObject);
        });
    }
}
