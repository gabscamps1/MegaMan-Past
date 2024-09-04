using UnityEngine;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text[] menuItems; // Array de itens do menu
    public Image highlightImage; // Imagem do polígono de seleção
    public float moveSpeed = 10f; // Velocidade de movimento do polígono

    public float horizontalOffset = 0f; // Offset horizontal da imagem de seleção

    public Font defaultFont; // Fonte padrão dos itens do menu
    public Font highlightedFont; // Fonte dos itens do menu quando selecionado

    public Text selectedOptionText; // Texto que exibirá a opção selecionada

    public string customText = "Selected: "; // Texto personalizado para exibir com a opção selecionada

    private int currentIndex = 0;

    void Start()
    {
        UpdateHighlight();
        UpdateSelectedOptionText(); // Inicializa o texto com a opção selecionada
    }

    void Update()
    {
        HandleKeyboardInput();
        HandleMouseInput();
    }

    private void HandleKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            ChangeSelection(1);
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ChangeSelection(-1);
        }
    }

    private void HandleMouseInput()
    {
        // Verifica se o mouse está sobre um item do menu
        for (int i = 0; i < menuItems.Length; i++)
        {
            RectTransform rectTransform = menuItems[i].GetComponent<RectTransform>();
            if (RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
            {
                //if (Input.GetMouseButtonDown(0))
                //{
                    ChangeSelection(i - currentIndex);
                //}
            }
        }
    }

    private void ChangeSelection(int direction)
    {
        currentIndex = Mathf.Clamp(currentIndex + direction, 0, menuItems.Length - 1);
        UpdateHighlight();
        UpdateSelectedOptionText(); // Atualiza o texto com a opção selecionada
    }

    private void UpdateHighlight()
    {
        for (int i = 0; i < menuItems.Length; i++)
        {
            if (i == currentIndex)
            {
                menuItems[i].color = Color.black; // Destaca o item selecionado
                menuItems[i].font = highlightedFont; // Altera a fonte para a destacada
                RectTransform itemRect = menuItems[i].GetComponent<RectTransform>();
                Vector3 newPos = itemRect.position + new Vector3(horizontalOffset, 0, 0); // Aplica o offset horizontal
                highlightImage.transform.position = Vector3.Lerp(highlightImage.transform.position, newPos, moveSpeed * Time.deltaTime);
            }
            else
            {
                menuItems[i].color = Color.white; // Reseta a cor do item não selecionado
                menuItems[i].font = defaultFont; // Altera a fonte para a padrão
            }
        }
    }

    private void UpdateSelectedOptionText()
    {
        if (selectedOptionText != null && menuItems.Length > 0)
        {
            if (currentIndex == 0)
            {
                selectedOptionText.text = "Start the game.";
            }
            else if (currentIndex == 1)
            {
                selectedOptionText.text = "Change game settings.";
            }
            else if (currentIndex == 2)
            {
                selectedOptionText.text = "See the game credits.";
            }
            else if (currentIndex == 3)
            {
                selectedOptionText.text = "Close the game.";
            }
            else {
                selectedOptionText.text = "";
            }

            //selectedOptionText.text = customText + menuItems[currentIndex].text; // Atualiza o texto com o item selecionado e o texto personalizado
        }
    }
}
