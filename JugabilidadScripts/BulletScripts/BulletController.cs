using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Transform bulletTr;
    Rigidbody bullerRb;
    private float time = 0f;// Tiempo del juego
    //Valores de la bala:
    [Header("Configuración de Bala")]
    public float bulletPower = 0f;// Fuerza de disparo
    public float lifeTime = 2f;// Tiempo de estancia de la bala
    public float bulletDamage = 1;// Daño de la bala

    //Valores asignados a la bala
    Vector3 lastBulletPos;//ultima poscision
    public LayerMask hitboxMask;// mascara del Hitbox


    void Start()
    {
        bulletTr = GetComponent<Transform>();
        bullerRb = GetComponent<Rigidbody>();

        bullerRb.velocity = this.transform.forward * bulletPower;

        hitboxMask = LayerMask.NameToLayer("Hitbox");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        time += Time.deltaTime;
        DetectCollision();

        //Eliminacion de las balas
        if (time >= lifeTime)
        {
            Destroy(this.gameObject);
        }
    }

    public void DetectCollision()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, bulletPower * Time.fixedDeltaTime))
        {
             
            GameObject targetObject = hit.collider.gameObject;

            NPCBodyPartHitCheck nPCBodyPart = targetObject.GetComponent<NPCBodyPartHitCheck>();
            if (nPCBodyPart != null)
            {
                nPCBodyPart.TakeHit(bulletDamage);
            }

            Destroy(gameObject);
        }

    }
}