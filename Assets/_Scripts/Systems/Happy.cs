using UnityEngine;

using TMPro;

public class Happy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [SerializeField] private Points pointScript;
    
    // call this in other functions
    public static int happyCap = 100; // liimit to the happy amt
    
    [SerializeField] private TextMeshProUGUI happyDebug; // text
    [SerializeField] public int herHappy = 20; // her happiness, defualt 20
    

    void Start()
    {
        UpdateHappinessUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int getHappy()
    {
        return herHappy;
    }

    public void editHerHappy(int amt)
    {
        // in the case amt is negative, cant go to negatives basically
        if (herHappy + amt < 0) {
            herHappy = 0;
            pointScript.setGameOver();
        }
        // just adds to the original happiness
        else {
            // happiness cant go past 100 pls
            if (herHappy + amt >= happyCap)
            {
                herHappy = happyCap;
            }
            else{     
                herHappy += amt;
            }
        }
        pointScript.setMultplier();
        pointScript.updateUltimate(amt);

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
