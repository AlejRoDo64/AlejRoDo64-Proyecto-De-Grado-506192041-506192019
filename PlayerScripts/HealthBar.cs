using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//para trabajar con imagenes

public class HealthBar : MonoBehaviour
{
    public PController thePlayer;

    public Image lifebarFill;//llenado de barra de vida

    private RectTransform rectTransform;//cantidad de vida

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        float healthPercentage = thePlayer.currentHealt / thePlayer.maxHealth;//Total de la vida /maxima cantidad de vida

        rectTransform.localScale = new Vector3(healthPercentage, 1, 1);

        lifebarFill.color = Color.Lerp(Color.red, Color.green, healthPercentage);
    }
}

