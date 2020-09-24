using UnityEngine;
using UnityEngine.InputSystem;

namespace Symmetry {
	public class GameController : MonoBehaviour {
		[SerializeField] protected Level  _level;
		[SerializeField] protected Player _whitePlayer;
		[SerializeField] protected Player _blackPlayer;

		private int countPlayersAtExit { get; set; }

		private void Start() {
			Inputs.controls.Game.Restart.AddPerformListenerOnce(RestartLevel);
			LevelEventSystem.onExitEntered.AddListenerOnce(HandlePlayerEnteredLevelExit);
			LevelEventSystem.onExitExited.AddListenerOnce(HandlePlayerExitedLevelExit);
			RestartLevel();
		}

		private void HandlePlayerEnteredLevelExit() {
			countPlayersAtExit++;
			if (countPlayersAtExit < 2) return;
			_level.LoadNextLevel();
			_whitePlayer.ResetPositionAndVelocity(_level.whiteSpawn);
			_blackPlayer.ResetPositionAndVelocity(_level.blackSpawn);
		}

		private void HandlePlayerExitedLevelExit() => countPlayersAtExit--;

		private void OnDestroy() {
			Inputs.controls.Game.Restart.RemovePerformListener(RestartLevel);
		}

		private void OnEnable() {
			SetListenersEnabled(true);
		}

		private void OnDisable() {
			SetListenersEnabled(false);
		}

		private static void SetListenersEnabled(bool enabled) {
			Inputs.controls.Player.HorizontalMovement.SetEnabled(enabled);
			Inputs.controls.Player.Jump.SetEnabled(enabled);
			Inputs.controls.Game.Restart.SetEnabled(enabled);
		}

		private void RestartLevel(InputAction.CallbackContext obj) => RestartLevel();

		private void RestartLevel() {
			_whitePlayer.ResetPositionAndVelocity(_level.whiteSpawn);
			_blackPlayer.ResetPositionAndVelocity(_level.blackSpawn);
		}
	}
}