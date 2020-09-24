using UnityEngine;

namespace Symmetry {
	public class LevelExit : MonoBehaviour {
		private void OnTriggerEnter2D(Collider2D other) => LevelEventSystem.onExitEntered.Invoke();
		private void OnTriggerExit2D(Collider2D other) => LevelEventSystem.onExitExited.Invoke();
	}
}