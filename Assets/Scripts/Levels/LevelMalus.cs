using UnityEngine;

namespace Symmetry {
	public class LevelMalus : MonoBehaviour {
		private void OnTriggerEnter2D(Collider2D other) {
			if (!other.gameObject.TryGetComponent<Player>(out var player)) return;
			player.Destroy();
		}
	}
}