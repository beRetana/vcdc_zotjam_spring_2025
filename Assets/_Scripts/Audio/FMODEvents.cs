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

     [field: Header("Dash SFX")]
     [field: SerializeField] public EventReference DashSFX {get; private set;}

     [field: Header("MessageReceived")]
     [field: SerializeField] public EventReference MessageReceived {get; private set;}

     [field: Header("Haymaker Charge")]
     [field: SerializeField] public EventReference HaymakerCharge {get; private set;}

     [field: Header("Haymaker Impact")]
     [field: SerializeField] public EventReference HaymakerImpact {get; private set;}

     [field: Header("Ultimate SFX")]
     [field: SerializeField] public EventReference ULTSFX {get; private set;}

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
