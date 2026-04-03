using UnityEngine;

public class CameraSway : MonoBehaviour
{
    [Header("Configurações de Balanço")]
    [SerializeField] private float amplitude = 0.05f; // Tamanho do movimento
    [SerializeField] private float frequencia = 1f;   // Velocidade da oscilação

    private Vector3 posicaoInicial;
    private float tempo;

    void Start()
    {
        posicaoInicial = transform.localPosition;
    }

    void Update()
    {
        tempo += Time.deltaTime;

        // Movimento senoidal suave nos eixos X e Y
        float offsetX = Mathf.Sin(tempo * frequencia) * amplitude;
        float offsetY = Mathf.Cos(tempo * frequencia * 0.8f) * amplitude * 0.5f;

        transform.localPosition = posicaoInicial + new Vector3(offsetX, offsetY, 0f);
    }
}
