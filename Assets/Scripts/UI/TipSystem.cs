using UnityEngine;
using TMPro;

public class TipSystem : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI tipText;

    private string[] tips = {
        "If you want to jump, use the 'Space' button.",
        "You're Nin, an extraordinary ninja.",
        "Use 'LMB' to shoot Shurikens.",
        "Beware of your surroudings!",
        "You can wall jump by pressing 'Space' on the nearest wall."
    };

    void Start()
    {
        ShowRandomTip();
    }

    void ShowRandomTip()
    {
        int randomIndex = Random.Range(0, tips.Length);  // Escolhe um índice aleatório
        titleText.text = "Gameplay Tip #" + (randomIndex + 1);  // Define o título da dica
        tipText.text = tips[randomIndex];  // Exibe o texto da dica
    }
}
