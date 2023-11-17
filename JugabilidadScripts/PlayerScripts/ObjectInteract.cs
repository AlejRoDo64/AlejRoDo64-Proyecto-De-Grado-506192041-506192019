using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInteract : MonoBehaviour
{
    public Inventory inventory;
    public GameObject Player;
    public GameObject canvas;
    public ObjectInteract nextObjectScript;  // Referencia al siguiente script
    public bool hasCounted = false;  // Bandera para rastrear si ya se ha contado

    void Start()
    {
        inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        print(nextObjectScript);
    }

    void Update()
    {
        interaction();
    }

    void interaction()
    {
        if (Player != null && Input.GetKeyDown(KeyCode.E))
        {
            canvas.SetActive(true);
            Time.timeScale = 0;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            hasCounted = true;
        }
        if (hasCounted && Input.GetKeyDown(KeyCode.Space))
        {
            Time.timeScale = 1;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            canvas.SetActive(false);
            if (hasCounted)
            {
                //Suma el inventario
                inventory.amount += 1;
                print(inventory.amount);
            }
            Destroy(gameObject);
            ActivateNextObject();
        }
    }

    // Método para activar el objeto siguiente
    void ActivateNextObject()
    {
        // Verifica si la referencia al siguiente script está asignada antes de intentar activar el siguiente objeto
        if (nextObjectScript != null)
        {
            nextObjectScript.Activate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player = other.gameObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            Player = null;
        }
    }

    // Método para activar este objeto (puedes personalizar esto según tus necesidades)
    public void Activate()
    {
        gameObject.SetActive(true);
    }
}