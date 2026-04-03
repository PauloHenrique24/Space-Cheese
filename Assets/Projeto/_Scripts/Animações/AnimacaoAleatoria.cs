using System.Collections;
using UnityEngine;

public class AnimacaoAleatoria : MonoBehaviour
{
    private Animator animator;
    public bool atrasar, velocidade;


    IEnumerator Start()
    {
        animator = GetComponent<Animator>();
        float atraso = 0f;

        if (atrasar)
            atraso = Random.Range(0f, 0.5f);

        yield return new WaitForSeconds(atraso);

        if(velocidade)
            animator.speed = Random.Range(0.8f, 1.2f);
            
        animator.Play("Idle");
    }

}
