using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool aim;
		public bool sneek;
		public bool shoot;
		public bool interact;
		public bool raycastinteract;
		public Vector2 puzzleMove;
		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
		public void OnAim(InputValue value)
		{
			AimInput(value.isPressed);
		}
		public void OnShoot(InputValue value)
		{
			ShootInput(value.isPressed);
		}
		public void OnSneek(InputValue value)
		{
			SneekInput(value.isPressed);
		}

		public void OnRaycastInteract(InputValue value)
		{
			RaycastInteractInput(value.isPressed);
		}

		public void OnInteract(InputValue value)
		{
			// InteractInput(value.isPressed);
		}
		
		public void OnPuzzleNavigateUpDown(InputValue value)
		{
			PuzzleNavigateInput(0, value.Get<float>());
		}
		public void OnPuzzleNavigateLeftRight(InputValue value)
		{
			PuzzleNavigateInput(1, value.Get<float>());
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		public void AimInput(bool newAimState)
		{
			aim = newAimState;
		}
		public void SneekInput(bool newSneekState)
		{
			sneek = newSneekState;
		}

		public void ShootInput(bool newShootState)
		{
			shoot = newShootState;
		}

		public void RaycastInteractInput(bool newRaycastInteractState)
		{
			raycastinteract = newRaycastInteractState;
		}

		// public void InteractInput(bool newInteractState)
		// {
		// 	interact = newInteractState;
		// }

		public void PuzzleNavigateInput(int dir, float value)
		{
			puzzleMove[dir] = value;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}
	}
	
}