using UnityEngine;

public class CameraSwing : MonoBehaviour
{
    public float amplitude = 0.5f; // Amplitude
    public float frequency = 1f;   // Frequência

    private Vector3 startPosition;

    void Start(){
        startPosition = transform.position; //Posição inicial
    }

    void Update(){
        // Calcula o deslocamento do objeto baseado no tempo
        float xOffset = amplitude * Mathf.Sin(Time.time * frequency);

        // Aplica o deslocamento à posição inicial do objeto
        transform.position = new Vector3(startPosition.x + xOffset, startPosition.y, startPosition.z);
    }
}
