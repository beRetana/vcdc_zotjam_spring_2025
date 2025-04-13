using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using FMOD.Studio;

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

    [SerializeField, Range(1f, 50f)] private float _ultiRangeAttack = 10f;
    [SerializeField, Range(1f, 100f)] private float _ultiDamageAttack = 50f;
    [SerializeField, Range(1f, 100f)] private float _ultiNecessaryPoints = 100f;
    [SerializeField] private LayerMask _enemiesMask;
    [SerializeField] private GameObject ultiVFX;

    private PlayerController _playerController;
    private PlayerAnimations _playerAnimations;
    private float _ultiCurrentPoints;

    // combo state
    private Coroutine _attackTimerCoroutine;
    private bool can_go_into_two;
    private enum MeleeAttackType { One, Two, Charged }
    private MeleeAttackType _meleeAttackType = MeleeAttackType.One;
    private int _facingRight = 1;
    public int FacingRight { get { return _facingRight; } set { _facingRight = value; } }

    // charging state
    private Coroutine _charging;
    private bool _isCharging = false;
    private bool _canReleaseAttack = false;
    private float _currentChargeTime = 0f;
    private float _chargedAttackRatio = 0f;

    // FMOD event instances
    private EventInstance HaymakerCharge;
    private EventInstance HaymakerImpact;

    void Start()
    {
        _playerController = new PlayerController();
        _playerAnimations = GetComponent<PlayerAnimations>();
        Enable();

        // Create FMOD instances
        HaymakerCharge = AudioManager.instance.CreateInstance(FMODEvents.instance.HaymakerCharge);
        HaymakerImpact = AudioManager.instance.CreateInstance(FMODEvents.instance.HaymakerImpact);
    }

    // ─────── CHARGING ENTRY POINT ───────
    private void StartCharge(InputAction.CallbackContext ctx)
    {
        if (_charging != null) 
            StopCoroutine(_charging);

        _playerAnimations.Charging(true);

        // Start the FMOD charge sound immediately
        HaymakerCharge.start();

        // Begin charging loop
        _isCharging = true;
        _charging = StartCoroutine(ChargeAttack());
    }

    void Enable()
    {
        var actions = _playerController.PlayerActions;
        actions.Melee.started += MeleeAttacks;
        actions.Range.started += StartCharge;
        actions.Range.canceled += StopCharge;
        actions.Ultimate.performed += UltimateAttack;
        actions.Enable();
    }

    void OnDisable()
    {
        var actions = _playerController.PlayerActions;
        actions.Melee.started -= MeleeAttacks;
        actions.Range.started -= StartCharge;
        actions.Range.canceled -= StopCharge;
        actions.Ultimate.performed -= UltimateAttack;
        actions.Disable();
    }

    private void UltimateAttack(InputAction.CallbackContext ctx)
    {
        _playerAnimations.Ulti();
    }

    public void LogicUlti()
    {
        var hits = Physics.OverlapSphere(transform.position, _ultiRangeAttack, _enemiesMask);
        var game = Instantiate(ultiVFX, transform);
        Destroy(game, 2f);

        foreach (var c in hits)
        {
            AudioManager.instance.PlayOneShot(FMODEvents.instance.ULTSFX, transform.position);
            float ratio = Mathf.Min(1, _ultiRangeAttack / (c.transform.position - transform.position).magnitude);
            c.GetComponent<EnemyHealth>()?.ModifyHealth(ratio * _ultiDamageAttack);
        }
    }

    private void MeleeAttacks(InputAction.CallbackContext ctx)
    {
        if (_meleeAttackType == MeleeAttackType.One)
        {
            if (!can_go_into_two)
            {
                AttackMeleeOne();
                _playerAnimations.Jab_One();
                can_go_into_two = true;
                if (_attackTimerCoroutine != null) StopCoroutine(_attackTimerCoroutine);
                _attackTimerCoroutine = StartCoroutine(AttackTimer());
            }
            else
            {
                AttackMeleeTwo();
                _playerAnimations.Jab_Two();
                if (_attackTimerCoroutine != null) StopCoroutine(_attackTimerCoroutine);
                _meleeAttackType = MeleeAttackType.One;
                can_go_into_two = false;
            }
        }
    }

    private void AttackMeleeOne()
    {
        bool hit = MeleeAttack(_meleeAttackOneDamage);
        AudioManager.instance.PlayOneShot(
            hit ? FMODEvents.instance.JabPunch : FMODEvents.instance.PunchWhiff,
            transform.position
        );
    }

    private void AttackMeleeTwo()
    {
        bool hit = MeleeAttack(_meleeAttackTwoDamage);
        AudioManager.instance.PlayOneShot(
            hit ? FMODEvents.instance.CrossPunch : FMODEvents.instance.PunchWhiff,
            transform.position
        );
    }

    private IEnumerator AttackTimer()
    {
        yield return new WaitForSeconds(_attackSpeed);
        can_go_into_two = false;
        _meleeAttackType = MeleeAttackType.One;
        _attackTimerCoroutine = null;
    }

    // ─────── CHARGING EXIT POINT ───────
    private void StopCharge(InputAction.CallbackContext ctx)
    {
        if (!_isCharging) return;

        _isCharging = false;
        StopCoroutine(_charging);

        if (_canReleaseAttack)
        {
            HaymakerCharge.stop(STOP_MODE.ALLOWFADEOUT);
            HaymakerImpact.start();
            MeleeAttack(_chargedAttackRatio * _chargedAttackDamage);
        }
        else
        {
            HaymakerCharge.stop(STOP_MODE.IMMEDIATE);
        }

        _playerAnimations.Charging(false);

        _currentChargeTime = 0f;
        _chargedAttackRatio = 0f;
        _canReleaseAttack = false;
    }

    private IEnumerator ChargeAttack()
    {
        _isCharging = true;
        _currentChargeTime = 0f;
        _chargedAttackRatio = 0f;
        _canReleaseAttack = false;

        PLAYBACK_STATE ps;
        HaymakerCharge.getPlaybackState(out ps);
        if (ps == PLAYBACK_STATE.STOPPED)
            HaymakerCharge.start();

        while (_isCharging)
        {
            _currentChargeTime += Time.deltaTime;
            _chargedAttackRatio = Mathf.Clamp01(_currentChargeTime / _chargingTime);

            if (!_canReleaseAttack && _chargedAttackRatio >= 0.40f)
                _canReleaseAttack = true;

            if (_chargedAttackRatio >= .6f)
            {
                _isCharging = false;
                HaymakerCharge.stop(STOP_MODE.ALLOWFADEOUT);
                HaymakerImpact.start();
                _playerAnimations.Charging(false);
                MeleeAttack(_chargedAttackRatio * _chargedAttackDamage);
                yield break;
            }

            yield return null;
        }
    }

    private bool MeleeAttack(float damage)
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, _meleeAttackArea, Vector3.right * _facingRight, out hit, _meleeAttackRange))
        {
            hit.transform.GetComponent<EnemyHealth>()?.ModifyHealth(damage);
            return true;
        }
        return false;
    }
}
