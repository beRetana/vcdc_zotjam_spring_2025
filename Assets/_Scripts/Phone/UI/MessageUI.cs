using TMPro;
using UnityEngine;

public class MessageUI : MonoBehaviour
{
    [SerializeField] private int size = 1;

    private TextMeshProUGUI content;

    private void Awake()
    {
        content = transform.GetComponentInChildren<TextMeshProUGUI>();
    }

    public int Size {  get { return size; } }

    public void UpdateText(string text)
    {
        content.text = text;
    }
}
