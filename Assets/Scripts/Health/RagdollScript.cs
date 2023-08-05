using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollScript : MonoBehaviour
{
    public GameObject thisGameObject;
    public Animator animator;


    void Start()
    {
        GetRagdollBits();
        Ragdolloff();


    }

    Collider[] ragdollColliders;
    Rigidbody[] limbsRigidBodies;
    void GetRagdollBits()
    {
        ragdollColliders = thisGameObject.GetComponentsInChildren<Collider>();
        limbsRigidBodies = thisGameObject.GetComponentsInChildren<Rigidbody>();
    }

    public void RagdollOn()
    {
        animator.enabled = false;
        foreach (Collider col in ragdollColliders)
        {
            col.enabled = true;
        }

        foreach (Rigidbody rigidbody in limbsRigidBodies)
        {
            rigidbody.isKinematic = false;
        }

        GetComponent<Rigidbody>().isKinematic = true;
    }


    public void Ragdolloff()
    {
        foreach(Collider col in ragdollColliders)
        {
            col.enabled = false;
        }

        foreach(Rigidbody rigidbody in limbsRigidBodies)
        {
            rigidbody.isKinematic = true;
        }

        animator.enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
    }
}
