using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAlternateAttacks : MonoBehaviour
{
    [SerializeField, Range(.2f, 2f), Tooltip("How long the player has to switch into attack two")] 
    private float _attackSpeed = 1f;
    [SerializeField, Range(.1f, 5f)] private float _meleeAttackArea = .5f;
    [SerializeField, Range(.1f, 10f)] private float _meleeAttackRange = 5f;
    //[SerializeField, Range(2f, 20f)] private float _rangedAttackRange = 8f;
    [SerializeField] private GameObject meleeAttackOneVFX;
    [SerializeField] private GameObject meleeAttackTwoVFX;

    private PlayerController _playerController;

    // counts if youre in a loop yet or not
    private Coroutine _attackTimerCoroutine;

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
                
    }

    private void AttackMeleeOne()
    {
        Debug.Log("1");
        MeleeAttack(meleeAttackOneVFX);
        // StartCoroutine(AttackTimer());
    }

    private void AttackMeleeTwo()
    {
        Debug.Log("2");
        // StopCoroutine(AttackTimer());
        MeleeAttack(meleeAttackTwoVFX);
        // _meleeAttackType = MeleeAttackType.One;
    }

    private void MeleeAttack(GameObject attackVFX)
    {
        RaycastHit raycastHit;
        if (!Physics.SphereCast(transform.position, _meleeAttackArea, Vector3.right * _facingRight, out raycastHit, _meleeAttackRange)) return;
        Instantiate(attackVFX, transform.position + new Vector3(_meleeAttackRange, 0,0) * _facingRight, Quaternion.identity);
        Debug.Log(" "+raycastHit.point);
    }

    IEnumerator AttackTimer(){
        // wait for this time, to see if you enter again
        yield return new WaitForSeconds(_attackSpeed);

        can_go_into_two = false;

        // reset if nothing
        _meleeAttackType = MeleeAttackType.One;
        _attackTimerCoroutine = null;
    }
}
