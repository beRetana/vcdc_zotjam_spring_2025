using System.Collections;
using System.Collections. Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour
{


     [field: Header("Punch Whiff")]
     [field: SerializeField] public EventReference PunchWhiff {get; private set;}

     [field: Header("Jab Punch")]
     [field: SerializeField] public EventReference JabPunch {get; private set;}

     [field: Header("Cross Punch")]
     [field: SerializeField] public EventReference CrossPunch {get; private set;}

    //AudioManager.instance.PlayOneShot(FMODEvents.instance.PunchWhiff,this.transform.position);
    
    
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
