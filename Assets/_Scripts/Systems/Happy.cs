using UnityEngine;

using TMPro;

public class Happy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public static int herHappy = 20;
    
    [SerializeField]
    private TextMeshProUGUI happyDebug;
    void Start()
    {
        UpdateHappinessUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void editHerHappy(int amt)
    {
        // in the case amt is negative, cant go to negatives basically
        if (herHappy + amt < 0) {
            herHappy = 0;
        }
        // just adds to the original happiness
        else {
            herHappy += amt;
        }
        UpdateHappinessUI();
    }

    void UpdateHappinessUI()
    {
        if (happyDebug != null)
        {
            happyDebug.text = "Happiness: " + herHappy;
        }
    }
}
