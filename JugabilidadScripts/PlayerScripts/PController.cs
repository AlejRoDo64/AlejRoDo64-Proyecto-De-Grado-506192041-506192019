using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PController : MonoBehaviour
{
    public bool Player = true;
    public bool Active = true;
    //Personaje
    Transform playerTr;
    Rigidbody playerRb;
    Animator playerAnim;
    RagdollController playerRagdoll;
    public float playerSpeed = 0f;
    //Variables Vida
    [HideInInspector]
    public float maxHealth = 100f;
    public float currentHealt;
    public bool hasMusket = false;
    private Vector2 newDirection;
    //Camara
    public Transform cameraAxis;//Crear como GameObject Vacio
    public Transform cameraTrack;//Crear como GameObject dentreo de camera Axis
    public Transform cameraWeaponTrack;
    private Transform theCamera;
    //Rotacion de camara
    private float rotY = 0f;
    private float rotX = 0f;
    public float camRotSpeed = 200f;
    public float minAngle = -45f;
    public float maxAngle = 45f;
    public float cameraSpeed = 200f;
    //Items
    public GameObject nearItem;
    public GameObject itemPrefab;
    public GameObject MusketPrefab;
    public Transform itemSlot;
    public GameObject itemSlotObj;
    //Canvas
    public GameObject weaponSight;
    public GameObject unWeaponSight;
    public GameObject CanvasInteractive;
    public GameObject CanvasPerder;

    // Start is called before the first frame update
    void Start()
    {
        playerTr = this.transform; //poscicion
        playerRb = GetComponent<Rigidbody>(); //Fisicas

        playerAnim = GetComponentInChildren<Animator>(); //Control de animaciones
        playerRagdoll = GetComponentInChildren<RagdollController>();//Huesos del muñeco

        theCamera = Camera.main.transform;// camara

        //esconde el cursor
        Cursor.lockState = CursorLockMode.Locked;

        currentHealt = maxHealth;//Llenar Vida de personaje
        if (!Player)
        {
            Active = false;
        }
        else
        {
            Active = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Player)
        {
            CameraLogic();
            MoveLogic();
        }
        if (!Active)
        {
            return;
        }

        ItemLogic();
        AnimLogic();

        if (Input.GetKeyDown(KeyCode.Y))
        {
            TakeDamage(10f);
        }

    }

    public void MoveLogic()
    {
        Vector3 direction = playerRb.velocity;

        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");
        float theTime = Time.deltaTime;

        newDirection = new Vector2(moveX, moveZ);

        Vector3 side = playerSpeed * moveX * theTime * playerTr.right;
        Vector3 forward = playerSpeed * moveZ * theTime * playerTr.forward;

        Vector3 endDirection = side + forward;

        endDirection.y = playerRb.velocity.y;

        playerRb.velocity = endDirection;
    }
    public void CameraLogic()
    {
        //Movimiento del mouse
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        //el tiempo para adaptar a cualquier CPU
        float theTime = Time.deltaTime;
        //Velocidad de rotacion de la camara
        rotY += mouseY * theTime * camRotSpeed;
        rotX = mouseX * theTime * camRotSpeed;
        //Para que rote con la camara
        playerTr.Rotate(0, rotX, 0);
        //Para valor minimo y valor maximo a rotY
        rotY = Mathf.Clamp(rotY, minAngle, maxAngle);
        //Rotacion Local de la camara sera rotY
        Quaternion localRotation = Quaternion.Euler(-rotY, 0, 0);
        cameraAxis.localRotation = localRotation;

        if (hasMusket)
        {
            CanvasInteractive.SetActive(false);
            //Activa vista arma y desactiva vista normal
            cameraTrack.gameObject.SetActive(false);
            cameraWeaponTrack.gameObject.SetActive(true);
            //Activa la mira
            unWeaponSight.gameObject.SetActive(false);
            weaponSight.gameObject.SetActive(true);
            //Posicion y rotacion de la camara
            theCamera.position = Vector3.Lerp(theCamera.position, cameraWeaponTrack.position, cameraSpeed * theTime);
            theCamera.rotation = Quaternion.Lerp(theCamera.rotation, cameraWeaponTrack.rotation, cameraSpeed * theTime);
        }
        else
        {
            //Desactiva vista arma y Activa vista normal
            cameraWeaponTrack.gameObject.SetActive(false);
            cameraTrack.gameObject.SetActive(true);
            //Desctiva la mira
            weaponSight.gameObject.SetActive(false);
            unWeaponSight.gameObject.SetActive(true);
            //Posicion y rotacion de la camara
            theCamera.position = Vector3.Lerp(theCamera.position, cameraTrack.position, cameraSpeed * theTime);
            theCamera.rotation = Quaternion.Lerp(theCamera.rotation, cameraTrack.rotation, cameraSpeed * theTime);
        }
    }

    public void ItemLogic()
    {
        if (itemSlotObj != null && Input.GetKeyDown(KeyCode.E))
        {
            UnequipMusket();
        }
        //Condicion para equipar arma hay objeto cerca o hay algun objeto en mano y se presiona E
        else if ((nearItem != null || itemSlotObj != null) && Input.GetKeyDown(KeyCode.E))
        {
            EquipMusket();
        }
    }

    public void UnequipMusket()
    {
        // Instanciar un nuevo prefab al desequipar
        Instantiate(MusketPrefab, itemSlot.position, itemSlot.rotation);
        hasMusket = false;
        Debug.Log(itemSlot);
        Destroy(itemSlotObj.gameObject);
        itemSlotObj = null;
    }

    
    public void EquipMusket()
    {
        GameObject instatiatedItem = Instantiate(itemPrefab, itemSlot.position, itemSlot.rotation);
        Destroy(nearItem?.gameObject);
        instatiatedItem.transform.parent = itemSlot;
        hasMusket = true;
        nearItem = null;
    }


    void OnTriggerEnter(Collider other)//al hacer collider 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))//con el objeto dropped  y si no tiene un arma
        {
            //Debug.Log("Hay un Item Cerca!" + other.gameObject.tag);
            CanvasInteractive.SetActive(true);
            nearItem = other.gameObject;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("ItemE"))//con el objeto equipado
        {
            //Debug.Log("Hay un Item equipado");
            itemSlotObj = other.gameObject;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Interact"))
        {
            Debug.Log("Objeto interactivo cerca");
            CanvasInteractive.SetActive(true);
        }
    }
    void OnTriggerExit(Collider other)//al salir del collider con el objeto
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Item"))//con el objeto dropped
        {
            CanvasInteractive.SetActive(false);
            Debug.Log("Ya no hay un Item Cerca!");
            nearItem = null;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("ItemE"))//con el objeto equipado
        {
            Debug.Log("No hay un Item equipado");
            itemSlotObj = null;
        }
        if (other.gameObject.layer == LayerMask.NameToLayer("Interact"))
        {
            Debug.Log("Objeto interactivo Lejos");
            CanvasInteractive.SetActive(false);
        }
    }

    public void AnimLogic()
    {
        // playerAnim.SetBool("ground", OnGround);
        // playerAnim.SetBool("Jumping", Jumping);

        //toma la entrada para activar la animacion
        playerAnim.SetFloat("X", newDirection.x);
        playerAnim.SetFloat("Y", newDirection.y);
        //
        playerAnim.SetBool("holdMusket", hasMusket);
        //Activa la animacion de sostener arma
        if (hasMusket)
        {
            playerAnim.SetLayerWeight(1, 1);
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealt -= damage;

        if (currentHealt <= 0f)
        {
            CanvasPerder.SetActive(true);
            playerRagdoll.Active(true);
            Active = false;

        }
    }
}
