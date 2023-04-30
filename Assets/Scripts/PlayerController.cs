using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Tuna
{
    public class PlayerController : MonoBehaviour
    {
        //Fields
        [SerializeField] private StackManager stackManager;
        [SerializeField] private Joystick joystick;
        [SerializeField] private Transform weaponHandler;
        [SerializeField] private Transform axeHandler;
        [SerializeField] private Transform cleaner;
        [SerializeField] private Animator animator;
        [SerializeField] private float sensitivity = 1;
        [SerializeField] private float weaponSpinSpeed;

        //Properties
        private bool _takeInput = true;
        private Vector3 _moveDirection;
        private Quaternion _lastRotation;
        private PlayerStatus _currentStatus;

        //Farm Properties        
        private Farm _currentFarm;
        private bool _inFarmZone;
        private float lastTimeFarmed;
        private float farmDuration = 1.4f;
        private bool inFarmZone
        {
            get => _inFarmZone;
            set
            {
                _inFarmZone = value;
                if (_inFarmZone)
                {
                    axeHandler.localEulerAngles = Vector3.zero;
                    axeHandler.DOLocalRotate(new Vector3(40f, 0f,0f), farmDuration/2)
                        .SetLoops(-1, LoopType.Restart)
                        .SetEase(Ease.InOutSine);
                }
                else
                {
                    axeHandler.DOKill();
                }
                axeHandler.gameObject.SetActive(_inFarmZone);
            }
        }

        //Fight Properties
        private bool _hasWeapon = false;
        private bool _inFightZone;
        private bool inFightZone
        {
            get => _inFightZone;
            set
            {
                _inFightZone = value;
                var showWeapon = inFightZone && _hasWeapon;
                weaponHandler.gameObject.SetActive(showWeapon);
            }
        }
        private bool noEnemyLeft;
        private bool noBloodLeft;
        private static readonly int WalkSpeed = Animator.StringToHash("WalkSpeed");
        private static readonly int IsCarrying = Animator.StringToHash("isCarrying");

        //Unity Methods
        private void Start()
        {

            EventManager.EnemyDeath += OnEnemyDeath;
            EventManager.GameOver += StopCleaning;

            SetStatus(PlayerStatus.Natural);
        }

        private void OnDestroy()
        {
            EventManager.EnemyDeath -= OnEnemyDeath;
        }

        private void Update()
        {
            if (_takeInput)
            {
                var inputVector = new Vector3(joystick.Horizontal, 0f, joystick.Vertical);
                _moveDirection = Vector3.ClampMagnitude(inputVector, 1);
                animator.SetFloat(WalkSpeed, _moveDirection.magnitude);
            }

            CheckStatusJobs();
        }

        private void FixedUpdate()
        {
            if (_moveDirection.sqrMagnitude > 0.01)
            {
                transform.position += _moveDirection * (sensitivity * Time.fixedDeltaTime);
                _lastRotation = Quaternion.LookRotation(_moveDirection);
                transform.rotation = _lastRotation;
            }
            else
            {
                transform.rotation = _lastRotation;
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Farm"))
            {
                if (other.transform.TryGetComponent<Farm>(out Farm _farm))
                {
                    _currentFarm = _farm;
                    SetStatus(PlayerStatus.Farming);
                }
            }
            else if (other.CompareTag("Enemy") && !noEnemyLeft)
            {
                SetStatus(PlayerStatus.Fighting);
            }
            else if (other.CompareTag("DropZone"))
            {
                if (other.TryGetComponent<Factory>(out Factory factory))
                {
                    var factoryType = factory.factoryType;
                    var _hasType = stackManager.HasType(factoryType, out List<Collectable> collectablesToSend);
                    if (_hasType)
                    {
                        factory.TryTakeItem(stackManager ,collectablesToSend);
                    }
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Farm"))
            {
                SetStatus( PlayerStatus.Natural);
            }
            else if (other.CompareTag("Enemy"))
            {
                SetStatus( PlayerStatus.Natural);
            }
        }
        
        //Private Methods
        private void CheckStatusJobs()
        {
            switch (_currentStatus)
            {
                case PlayerStatus.Farming:
                    if (Time.time >= lastTimeFarmed + farmDuration)
                    {
                        lastTimeFarmed = Time.time;
                        _currentFarm.TryTakeProduct();
                    }
                    break;
                case PlayerStatus.Fighting:
                    weaponHandler.RotateAround(weaponHandler.transform.position, Vector3.down, 3f);
                    break;
            }
        }

        private void SetStatus(PlayerStatus newStatus)
        {
            _currentStatus = newStatus;

            switch (_currentStatus)
            {
                case PlayerStatus.Natural:
                    _currentFarm = null;
                    inFarmZone = false;
                    inFightZone = false;
                    break;
                case PlayerStatus.Farming:
                    inFarmZone = true;
                    break;
                case PlayerStatus.Fighting:
                    inFightZone = true;
                    break;
                case PlayerStatus.Cleaning:
                    inFightZone = false;
                    inFarmZone = false;
                    break;
            }
        }
        
        //Public Methods
        public void AcquireWeapon()
        {
            _hasWeapon = true;
        }

        public void IsHandsEmpty(bool value)
        {
            animator.SetBool(IsCarrying, value);
        }
        
        private void StopCleaning()
        {
            cleaner.gameObject.SetActive(false);
            SetStatus(PlayerStatus.Natural);
        }

        private void OnEnemyDeath()
        {
            SetStatus(PlayerStatus.Cleaning);
            noEnemyLeft = true;
            cleaner.gameObject.SetActive(true);
        }

    }
}
