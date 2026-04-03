using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UFOMoviment : MonoBehaviour
{
    [Header("Movimentação")]
    [SerializeField] private float speed = 4f;
    [SerializeField] private float derrapagem = 5f;

    [Header("Oscilação Espacial")]
    [SerializeField] private float amplitudeRotacao = 2f;
    [SerializeField] private float frequenciaRotacao = 2f;

    private Vector2 Movimento;
    private Vector2 MovimentoSuavizado;

    private float tempo;
    private Animator anim;

    [Header("Abdução")]
    [SerializeField] private Transform positionAbducao;
    [SerializeField] private float timerAbducao;

    private bool abduzindo;
    private bool abduzindoCow;

    private AudioSource abducaoSound;

    [Header("Dead")]
    [SerializeField] private GameObject explosion_efx;
    [SerializeField] private Color destroyColor;

    public bool bomb;

    [SerializeField] private Transform spawn_alien;

    void Start()
    {
        anim = GetComponent<Animator>();
        abducaoSound = GetComponent<AudioSource>();

        bomb = false;

        transform.position = spawn_alien.position;
    }

    void FixedUpdate()
    {
        if (!abduzindo)
            Moviment();
        else
        {
            MovimentoSuavizado = Vector2.zero;
            Movimento = Vector2.zero;
        }

        Oscilar();
    }

    public void Moviment()
    {
        MovimentoSuavizado = Vector2.Lerp(MovimentoSuavizado, Movimento, derrapagem * Time.fixedDeltaTime);
        transform.position += (Vector3)MovimentoSuavizado * speed * Time.fixedDeltaTime;
    }

    public void SetMovimento(InputAction.CallbackContext value)
    {
        Movimento = value.ReadValue<Vector2>();
    }

    public void SetAbduzir(InputAction.CallbackContext value)
    {
        Abduzir();
    }

    private void Oscilar()
    {
        tempo += Time.fixedDeltaTime;
        float rotacao = Mathf.Sin(tempo * frequenciaRotacao) * amplitudeRotacao;
        transform.rotation = Quaternion.Euler(0f, 0f, rotacao);
    }

    // Abduzir

    private void Abduzir()
    {
        if (!abduzindo && CowManager.cow.feixe_qtd > 0)
        {
            anim.SetBool("Abduzir", true);
            abducaoSound.Play();
            abduzindo = true;

            CowManager.cow.DecrementFeixe();

            StartCoroutine(Abducao());
        }
    }

    IEnumerator Abducao()
    {
        yield return new WaitForSeconds(timerAbducao);
        anim.SetBool("Abduzir", false);

        abducaoSound.Stop();

        abduzindo = false;
        abduzindoCow = false;

        if (bomb)
        {
            Explosion();
        }
    }

    void Explosion()
    {
        GetComponent<SpriteRenderer>().color = destroyColor;
        transform.GetChild(0).gameObject.SetActive(false);
        var pos = new Vector3(transform.position.x, transform.position.y + 1f);
        Instantiate(explosion_efx, pos, Quaternion.identity);

        StartCoroutine(Restart());
    }

    public void SpawnAlien() => transform.position = spawn_alien.position;

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Cow") && abduzindo && !abduzindoCow)
        {
            collision.GetComponent<Cow>().Abduzir(positionAbducao.position, timerAbducao);
            abduzindoCow = true;
        }
    }
}
