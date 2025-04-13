using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacks : MonoBehaviour
{
    [SerializeField, Range(.2f, 2f), Tooltip("How long the player has to switch into attack two")] 
    private float _attackSpeed = 1f;
    [SerializeField, Range(.1f, 5f)] private float _meleeAttackArea = .5f;
    [SerializeField, Range(.1f, 10f)] private float _meleeAttackRange = 5f;
    [SerializeField, Range(0f, 100f)] private float _meleeAttackOneDamage = 10f;
    [SerializeField, Range(0f, 100f)] private float _meleeAttackTwoDamage = 10f;
    [SerializeField, Range(2f, 20f)] private float _rangedAttackRange = 8f;
    [SerializeField] private GameObject meleeAttackOneVFX;
    [SerializeField] private GameObject meleeAttackTwoVFX;
    private PlayerController _playerController;


    // counts if youre in a loop yet or not
    private Coroutine _attackTimerCoroutine;

    // tells you if you can enter into two to continue the combo
    private bool can_go_into_two;

    private enum MeleeAttackType{
        One,
        Two,
        Charged,
    }

    private MeleeAttackType _meleeAttackType;
    private int _facingRight = 1; 
    // Facing right is assigned 1 and -1 for left this is to send the attacks on the right direction.

    public int FacingRight { get { return _facingRight; } set { _facingRight = value;}}

    void Start()
    {
        _playerController = new();
        Enable();
    }

    void Enable()
    {
        _playerController.PlayerActions.Melee.started += MeleeAttacks;
        _playerController.PlayerActions.Enable();
    }

    void OnDisable()
    {
        _playerController.PlayerActions.Melee.started -= MeleeAttacks;
        _playerController.PlayerActions.Disable();
    }

    private void MeleeAttacks(InputAction.CallbackContext context)
    {
        /*
        switch (_meleeAttackType)
        {
            case MeleeAttackType.One:
                AttackMeleeOne();
                break;
            case MeleeAttackType.Two:
                AttackMeleeTwo();
                break;
        }
        */

        // mary code, remove if it doesnt work
        // /*
        // its not cases because i dont rlly wanna do cases if im not using two.
        // maybe implement cases if we have more and more attacks but i do NOT
        // want to animate ALLAT!!! 😭
        if(_meleeAttackType ==  MeleeAttackType.One)
        {
            if(!can_go_into_two)
            {
                AttackMeleeOne();
                can_go_into_two = true;

                // no overlap!
                if (_attackTimerCoroutine != null){
                    StopCoroutine(_attackTimerCoroutine);
                }
                // starts the wait timer to see if we enter the other attack
                _attackTimerCoroutine = StartCoroutine(AttackTimer());
            }
            else
            {
                AttackMeleeTwo();

                // no overlap, again
                if (_attackTimerCoroutine != null) {
                    StopCoroutine(_attackTimerCoroutine);
                }

                // go back to the first attack type. no need to wait for anything 
                _meleeAttackType = MeleeAttackType.One;
                can_go_into_two = false;
            }
        }
        // /*
    }

    /*
    private void AttackMeleeOne()
    {
        MeleeAttack(meleeAttackOneVFX);
        StartCoroutine(AttackTimer());
    }

    private void AttackMeleeTwo()
    {
        StopCoroutine(AttackTimer());
        MeleeAttack(meleeAttackTwoVFX);
        _meleeAttackType = MeleeAttackType.One;
    }

    IEnumerator AttackTimer(){
        _meleeAttackType = MeleeAttackType.Two;
        yield return new WaitForSeconds(_attackSpeed);
        _meleeAttackType = MeleeAttackType.One;
    }
    */

    // mary edits:
    // /*
    private void AttackMeleeOne()
    {
        Debug.Log("1");
        MeleeAttack(_meleeAttackOneDamage);
    }

    private void AttackMeleeTwo()
    {
        Debug.Log("2");
        MeleeAttack(_meleeAttackTwoDamage);
    }

    IEnumerator AttackTimer(){
        // wait for this time, to see if you enter again
        yield return new WaitForSeconds(_attackSpeed);

        // didnt go fast enough, now youre cooked.
        can_go_into_two = false;

        // reset if nothing
        _meleeAttackType = MeleeAttackType.One;
        _attackTimerCoroutine = null;
    }

    // */

    private void MeleeAttack(float damage)
    {
        RaycastHit raycastHit;
        if (!Physics.SphereCast(transform.position, _meleeAttackArea, Vector3.right * _facingRight, out raycastHit, _meleeAttackRange)) return;
        raycastHit.transform.GetComponent<EnemyHealth>()?.ModifyHealth(damage);
    }

    
}
