using UnityEngine;

public class ArvoreOscilante : MonoBehaviour
{
    [Header("Oscilação do Vento")]
    [SerializeField] private float intensidade = 5f;       // Quanto gira para os lados
    [SerializeField] private float velocidade = 2f;        // Velocidade da oscilação
    [SerializeField] private float atrasoAleatorio = 0.5f; // Variação para parecer natural

    private float tempoOffset;

    void Start()
    {
        // Cada árvore começa em um ponto diferente da animação
        tempoOffset = Random.Range(0f, atrasoAleatorio);
    }

    void Update()
    {
        float rotZ = Mathf.Sin((Time.time + tempoOffset) * velocidade) * intensidade;
        transform.rotation = Quaternion.Euler(0f, 0f, rotZ);
    }
}
