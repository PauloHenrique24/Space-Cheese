using System.Collections.Generic;
using EasyTransition;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelsGenetor : MonoBehaviour
{
    public static LevelsGenetor current;

    void Awake()
    {
        current = !current ? this : current;
    }

    [Header("Niveis")]
    [SerializeField] private int nivel = 0;
    [SerializeField] private int maxNivel = 7;

    [Header("Geração Procedural")]
    [SerializeField] private Vector2 top;
    [SerializeField] private Vector2 down;

    [SerializeField] private int min;
    [SerializeField] private int max;

    [SerializeField] private Transform parent_objs;
    [SerializeField] private Transform parent_animals;

    public List<GameObject> objs = new();

    [Header("Dia 1")]
    public List<GameObject> vacas = new();
    public List<GameObject> bois = new();

    [Header("Dia 2")]
    public List<GameObject> vacaMentira = new();

    [Header("Dia 3")]
    [SerializeField] private GameObject cameras_obj;
    [SerializeField] private GameObject fazendeiroParado;

    [Header("Dia 4")]
    [SerializeField] private List<GameObject> vacaExplosiva = new();
    [SerializeField] private GameObject fazendeiroPatrulha;

    [SerializeField] private TextMeshProUGUI level_txt;

    [Header("Pause")]
    [SerializeField] private GameObject pause_interface;
    [SerializeField] private TransitionSettings transition;
    private bool pause;

    void Start()
    {
        nivel = 1;
        GeneratorLevels();
        GenerateMap();

        Time.timeScale = 1f;
    }

    public void SetPause(InputAction.CallbackContext value)
    {
        Pause();
    }

    void Pause()
    {
        pause = !pause;
        pause_interface.SetActive(pause);
        Time.timeScale = pause ? 0 : 1;
    }

    public void Continuar()
    {
        Time.timeScale = 1;
        pause = false;
        pause_interface.SetActive(false);
    }

    public void VoltarAoMenu()
    {
        Time.timeScale = 1f;
        TransitionManager.Instance().Transition("Menu", transition, 0);
    }

    public void NextLevel()
    {
        if (nivel < maxNivel)
        {
            nivel++;
            LevelTxt();
            GeneratorLevels();
        }
        else
        {
            VoltarAoMenu();
        }
    }

    private List<Vector2> occupiedPositions = new();

    public void GeneratorLevels()
    {
        if (nivel == 1)
        {
            Level1();
        }
        else if (nivel == 2)
        {
            Level2();
        }
        else if (nivel == 3)
        {
            Level3();
        }
        else if (nivel == 4)
        {
            Level4();
        }
        else if (nivel == 5)
        {
            Level5();
        }
        else if (nivel == 6)
        {
            Level5();
        }
        else if (nivel == 6)
        {
            Level5();
        }
    }

    public void Level1()
    {
        occupiedPositions.Clear();

        int rand = Random.Range(4, 6);
        for (int i = 0; i < rand; i++)
        {
            Vector2 pos;

            // Escolhe vaca ou boi
            var cow = vacas[Random.Range(0, vacas.Count)];

            // Garante posição livre
            pos = GetFreePosition();

            var instance = Instantiate(cow, pos, Quaternion.identity, parent_animals);
            SetSortingOrder(instance, pos);
            occupiedPositions.Add(pos);
        }

        rand = Random.Range(2, 4);
        for (int i = 0; i < rand; i++)
        {
            Vector2 pos;

            // Escolhe vaca ou boi
            var cow = bois[Random.Range(0, bois.Count)];

            // Garante posição livre
            pos = GetFreePosition();

            var instance = Instantiate(cow, pos, Quaternion.identity, parent_animals);
            SetSortingOrder(instance, pos);
            occupiedPositions.Add(pos);
        }
    }

    public void Level2()
    {
        ClearAnimals();

        Level1();

        int rand = Random.Range(1, 4);
        for (int i = 0; i < rand; i++)
        {
            Vector2 pos;

            // Escolhe vaca ou boi
            var cow = vacaMentira[Random.Range(0, vacaMentira.Count)];

            // Garante posição livre
            pos = GetFreePosition();

            var instance = Instantiate(cow, pos, Quaternion.identity, parent_animals);
            SetSortingOrder(instance, pos);
            occupiedPositions.Add(pos);
        }
    }

    public void Level3()
    {
        Level2();
        cameras_obj.SetActive(true);
        fazendeiroParado.SetActive(true);
    }

    public void Level4()
    {
        Level3();

        int rand = Random.Range(1, 3);
        for (int i = 0; i < rand; i++)
        {
            Vector2 pos;

            // Escolhe vaca ou boi
            var cow = vacaExplosiva[Random.Range(0, vacaExplosiva.Count)];

            // Garante posição livre
            pos = GetFreePosition();

            var instance = Instantiate(cow, pos, Quaternion.identity, parent_animals);
            SetSortingOrder(instance, pos);
            occupiedPositions.Add(pos);
        }
    }

    public void Level5()
    {
        Level4();
        fazendeiroParado.SetActive(false);
        fazendeiroPatrulha.SetActive(true);
    }

    void ClearAnimals()
    {
        for (int i = 0; i < parent_animals.childCount; i++)
        {
            Destroy(parent_animals.GetChild(i).gameObject);
        }
    }

    public void GenerateMap()
    {
        int rand = Random.Range(min, max);
        for (int i = 0; i < rand; i++)
        {
            Vector2 pos = GetFreePosition();
            GameObject obj = objs[Random.Range(0, objs.Count)];

            var instance = Instantiate(obj, pos, Quaternion.identity, parent_objs);
            SetSortingOrder(instance, pos);
            occupiedPositions.Add(pos);
        }
    }

    private Vector2 GetFreePosition()
    {
        Vector2 pos;
        int attempts = 0;
        do
        {
            pos = new Vector2(Random.Range(down.x, top.x), Random.Range(down.y, top.y));
            attempts++;
        } while (IsNearOther(pos) && attempts < 100);
        return pos;
    }

    private bool IsNearOther(Vector2 pos)
    {
        float minDistance = 0.5f; // distância mínima para não considerar "em cima"
        foreach (var p in occupiedPositions)
        {
            if (Vector2.Distance(p, pos) < minDistance)
                return true;
        }
        return false;
    }

    private void SetSortingOrder(GameObject obj, Vector2 pos)
    {
        var sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.sortingOrder = -(int)(pos.y * 1); // Quanto maior o Y, menor o sorting
    }

    private void LevelTxt()
    {
        level_txt.text = "Level " + nivel;
        level_txt.gameObject.SetActive(true);
    }
}
