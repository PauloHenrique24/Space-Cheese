using System;
using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogoManager : MonoBehaviour
{
    [Header("Dialogo")]
    public TextMeshProUGUI dialogoText;
    public Image personagemImage;
    public GameObject pular;

    public float delayNormal;
    public float delaySpeed;

    private float delay;

    [Space]
    public TransitionSettings transition;

    [Header("Frases")]
    public List<DialogoFrase> frases = new();
    private int fraseAtual = 0;

    private bool next = true;

    void Start()
    {
        next = true;
        StartDialogo();
    }

    public void OnNext()
    {
        if (next)
        {
            StartDialogo();
        }
        else
        {
            delay = delaySpeed;
        }
    }

    public void StartDialogo()
    {
        if (next)
        {
            pular.SetActive(false);

            personagemImage.sprite = frases[fraseAtual].personagemImage;

            StartCoroutine(Dialogo());
        }
    }

    IEnumerator Dialogo()
    {
        dialogoText.text = "";
        next = false;
        delay = delayNormal;

        foreach (Char c in frases[fraseAtual].textoFrase)
        {
            dialogoText.text += c;
            yield return new WaitForSeconds(delay);
        }
        fraseAtual++;

        if (fraseAtual >= frases.Count)
        {
            yield return new WaitForSeconds(1f);
            print("Acabou!");
            Game();
            PlayerPrefs.SetInt("Dialogo", 1);
        }
        else
        {
            pular.SetActive(true);
            next = true;
        }
    }

    public void Game(){
        TransitionManager.Instance().Transition("Game", transition, 0);
    }
}

[Serializable]
public class DialogoFrase
{
    [TextArea]
    public string textoFrase;
    public Sprite personagemImage;
}
