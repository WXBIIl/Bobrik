using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Sounds : MonoBehaviour
{
    public AudioSource myFx;
    public AudioClip hoverFX;
    public AudioClip pressedFX;

    public void HoverSound()
    {
        myFx.PlayOneShot(hoverFX);
    }
    public void ClickSound()
    {
        myFx.PlayOneShot(pressedFX);
    }
}
