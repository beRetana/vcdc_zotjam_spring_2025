using UnityEngine;

using TMPro;

public class Happy : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    // ultimate state
    public static int ultimateCount; // count. 
    public static bool ultReady; // to use in other functions for combat and whatnot
    public static float mult; // multiplies 
    
    // call this in other functions
    public static bool gameOver; // to use in other functions for combat and whatnot
    public static int happyCap = 100; // liimit to the happy amt
    
    [SerializeField] private TextMeshProUGUI happyDebug; // text
    [SerializeField] private TextMeshProUGUI ultimateDebug; // text
    [SerializeField] private TextMeshProUGUI multDebug; // text
    [SerializeField] public int herHappy = 20; // her happiness, defualt 20
    [SerializeField] public int ultCap = 20; // liimit to the ultimate amt
    

    void Start()
    {
        UpdateHappinessUI();
        ultimateCount = 0;
        ultReady = false;
        gameOver = false;
        mult = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Space key was just released.");
            editHerHappy(-5);
        }
    }

    public void editHerHappy(int amt)
    {
        // in the case amt is negative, cant go to negatives basically
        if (herHappy + amt < 0) {
            herHappy = 0;
            gameOver = true;
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
        setMultplier();
        updateUltimate(amt);

        UpdateHappinessUI();
    }

    void UpdateHappinessUI()
    {
        if (happyDebug != null)
        {
            happyDebug.text = "Happiness: " + herHappy;
        }
    }

// updates the ultimate count with hwatever the amount should be
    void updateUltimate(int amt)
    {

        if (amt < 0 )
        {
            // do nothing. 
            // ultimate count can only go up
            // tbf this is stupid coding on my part sorry!
        }
        else
        {
            if(ultimateCount + amt * mult >= ultCap)
            {
                ultimateCount = ultCap;
                ultReady = true;
            }
            else
            {

                ultimateCount += (int)(amt * mult);
            }
        }

        
        if (ultimateDebug != null)
        {
            ultimateDebug.text = "Ult: " + ultimateCount;
        }
    }

    void setMultplier()
    {
        // if multiplier calc would be less than 5 
        if (herHappy < 20) 
        {
            mult = 0.025f * herHappy + 0.5f;
        }
        else
        {
            mult = Mathf.Pow(0.5f * herHappy - 10, 0.5f) * 0.4f + 1.0f; // playing around . plug into desmos lol
        }

        multDebug.text = "Multplier: " + mult;
    }
}
