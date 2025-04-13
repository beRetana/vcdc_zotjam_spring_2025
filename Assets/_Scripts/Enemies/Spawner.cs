using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    [SerializeField] private Transform[][] waves;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private GameObject portal;
    [SerializeField] private float delayToSpawn;
    [SerializeField] private float inbetwenSpawn;
    [SerializeField] private int wavesToUse;
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

    [System.Serializable]
    public struct Waves
    {

        Transform[] wave1;
        Transform[] wave2;
        Transform[] wave3;
        Transform[] wave4;
        Transform[] wave5;
    }

    private void Start()
    {
        portal.SetActive(false);
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
