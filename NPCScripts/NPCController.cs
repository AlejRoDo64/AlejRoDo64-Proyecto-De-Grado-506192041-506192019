using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPCController : MonoBehaviour
{
    public bool NPC = true;
    //Personaje
    Transform NPCTr;
    Rigidbody NPCRb;
    Animator NPCAnimator;
    NPCRagdollController NPCRagdoll;
    //Variables Vida
    public float maxHealth = 100f;
    public float currentHealt;
    //Item Spawn If Death
    public GameObject itemSlot;
    public GameObject itemPrefab;
    //Navegacion
    private AI_Controller iA;
    //Inventario del jugador
    public Inventory inventory;

    // Start is called before the first frame update
    void Start()
    {
        NPCTr = this.transform; //poscicion
        NPCRb = GetComponent<Rigidbody>(); //Fisicas
        NPCAnimator = GetComponentInChildren<Animator>(); //Control de animaciones
        NPCRagdoll = GetComponentInChildren<NPCRagdollController>();//Huesos del muñeco

        iA = GetComponentInChildren<AI_Controller>();//IA del modelo

        currentHealt = maxHealth;//Vida de personaje
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
    }
    public void TakeDamage(float damage)
    {
        currentHealt -= damage;
        if (currentHealt <= 0f)
        {
            if (NPC)//si aun esta activo
            {
                GameObject instatiatedItem = Instantiate(itemPrefab, itemSlot.transform.position, itemSlot.transform.rotation);
                //Cambia el nombre del objeto y borra el layer del objeto NPC (enemy o ally)
                gameObject.name = "Killed";
                gameObject.layer = 0;
                // Obtener el primer hijo del GameObject actual
                Transform primerHijo = transform.GetChild(0);
                // Acceder al GameObject del primer hijo
                GameObject hijoGameObject = primerHijo.gameObject;
                //Con el GameObject hijo cambia el nombre y quita el layer del objeto interno del NPC
                hijoGameObject.name = "Killed";
                hijoGameObject.layer = 0;
                itemSlot.SetActive(false);
                NPCAnimator.enabled = false;
                NPCRagdoll.Active(true);
                iA.agent.enabled = false;
                iA.enabled = false;
                inventory.Kills += 1;
                NPC = false;
            }
        }
    }
}
