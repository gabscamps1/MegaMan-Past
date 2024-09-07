using System.Collections;
using UnityEngine;
using TMPro;

public class WavyTextEffect : MonoBehaviour
{
    public TextMeshProUGUI textComponent;
    public float waveSpeed = 2f;
    public float waveHeight = 10f;

    void Start()
    {
        StartCoroutine(AnimateTextWave());
    }

    IEnumerator AnimateTextWave()
    {
        TMP_TextInfo textInfo = textComponent.textInfo;
        textComponent.ForceMeshUpdate();

        Vector3[] vertices;
        //Matrix4x4 matrix;

        while (true)
        {
            textComponent.ForceMeshUpdate(); // Atualiza a malha do texto
            textInfo = textComponent.textInfo;

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                if (!textInfo.characterInfo[i].isVisible)
                    continue;

                vertices = textInfo.meshInfo[textInfo.characterInfo[i].materialReferenceIndex].vertices;

                for (int j = 0; j < 4; j++)
                {
                    Vector3 offset = new Vector3(0, Mathf.Sin(Time.time * waveSpeed + textInfo.characterInfo[i].index) * waveHeight, 0);
                    vertices[textInfo.characterInfo[i].vertexIndex + j] += offset;
                }
            }

            // Atualiza a malha com os novos valores
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                textComponent.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
            }

            yield return null;
        }
    }
}
