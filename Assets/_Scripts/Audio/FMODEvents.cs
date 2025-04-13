using System.Collections;
using System.Collections. Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{


    // [field: Header("sonarPing")]
    // [field: SerializeField] public EventReference sonarPing {get; private set;}

    
    
    
    public static FMODEvents instance {get; private set;}

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events scripts in the scene");
        }
        instance = this;
    }
}
