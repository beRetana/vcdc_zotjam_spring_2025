using UnityEngine;

using TMPro;

public class Love : MonoBehaviour
{
    public delegate void UpdatedLove(float lovePercent);
    public static event UpdatedLove OnUpdatedLove;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private Points pointScript;
    
    // call this in other functions
    public const int loveCap = 100; // liimit to the love amt
    
    [SerializeField] private TextMeshProUGUI loveDebug; // text
    [SerializeField] int herLove = 20; // her happiness, defualt 20
    

    void Start()
    {
        UpdateHappinessUI();
    }

    public int getLove()
    {
        return herLove;
    }

    public void editHerLove(int amt)
    {
        // in the case amt is negative, cant go to negatives basically
        if (herLove + amt < 0) {
            herLove = 0;
            pointScript.setGameOver();
        }
        // just adds to the original happiness
        else {
            // happiness cant go past 100 pls
            if (herLove + amt >= loveCap)
            {
                herLove = loveCap;
            }
            else{     
                herLove += amt;
            }
        }
        pointScript.setMultplier();
        pointScript.updateUltimate(amt);

        UpdateHappinessUI();
    }

    void UpdateHappinessUI()
    {
        OnUpdatedLove?.Invoke(getLovePercent());
        if (loveDebug != null)
        {
            loveDebug.text = "Love: " + herLove;
        }
    }

    public float getLovePercent()
    {
        return (float)herLove / loveCap;
    }
}
