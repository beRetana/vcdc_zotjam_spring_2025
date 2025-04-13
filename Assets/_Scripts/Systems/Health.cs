using UnityEngine;

public class Health : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private HealthBar bonusHealthBar;
    [SerializeField] private int imageryMax = 5;

    private int totalHP;
    private int maxHP;

    void Start()
    {
        totalHP = 5;
        maxHP = 5;
        healthBar.setHealth(imageryMax);
        bonusHealthBar.setHealth(0);
    }

    // when you want to have a BONUS HEALTH BAR.
    // i assume you want to also gain health if you area
    // increasing the max value, like you gain a new
    // life from a stage? so idk
    public void increaseMaxHP()
    {
        if(maxHP < 2 * imageryMax)
        {
            maxHP++;
        }
        GainHealth();
    }

    public void GainHealth()
    {
        Debug.Log("GAIN!");
        // if it's less than 2 rows...
        // AAAND maxHP is real
        if (totalHP + 1 <= 2* imageryMax  && totalHP +1 <= maxHP)
        {
            // if you surpass the first row
            if (totalHP + 1 > imageryMax){
                bonusHealthBar.gainHealth();
            }
            else
            {
                // gain generic health
                healthBar.gainHealth();
            }
            totalHP++;
        }
        else
        {
            // over the limit. nice try, idiot
        }
    }

    public void LoseHealth()
    {
        // you lose!!!
        if (totalHP - 1 == 0)
        {
            // YOU LOSE!!!
        }
        else
        {
            // your total HP is over the max already so no worries
            if(totalHP > imageryMax) {
                bonusHealthBar.LoseHealth();
            }
            else
            {
                healthBar.LoseHealth();
            }
            totalHP --;
        }
    }

    // new stage or something
    public void resetHealth()
    {
        healthBar.setHealth(imageryMax);
        bonusHealthBar.setHealth(totalHP - imageryMax);
        totalHP = maxHP;
    }


}
