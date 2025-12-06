using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscData : MonoBehaviour
{
    [SerializeField] private AudioClip trackClip;
    public AudioClip TrackClip()
    {
        return trackClip;
    }
}
