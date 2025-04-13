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
    [SerializeField, Range(0f, 100f)] private float _chargedAttackDamage = 20f;
    [SerializeField, Range(0f, 2f)] private float _chargingTime = 2f;
    [SerializeField, Range(0f, .5f)] private float _chargeHoldingTime = .5f;

    //[SerializeField, Range(2f, 20f)] private float _rangedAttackRange = 8f;
    [SerializeField, Range(1f, 50f)] private float _ultiRangeAttack = 10f;
    [SerializeField, Range(1f, 100f)] private float _ultiDamageAttack = 50f;
    [SerializeField, Range(1f, 100f)] private float _ultiNecessaryPoints = 100f;

    [SerializeField] private LayerMask _enemiesMask;

    private PlayerController _playerController;
    private float _chargedAttackRatio;
    private float _ultiCurrentPoints;

    // counts if youre in a loop yet or not
    private Coroutine _attackTimerCoroutine;
    private Coroutine _charging;

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
        _playerController.PlayerActions.Range.started += StartCharge;
        _playerController.PlayerActions.Range.canceled += StopCharge;
        _playerController.PlayerActions.Ultimate.performed += UltimateAttack;
        _playerController.PlayerActions.Enable();
    }

    private void UltimateAttack(InputAction.CallbackContext context)
    {
        if (_ultiNecessaryPoints != _ultiCurrentPoints) return;

        Collider[] collisions = Physics.OverlapSphere(transform.position, _ultiRangeAttack, _enemiesMask);

        foreach (Collider collision in collisions)
        {
            float ratio = Mathf.Min(1, _ultiRangeAttack / (collision.transform.position - transform.position).magnitude);
            collision.transform.GetComponent<EnemyHealth>().ModifyHealth(ratio * _ultiDamageAttack);
        }
    }

    void OnDisable()
    {
        _playerController.PlayerActions.Melee.started -= MeleeAttacks;
        _playerController.PlayerActions.Range.started -= StartCharge;
        _playerController.PlayerActions.Range.canceled -= StopCharge;
        _playerController.PlayerActions.Ultimate.performed -= UltimateAttack;
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
        bool hit = MeleeAttack(_meleeAttackOneDamage);
        return;
        if (hit)
            AudioManager.instance.PlayOneShot(FMODEvents.instance.JabPunch, this.transform.position);
        else
            AudioManager.instance.PlayOneShot(FMODEvents.instance.PunchWhiff, this.transform.position);
    }

    private void AttackMeleeTwo()
    {
        Debug.Log("2");
        bool hit = MeleeAttack(_meleeAttackTwoDamage);
        if (hit)
            AudioManager.instance.PlayOneShot(FMODEvents.instance.CrossPunch, this.transform.position); // or a different sound if you have for combo 2
        else
            AudioManager.instance.PlayOneShot(FMODEvents.instance.PunchWhiff, this.transform.position);
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

    private void StartCharge(InputAction.CallbackContext context)
    {
        _charging = StartCoroutine(Charging(_chargingTime, _chargeHoldingTime));
    }

    private void StopCharge(InputAction.CallbackContext context)
    {
        StopCoroutine(_charging);
        MeleeAttack(Mathf.Min(_chargedAttackRatio, 1) * _chargedAttackDamage);
        _chargedAttackRatio = 0;
    }

    IEnumerator Charging(float chargeTime, float holdingTime)
    {
        float timer = 0;
        while (timer < chargeTime + holdingTime)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            _chargedAttackRatio = timer / chargeTime;
        }

        MeleeAttack(Mathf.Min(_chargedAttackRatio, 1) * _chargedAttackDamage);
        _chargedAttackRatio = 0;
    }

    private bool MeleeAttack(float damage)
    {
        RaycastHit raycastHit;
        if (Physics.SphereCast(transform.position, _meleeAttackArea, Vector3.right * _facingRight, out raycastHit, _meleeAttackRange))
        {
            var enemyHealth = raycastHit.transform.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.ModifyHealth(damage);
                return true; // It hit!
            }
        }
        return false; // It missed!
    }
}
