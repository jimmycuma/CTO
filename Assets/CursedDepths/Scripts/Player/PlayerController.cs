using UnityEngine;
using UnityEngine.InputSystem;
using CursedDepths.Core;

namespace CursedDepths.Player
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        private Rigidbody2D body;
        private PlayerHealth health;
        private PlayerShooting shooting;
        private PlayerStatsData stats;
        private Vector2 moveInput;

        public Vector2 AimDirection { get; private set; } = Vector2.right;
        public PlayerHealth Health => health;
        public PlayerShooting Shooting => shooting;
        public PlayerStatsData Stats => stats;

        public void Initialize(PlayerStatsData data)
        {
            stats = data;
            health.ApplyStats(data);
            shooting.ApplyStats(data);
        }

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            body.gravityScale = 0f;
            body.drag = 8f;
            health = GetComponent<PlayerHealth>();
            shooting = GetComponent<PlayerShooting>();
        }

        private void Update()
        {
            ReadInput();
            shooting.Tick(Time.deltaTime, AimDirection);
        }

        private void FixedUpdate()
        {
            body.velocity = moveInput * stats.moveSpeed;
        }

        private void ReadInput()
        {
            var keyboard = Keyboard.current;
            var gamepad = Gamepad.current;

            float x = 0f;
            float y = 0f;

            if (keyboard != null)
            {
                x = (keyboard.aKey.isPressed ? -1f : 0f) + (keyboard.dKey.isPressed ? 1f : 0f);
                y = (keyboard.sKey.isPressed ? -1f : 0f) + (keyboard.wKey.isPressed ? 1f : 0f);
            }

            if (gamepad != null)
            {
                var stick = gamepad.leftStick.ReadValue();
                if (stick.sqrMagnitude > 0.01f)
                {
                    x = stick.x;
                    y = stick.y;
                }
            }

            moveInput = new Vector2(x, y);
            if (moveInput.sqrMagnitude > 1f)
            {
                moveInput.Normalize();
            }

            Vector2 aimInput = moveInput;
            if (gamepad != null)
            {
                var aimStick = gamepad.rightStick.ReadValue();
                if (aimStick.sqrMagnitude > 0.1f)
                {
                    aimInput = aimStick.normalized;
                }
            }

            if (aimInput.sqrMagnitude > 0.01f)
            {
                AimDirection = aimInput.normalized;
            }

            if (keyboard != null && keyboard.spaceKey.wasPressedThisFrame)
            {
                shooting.TryShoot(AimDirection);
            }

            if (gamepad != null && gamepad.rightTrigger.wasPressedThisFrame)
            {
                shooting.TryShoot(AimDirection);
            }
        }
    }
}
