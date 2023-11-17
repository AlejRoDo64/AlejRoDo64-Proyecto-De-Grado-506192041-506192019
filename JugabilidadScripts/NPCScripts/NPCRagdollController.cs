using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;

public class NPCRagdollController : MonoBehaviour
{
    //Se añade al modelo del personaje
    Animator NPCAnimator;

    Rigidbody NPCBody;
    Rigidbody[] NPCBones;

    NPCController NPC;

    public List<NPCHitMultiplier> NPCHitStats;
    void Start()
    {
        NPCAnimator = GetComponent<Animator>();

        NPCBody = GetComponentInParent<Rigidbody>();
        NPCBones = GetComponentsInChildren<Rigidbody>();

        NPC = GetComponentInParent<NPCController>();

        SetUp();
    }
    public void SetUp()
    {
        LayerMask layerOfHits = LayerMask.NameToLayer("Hitbox");

        foreach (Rigidbody bone in NPCBones)
        {
            //colocar los huesos en collider continuo y lo añade a layer of hits
            bone.collisionDetectionMode = CollisionDetectionMode.Continuous;
            bone.gameObject.layer = layerOfHits;

            NPCBodyPartHitCheck partToCheck = bone.gameObject.AddComponent<NPCBodyPartHitCheck>();

            partToCheck.NPC = NPC;

            string bName = bone.gameObject.name.ToLower();

            foreach (NPCHitMultiplier hit in NPCHitStats)
            {//si contiene el nombre asigando en el nombre del hueso
                if (bName.Contains(hit.boneName))
                {//hace la multplicacion de daño
                    partToCheck.Multiplier = hit.multiplyBy;
                    partToCheck.BodyName = hit.boneName;
                    break;
                }
            }
        }
        Active(false);
    }
    public void Active(bool state)
    {
        foreach (Rigidbody bone in NPCBones)
        {
            Collider c = bone.GetComponent<Collider>();

            if (bone.useGravity != state)
            {
                c.isTrigger = !state;
                bone.isKinematic = !state;

                // Ajusta el modo de detección de colisiones
                if (state)
                {
                    bone.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                }
                else
                {
                    bone.collisionDetectionMode = CollisionDetectionMode.Discrete;
                }

                bone.useGravity = state;
                bone.velocity = NPCBody.velocity;
            }
        }

        NPCAnimator.enabled = !state;
        NPCBody.useGravity = !state;
        NPCBody.detectCollisions = !state;
        NPCBody.isKinematic = state;

        // Llamada a la configuración del modo de detección de colisiones del cuerpo principal (NPCBody)
        if (state)
        {
            NPCBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
        else
        {
            NPCBody.collisionDetectionMode = CollisionDetectionMode.Discrete;
        }
    }

}

[System.Serializable]
public class NPCHitMultiplier
{
    public String boneName = "Head";
    public float multiplyBy = 1;
}