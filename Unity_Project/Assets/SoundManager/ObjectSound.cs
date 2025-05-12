using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectSound : MonoBehaviour
{
    public AudioSource src;

    public AudioClip sound;

    public void Start()
    {
        src.clip = sound;
        src.Play();

        
    }

    public void Update()
    {
        if (!src.isPlaying)
            src.Play();
    }

    public void OnClick()
    {
        src.Stop();
    }
}

