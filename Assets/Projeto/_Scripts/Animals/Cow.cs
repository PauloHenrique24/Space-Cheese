using System.Collections;
using UnityEngine;

public class Cow : MonoBehaviour
{
    private Animator anim;
    private bool abduzindo;
    private float timer;
    private float duracao;

    private Vector3 posFinal;
    private Vector3 posInicial;

    public bool mentira;
    public bool boi;
    public bool bomb;

    private bool _muw;
    

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Abduzir(Vector3 posAbd, float duracao)
    {
        
        anim.SetTrigger("abduzir");
            
        posFinal = posAbd;
        posInicial = transform.position;

        this.duracao = duracao;

        FindFirstObjectByType<UFOMoviment>().bomb = bomb;

        abduzindo = true;
    }

    void LateUpdate()
    {
        if (abduzindo)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / duracao);

            transform.localScale = Vector3.Lerp(
                new Vector3(1, 1, 1),
                new Vector3(0, 0, 0),
                t
            );
            transform.localRotation = Quaternion.Lerp(
                Quaternion.Euler(0, 0, 0),
                Quaternion.Euler(0, 0, 180),
                t
            );

            transform.position = Vector3.Lerp(
                posInicial,
                posFinal,
                t
            );

            if (Vector3.Distance(transform.position, posFinal) < 0.1f) EndAbducao();

            if(!mentira)
                if (!_muw) StartCoroutine(MUW());
        }
    }

    IEnumerator MUW()
    {
        _muw = true;
        Vector3 pos = new(Random.Range(transform.position.x - 1f, transform.position.x + 1f), Random.Range(transform.position.y + .4f, transform.position.y - .4f));
        Instantiate(CowManager.cow.prefab_muw, pos, Quaternion.identity);

        CowManager.cow.CowMuwSound();

        yield return new WaitForSeconds(1f);
        _muw = false;
    }

    public void Abducao()
    {
        if (!mentira && !boi)
            CowManager.cow.AddMilk(Random.Range(25f, 30f));

        if (bomb)
            CowManager.cow.Explosion();
    }

    public void EndAbducao()
    {
        Abducao();
        Destroy(gameObject);
    }
}
