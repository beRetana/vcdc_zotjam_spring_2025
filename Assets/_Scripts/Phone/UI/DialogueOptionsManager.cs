using UnityEngine;

public class DialogueOptionsManager : MonoBehaviour
{
    [SerializeField] private DialogueOption[] _dialogueOptionsUI;
    [SerializeField] private PlayerMovement playerMovement;



    public void DisplayResponseOptions(CSVReader.DialogueRow[] dialogueOptions)
    {
       
        for (int index = 0; index < dialogueOptions.Length; ++index)
        {
            _dialogueOptionsUI[index].UpdateOption(index+1, dialogueOptions[index].dialogue);
            _dialogueOptionsUI[index].gameObject.SetActive(true);
        } 
        
    }

    public void DisableResponseOptions()
    {
        for (int index = 0; index < _dialogueOptionsUI.Length; ++index)
            _dialogueOptionsUI[index].gameObject.SetActive(false);
    }

    private void Counter()
    {

    }
}
