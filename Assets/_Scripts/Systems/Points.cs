using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

using TMPro;

public class Points : MonoBehaviour
{
    // ultimate state
    public static int ultimateCount; // count. 
    public static bool ultReady; // to use in other functions for combat and whatnot
    public static float mult; // multiplies 
    
    public static bool gameOver; // to use in other functions for combat and whatnot
    
    private static int points; // count.
    [SerializeField] private Happy happyScript;
    [SerializeField] private TextMeshProUGUI multText; // text
    [SerializeField] private TextMeshProUGUI ultimateDebug; // text
    [SerializeField] public int ultCap = 20; // liimit to the ultimate amt

    
    [SerializeField] private float pulseSpeed = 1f;
    [SerializeField] private float pulseScale = 1.2f;
    
    private Vector3 originalScale;
    private Coroutine startedPulsing;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // if (textToPulse == null) textToPulse = GetComponent<TextMeshProUGUI>();
        originalScale = multText.transform.localScale;

        points = 0;
        ultimateCount = 0;
        ultReady = false;
        gameOver = false;
        mult = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // how many points the enemy is worth, goes into this
    // @ BRANDON!!!!!!
    // AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAH 
    public void EnemyDeath(int enemy_value)
    {
        points += (int)(mult * enemy_value);
    }

    public int getPoints()
    {
        return points;
    }
    
// updates the ultimate count with hwatever the amount should be
    public void updateUltimate(int amt)
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

    public void setMultplier()
    {
        // if multiplier calc would be less than 5 
        int herHappy = happyScript.getHappy();
        if (herHappy< 20) 
        {
            mult = 0.025f * herHappy + 0.5f;
        }
        else
        {
            mult = Mathf.Pow(0.5f * herHappy - 10, 0.5f) * 0.4f + 1.0f; // playing around . plug into desmos lol
        }

        StartPulse();
    }

    public float getMultiplier(){
        return mult;
    }

    public void StartPulse()
    {
        if (startedPulsing != null)
            StopCoroutine(PulseText());

        startedPulsing = StartCoroutine(PulseText());
    }

    private IEnumerator PulseText()
    {
        // decimal places...
        multText.text = "x" + mult.ToString("F2");

        // Scale up
        multText.transform.localScale = originalScale*pulseScale;

        // Scale down
        yield return ScaleTo(originalScale, pulseSpeed / 2f);
    
    }

    private IEnumerator ScaleTo(Vector3 targetScale, float duration)
    {
        Vector3 startScale = multText.transform.localScale;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            multText.transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

    }

    public void setGameOver(){
        gameOver = true;
    }

    public bool isGameOver(){
        return gameOver;
    }

}
