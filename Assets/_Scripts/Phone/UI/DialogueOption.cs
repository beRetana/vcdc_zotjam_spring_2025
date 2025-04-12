using TMPro;
using UnityEngine;

public class DialogueOption : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _indexText;
    [SerializeField] private TextMeshProUGUI _contentText;

    private const int CONTENT_MAX_LENGTH = 20;
    private const int CONTENT_OVER_INDEX = 16;

    public void UpdateOption(int index, string content)
    {
        UpdateIndex(index);
        UpdateContent(content);
    }

    private void UpdateContent(string content)
    {
        if (content.Length > CONTENT_MAX_LENGTH)
        {
            _contentText.text = content.Substring(0, CONTENT_OVER_INDEX) + "...";
            return;
        }
        _contentText.text = content;
    }

    private void UpdateIndex(int index)
    {
        _indexText.text = $"{index}:";
    }
}
