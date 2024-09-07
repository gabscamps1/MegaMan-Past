using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Se estiver usando TextMeshPro

public class TextFadeEffect : MonoBehaviour
{
    public float fadeSpeed = 1.0f; // Velocidade do efeito de fade
    private TMP_Text textMeshPro; // Referência ao TextMeshPro
    private Color originalColor; // Cor original do texto
    private bool fadeIn = true; // Controle para alternar entre fade in e fade out

    void Start()
    {
        // Obter o componente TextMeshPro anexado ao GameObject
        textMeshPro = GetComponent<TMP_Text>();

        // Salvar a cor original do texto
        originalColor = textMeshPro.color;
    }

    void Update()
    {
        // Controla a direção do fade (fade in ou fade out)
        if (fadeIn)
        {
            // Aumentar a opacidade do texto
            textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Min(textMeshPro.color.a + (fadeSpeed * Time.deltaTime), 1.0f));

            // Quando a opacidade atingir 100%, inverte o sentido
            if (textMeshPro.color.a >= 1.0f)
            {
                fadeIn = false;
            }
        }
        else
        {
            // Diminuir a opacidade do texto
            textMeshPro.color = new Color(originalColor.r, originalColor.g, originalColor.b, Mathf.Max(textMeshPro.color.a - (fadeSpeed * Time.deltaTime), 0.0f));

            // Quando a opacidade atingir 0%, inverte o sentido
            if (textMeshPro.color.a <= 0.0f)
            {
                fadeIn = true;
            }
        }
    }
}
