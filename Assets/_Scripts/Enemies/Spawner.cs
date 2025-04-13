using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform[] wave1;
    [SerializeField] private Transform[] wave2;
    [SerializeField] private Transform[] wave3;
    [SerializeField] private Transform[] wave4;
    [SerializeField] private Transform[] wave5;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private GameObject portal;
    [SerializeField] private float inbetwenSpawn;
    private Transform[][] waves;
    private int counter;

    public static Spawner Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        portal.SetActive(false);
        
        waves = new Transform[5][];
        waves[0] = wave1;
        waves[1] = wave2;
        waves[2] = wave3;
        waves[3] = wave4;
        waves[4] = wave5;

        StartCoroutine(Spawning());
    }

    private void Spawn(Transform[] wave)
    {
        counter = wave.Length;
        textMeshPro.text = $"Enemies Left: {counter}";
        foreach (Transform t in wave)
        {
            Instantiate(enemy, t.position, Quaternion.identity);
        }
    }

    private IEnumerator Spawning()
    {
        for (int i = 0; i < waves.Length; ++i)
        {
            yield return new WaitForSecondsRealtime(inbetwenSpawn);
            Spawn(waves[i]);
        }

        portal.SetActive(true);
    }

    public void UpdateCounter()
    {
        counter--;
        if (counter == 0) portal.SetActive(true);
        textMeshPro.text = $"Enemies Left: {counter}";
    }
}
