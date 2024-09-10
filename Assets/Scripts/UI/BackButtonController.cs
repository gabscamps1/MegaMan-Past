using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BackButtonController : MonoBehaviour
{
    private Button backButton;
    private bool isMouseOver = false;

    void Start()
    {
        backButton = GetComponent<Button>();
        backButton.onClick.AddListener(BackToPreviousScene);
    }

    void Update()
    {
        // Verifica se o botão "ESC" foi pressionado
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackToPreviousScene();
        }
    }

    // Função para voltar para a cena anterior
    void BackToPreviousScene()
    {
        if (SceneManager.GetActiveScene().buildIndex > 0)
        {
            SceneManager.LoadScene("Menu");
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
    }

    // Detecta quando o mouse está sobre o botão
    public void OnPointerEnter()
    {
        isMouseOver = true;
    }

    // Detecta quando o mouse sai do botão
    public void OnPointerExit()
    {
        isMouseOver = false; 
    }

    void OnGUI()
    {
        if (isMouseOver)
        {
            // Desenha o retângulo sem preenchimento sobre o botão
            Rect rect = new Rect(backButton.transform.position.x, Screen.height - backButton.transform.position.y, backButton.GetComponent<RectTransform>().rect.width, backButton.GetComponent<RectTransform>().rect.height);
            GUI.color = Color.white;
            GUI.DrawTexture(rect, Texture2D.whiteTexture, ScaleMode.StretchToFill);
            GUI.Box(rect, GUIContent.none);
        }
    }
}


