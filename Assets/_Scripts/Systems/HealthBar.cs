using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private GameObject heartPrefab;
    [SerializeField] private Transform heartContainer;
    [SerializeField] private float popTime = 0.5f;
    [SerializeField] private float heartTargetScale = 1.5f;
    [SerializeField] private float heartDropDistance = 20f;
    

    private List<GameObject> heartList = new();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setHealth(int currentHealth)
    {
        // Clear old hearts
        foreach (var heart in heartList)
        {
            Destroy(heart);
        }
        heartList.Clear();

        // Add new hearts
        for (int i = 0; i < currentHealth; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartContainer);
            heartList.Add(newHeart);
        }
    }

    public void gainHealth ()
    {
        GameObject newHeart = Instantiate(heartPrefab, heartContainer);
        heartList.Add(newHeart);
        StartCoroutine(HeartPopIn(newHeart.transform));
    }


    private IEnumerator HeartPopIn(Transform heartTransform)
    {
        Vector3 startScale = heartTransform.localScale;
        Vector3 bigScale = startScale * heartTargetScale;
        heartTransform.localScale = bigScale;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / popTime;
            heartTransform.localScale = Vector3.Lerp(bigScale, startScale, t);
            yield return null;
        }
        
        heartTransform.localScale = startScale; 
    }

    public void LoseHealth()
    {
        // remove the last heart!
        if (heartList.Count > 0)
        {
            GameObject lastHeart = heartList[heartList.Count - 1];
            heartList.RemoveAt(heartList.Count - 1);
            StartCoroutine(HeartFades(lastHeart));
        }
    }


    private IEnumerator HeartFades(GameObject heart )
    {
        Transform heartTransform = heart.transform;
        Image heartImage = heart.GetComponent<Image>();

        float t = 0f;
        float alphaVal = heartImage.color.a;
        Color tmp = heartImage.color;

        while (t < 1f)
        {
            t += Time.deltaTime / popTime;

            heartTransform.position += Vector3.down * heartDropDistance * Time.deltaTime;
            if (tmp.a > 0)
            {
                alphaVal -= 0.01f;
                tmp.a = alphaVal;
                heartImage.color = tmp;
            }
            yield return null;
        }

        Destroy(heart);
    }

}
