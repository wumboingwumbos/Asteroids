using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource soundPlayer;
    public void posUISound()
    {
        soundPlayer.Play();
    }
}
