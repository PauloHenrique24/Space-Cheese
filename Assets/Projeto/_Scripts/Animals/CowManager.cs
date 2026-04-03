using System.Collections;
using System.Collections.Generic;
using EasyTransition;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CowManager : MonoBehaviour
{
    public static CowManager cow { get; set; }

    [Header("Milk")]
    [SerializeField] private Image milk_fill;
    [SerializeField] private float milk_speed;

    [HideInInspector] public float milk;
    private float milkFull = 100f;

    [Header("Feixe Interface")]
    [SerializeField] private Image feixe_img;
    [SerializeField] private TextMeshProUGUI feixe_txt;
    public int feixe_qtd = 4;

    [Header("Transition")]
    public TransitionSettings transition;
    public float startDelay;

    void Awake()
    {
        cow = !cow ? this : cow;
        myAudio = GetComponent<AudioSource>();

        feixe_qtd = 4;
    }

    [Header("MUW")]
    public GameObject prefab_muw;
    public List<AudioClip> soundsMuw = new();

    private AudioSource myAudio;

    [SerializeField] private AudioClip explosion_sfx;

    public void CowMuwSound()
    {
        if (!myAudio.isPlaying)
        {
            myAudio.clip = soundsMuw[Random.Range(0, soundsMuw.Count)];
            myAudio.Play();
        }
    }

    void LateUpdate()
    {
        milk = Mathf.Clamp(milk, 0, milkFull);
    }

    public void AddMilk(float amount)
    {
        milk += amount;
        float targetFill = milk / milkFull;

        StartCoroutine(AnimateFill(targetFill));
    }

    public void Explosion()
    {
        myAudio.clip = explosion_sfx;
        myAudio.Play();
    }

    private IEnumerator AnimateFill(float target)
    {
        while (Mathf.Abs(milk_fill.fillAmount - target) > 0.01f)
        {
            milk_fill.fillAmount = Mathf.Lerp(milk_fill.fillAmount, target, milk_speed * Time.deltaTime);
            yield return null;
        }

        milk_fill.fillAmount = target; // Garante valor final exato
    }

    public void DecrementFeixe()
    {
        feixe_qtd--;
        ViewFeixe();

        if (feixe_qtd <= 0) NextLevel();
    }

    void ViewFeixe()
    {
        feixe_txt.text = "x" + feixe_qtd;
        feixe_img.fillAmount = (float)feixe_qtd / 4;
    }

    public void NextLevel()
    {
        StartCoroutine(delayNextLevel());
    }

    IEnumerator delayNextLevel()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine(NextLevelAuto());
    }

    public IEnumerator NextLevelAuto()
    {
        TransitionManager.Instance().Transition(transition, startDelay);

        yield return new WaitForSeconds(1f);

        StopAllCoroutines();
        milk = 0;
        milk_fill.fillAmount = 0;

        FindFirstObjectByType<UFOMoviment>().SpawnAlien();
        LevelsGenetor.current.NextLevel();

        feixe_qtd = 4;
        ViewFeixe();
    }
}
