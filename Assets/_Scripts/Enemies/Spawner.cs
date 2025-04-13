using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform enemy;
    [SerializeField] private List<Transform> locations;
    [SerializeField] private TextMeshProUGUI textMeshPro;
    [SerializeField] private GameObject portal;

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
        counter = locations.Count;

        foreach (Transform t in locations) { 
            Instantiate(enemy, t.position, Quaternion.identity);
        }

        textMeshPro.text = $"Enemies Left: {counter}";
    }

    public void UpdateCounter()
    {
        counter--;
        if (counter == 0) portal.SetActive(true);
        textMeshPro.text = $"Enemies Left: {counter}";
    }
}
