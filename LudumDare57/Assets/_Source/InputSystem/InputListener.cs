using FlashlightSystem;
using GameSystem;
using InteractionSystem;
using MovementSystem;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputSystem
{
    public class InputListener
    {
        private readonly MovementController _movementController;
        private readonly LookController _lookController;
        private readonly InteractionController _interactionController;
        private readonly Flashlight _flashlightController;
        private readonly GamePauseController _gamePauseController;

        private PlayerControls _playerControls;

        public InputListener(MovementController movementController,
                             LookController lookController,
                             InteractionController interactionController,
                             Flashlight flashlightController,
                             GamePauseController gamePauseController)
        {
            _movementController = movementController != null ? movementController : throw new ArgumentNullException(nameof(movementController));
            _lookController = lookController != null ? lookController : throw new ArgumentNullException(nameof(lookController));
            _interactionController = interactionController ?? throw new ArgumentNullException(nameof(interactionController));
            _flashlightController = flashlightController ?? throw new ArgumentNullException(nameof(flashlightController));
            _gamePauseController = gamePauseController ?? throw new ArgumentNullException(nameof(gamePauseController));
        }

        public void SetupInputActions(PlayerControls playerControls)
        {
            _playerControls = playerControls;

            _playerControls.Movement.Move.started += OnMoveInput;
            _playerControls.Movement.Move.performed += OnMoveInput;
            _playerControls.Movement.Move.canceled += OnMoveInput;

            _playerControls.Movement.Look.started += OnLookInput;
            _playerControls.Movement.Look.performed += OnLookInput;
            _playerControls.Movement.Look.canceled += OnLookInput;

            _playerControls.Interaction.Interact.performed += OnInteractInput;

            _playerControls.Flashlight.SwitchMode.performed += OnSwitchModeInput;

            _playerControls.Pause.TogglePause.performed += OnPauseInput;
        }

        public void UnsetupInputActions()
        {
            _playerControls.Movement.Move.started += OnMoveInput;
            _playerControls.Movement.Move.performed += OnMoveInput;
            _playerControls.Movement.Move.canceled += OnMoveInput;

            _playerControls.Movement.Look.started += OnLookInput;
            _playerControls.Movement.Look.performed += OnLookInput;
            _playerControls.Movement.Look.canceled += OnLookInput;

            _playerControls.Interaction.Interact.performed += OnInteractInput;

            _playerControls.Flashlight.SwitchMode.performed += OnSwitchModeInput;

            _playerControls.Pause.TogglePause.performed += OnPauseInput;
        }

        private void OnMoveInput(InputAction.CallbackContext context)
        {
            Vector2 movementInput = context.ReadValue<Vector2>();
            _movementController.Move(movementInput);
        }

        private void OnLookInput(InputAction.CallbackContext context)
        {
            Vector2 lookInput = context.ReadValue<Vector2>();
            _lookController.LookAt(lookInput);
        }

        private void OnInteractInput(InputAction.CallbackContext context)
        {
            _interactionController.Interact();
        }

        private void OnSwitchModeInput(InputAction.CallbackContext context)
        {
            float input = context.ReadValue<float>();

            if (input > 0f)
            {
                _flashlightController.SwitchToNextConfiguration();
            }
            else if (input < 0f)
            {
                _flashlightController.SwitchToPreviousConfiguration();
            }
        }

        private void OnPauseInput(InputAction.CallbackContext context)
        {
            if (_gamePauseController.IsPaused)
                _gamePauseController.UnpauseGame();
            else
                _gamePauseController.PauseGame();
        }
    }
}