using UnityEngine;

public class FazendeiroPatrulha : MonoBehaviour
{
    [Header("Limites da Patrulha")]
    public float minX = -5f;
    public float maxX = 5f;
    public float minY = -3f;
    public float maxY = 3f;

    [Header("Movimentação")]
    public float speed = 2f;

    private Vector2 nextTarget;
    private Animator animator;

    private bool isFacing;

    [Header("Luz")]
    [SerializeField] private GameObject side;
    [SerializeField] private GameObject up;
    [SerializeField] private GameObject down;

    void Start()
    {
        animator = GetComponent<Animator>();
        EscolherNovoDestino();
    }

    void Update()
    {
        Vector2 pos = transform.position;
        Vector2 dir = (nextTarget - pos).normalized;

        // Atualiza a posição
        transform.position = Vector2.MoveTowards(pos, nextTarget, speed * Time.deltaTime);

        // Atualiza o Blend Tree
        animator.SetFloat("X", dir.x);
        animator.SetFloat("Y", dir.y);

        if (dir.x < 0 && !isFacing)
        {
            Flip();
        }
        else if (dir.x > 0 && isFacing)
        {
            Flip();
        }
 
        // Verifica se chegou ao destino
        if (Vector2.Distance(pos, nextTarget) < 0.1f)
        {
            EscolherNovoDestino();
        }
    }

    void Flip()
    {
        isFacing = !isFacing;
        var scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    void EscolherNovoDestino()
    {
        int direcao = Random.Range(0, 4);
        Vector2 atual = transform.position;

        switch (direcao)
        {
            case 0: // Direita
                side.SetActive(true);
                up.SetActive(false);
                down.SetActive(false);
                nextTarget = new Vector2(Mathf.Min(atual.x + 1, maxX), atual.y);
                break;
            case 1: // Esquerda
                side.SetActive(true);
                up.SetActive(false);
                down.SetActive(false);
                nextTarget = new Vector2(Mathf.Max(atual.x - 1, minX), atual.y);
                break;
            case 2: // Cima
                side.SetActive(false);
                up.SetActive(true);
                down.SetActive(false);
                nextTarget = new Vector2(atual.x, Mathf.Min(atual.y + 1, maxY));
                break;
            case 3: // Baixo
                side.SetActive(false);
                up.SetActive(false);
                down.SetActive(true);
                nextTarget = new Vector2(atual.x, Mathf.Max(atual.y - 1, minY));
                break;
        }
    }
}
