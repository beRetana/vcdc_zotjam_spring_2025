using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemySpawners : MonoBehaviour
{
    [SerializeField] private Transform _enemy;
    [SerializeField] private List<Transform> _spawnLocations;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private TextMeshProUGUI _textMeshPro;

    private int _spawnCount;

    private void Start()
    {
        _textMeshPro.text = $"COUNTER: {_spawnLocations.Count}";
        _arrow.SetActive(false);
        foreach (Transform t in _spawnLocations)
        {
            Instantiate(_enemy, t.position, Quaternion.identity);
        }
    }

    public void AddCounter()
    {
        _spawnCount--;
        _textMeshPro.text = $"COUNTER: {_spawnCount}";
        if (_spawnCount == 0)
        {
            _arrow.SetActive(true);
        }
    }

    public static EnemySpawners Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

}
