using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro; // Se estiver usando TextMeshPro

public class MyLoading : MonoBehaviour
{
    AsyncOperation operation;
    static string level;
    public Slider slider;
    public TMP_Text loadingText; // Texto para "Carregando..."
    public TMP_Text pressAnyKeyText; // Texto para "Pressione Espaço"

    // Start is called before the first frame update
    void Start()
    {
        // Inicializa o texto com "Carregando..." e esconde o "Pressione Espaço"
        loadingText.text = "Loading...";
        pressAnyKeyText.gameObject.SetActive(false); // Esconder o texto de "Pressione Espaço"

        // Carrega a cena de forma assíncrona, mas não a ativa
        operation = SceneManager.LoadSceneAsync(level);
        operation.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Atualiza o valor do slider para mostrar o progresso do carregamento
        slider.value = Mathf.Lerp(slider.value, operation.progress, Time.deltaTime * 5);

        // Quando o carregamento estiver completo (progress >= 0.9)
        if (operation.progress >= 0.9f)
        {
            // Esconde o texto "Carregando..." e exibe o "Pressione Espaço"
            loadingText.gameObject.SetActive(false);
            pressAnyKeyText.gameObject.SetActive(true);

            // Espera o jogador pressionar a barra de espaço para ativar a cena
            if (Input.GetKeyDown(KeyCode.Space))
            {
                operation.allowSceneActivation = true; // Ativa a cena
            }
        }
    }

    /// <summary>
    /// Chame esta função para carregar um nível em vez de usar SceneManager
    /// </summary>
    /// <param name="nextlevel"></param>
    public static void LoadLevel(string nextlevel)
    {
        level = nextlevel;
        SceneManager.LoadScene("Loading");
    }
}
