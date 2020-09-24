using UnityEngine.Events;

namespace Symmetry {
	public static class LevelEventSystem {
		public static UnityEvent onExitEntered { get; } = new UnityEvent();
		public static UnityEvent onExitExited  { get; } = new UnityEvent();
	}
}