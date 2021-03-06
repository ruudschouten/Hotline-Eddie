﻿using Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private Player player;
        [SerializeField] private float angleOffset;
        [SerializeField] private Transform bulletEmitTransform;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private float baseSpeed;
        [SerializeField] private float dashSpeedMultiplier;
        [SerializeField] private float minimumStaminaNeededForDashing;
        [SerializeField] private float staminaUsedForDashing;
        [SerializeField] private float timeBetweenShots;

        private float _movementSpeed;
        private bool _isDashing;
        
        private bool _shouldMove;
        private Vector2 _inputVector;
        private Vector3 _directionSpeed;

        private Vector2 _cursorPosition;
        private bool _checkForShooting;
        private bool _canShoot;
        private float _shootTimer;

        private void Awake()
        {
            _movementSpeed = baseSpeed;
        }

        private void FixedUpdate()
        {
            if (player.IsDead)
            {
                return;
            }
            
            if (_shouldMove)
            {
                Move();
            }
            
            LookAtMouse();
            
            if (_isDashing)
            {
                player.Stamina -= staminaUsedForDashing * Time.deltaTime;
            }

            if (player.Stamina <= minimumStaminaNeededForDashing)
            {
                _isDashing = false;
                _movementSpeed = baseSpeed;
            }

            if (!_checkForShooting)
            {
                return;
            }

            if (_canShoot)
            {
                Shoot();
                _shootTimer = 0f;
            }
            else
            {
                _shootTimer += Time.deltaTime;
                if (_shootTimer >= timeBetweenShots)
                {
                    _canShoot = true;
                }
            }
        }

        public void HandleShootInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _canShoot = true;
                _checkForShooting = true;
            }

            if (context.canceled)
            {
                _checkForShooting = false;
                _canShoot = false;
                _shootTimer = 0f;
            }
        }
        
        public void HandleMoveInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _shouldMove = true;
                _inputVector = context.ReadValue<Vector2>();
                _directionSpeed = new Vector3(_inputVector.x, _inputVector.y, 0);
            }

            if (context.canceled)
            {
                _shouldMove = false;
                _directionSpeed = Vector3.zero;
            }
        }

        public void HandleDashInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _isDashing = true;
                _movementSpeed = baseSpeed * dashSpeedMultiplier;
            }

            if (context.canceled)
            {
                _isDashing = false;
                _movementSpeed = baseSpeed;
            }
        }

        private void Move()
        {
            var newPosition = player.transform.localPosition + _directionSpeed * (_movementSpeed * Time.deltaTime);
            player.transform.localPosition = newPosition;
        }

        private void Shoot()
        {
            player.OnShoot.Invoke();
            var bullet = Instantiate(bulletPrefab, bulletEmitTransform.position, bulletEmitTransform.rotation);
            bullet.ShouldUpdate = true;
            
            _canShoot = false;
        }

        private void LookAtMouse()
        {
            _cursorPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            var angle = LookAtHelper.GetAngleAtTarget(_cursorPosition, player.transform.localPosition) - angleOffset;
            player.transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}