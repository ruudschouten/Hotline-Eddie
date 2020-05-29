using System;
using Helpers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private new Camera camera;
        [SerializeField] private Player player;
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private float movementSpeed;
        [SerializeField] private float timeBetweenShots;
        
        private bool _shouldMove;
        private Vector2 _inputVector;
        private Vector3 _directionSpeed;

        private Vector2 _cursorPosition;
        private bool _checkForShooting;
        private bool _canShoot;
        private float _shootTimer;

        private void Update()
        {
            LookAtMouse();
        }

        private void FixedUpdate()
        {
            
            if (_shouldMove)
            {
                Move();
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

        private void Move()
        {
            var newPosition = player.transform.localPosition + _directionSpeed * (movementSpeed * Time.deltaTime);
            player.transform.localPosition = newPosition;
        }

        private void Shoot()
        {
            var bullet = Instantiate(bulletPrefab, player.transform.position, player.transform.rotation);
            bullet.ShouldUpdate = true;
            
            _canShoot = false;
        }

        private void LookAtMouse()
        {
            _cursorPosition = camera.ScreenToWorldPoint(Input.mousePosition);
            var angle = LookAtHelper.GetAngleAtTarget(_cursorPosition, player.transform.localPosition) - 90;
            player.transform.localRotation = Quaternion.Euler(0, 0, angle);
        }
    }
}