using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NivelesController : MonoBehaviour
{
    public static NivelesController instancia;//Singleton solo un controlador de niveles
    public Button[] botonesNiveles;
    public int desbloquearNiveles;

    private void Awake()// solo una instancia
    {
        if (instancia == true)
        {
            instancia = this;
        }
    }

    void Start()
    {
        if (botonesNiveles.Length > 0)
        {
            for (int i = 0; i < botonesNiveles.Length; i++)
            {
                botonesNiveles[i].interactable = false;
            }

            for (int i = 0; i < PlayerPrefs.GetInt("nivelesDesbloqueados", 1); i++)
            {
                botonesNiveles[i].interactable = true;
            }
        }
    }

    public void AumentarNiveles()
    {
        if (desbloquearNiveles > PlayerPrefs.GetInt("nivelesDesbloqueados", 1))
        {
            PlayerPrefs.SetInt("nivelesDesbloqueados", desbloquearNiveles);
        }
    }
    public void BloquearTodosLosNiveles()
    {
        PlayerPrefs.SetInt("nivelesDesbloqueados", 1); // Restablece el número de niveles desbloqueados al valor inicial
        ActualizarInteractividadBotones(); // Actualiza la interactividad de los botones de niveles
    }

    private void ActualizarInteractividadBotones()
    {
        for (int i = 0; i < botonesNiveles.Length; i++)
        {
            botonesNiveles[i].interactable = (i < PlayerPrefs.GetInt("nivelesDesbloqueados", 1));
        }
    }
}
