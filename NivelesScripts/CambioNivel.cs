using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CambioNivel : MonoBehaviour
{
    //Codigo Para Cambiar de escena
    public void CambiarEscena(string nombre)
    {
        if (NivelesController.instancia!= null)
        {
            NivelesController.instancia.AumentarNiveles();//Llama al aumentar niveles
        }

        SceneManager.LoadScene(nombre);
    }
    public void Salir()
    {
        Application.Quit();
    }
}
