using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateScript : MonoBehaviour
{
    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }

    void Update()
    {

    }
}
