using UnityEngine;

namespace Symmetry {
	public class Level : MonoBehaviour {
		[SerializeField]                           protected string         _levelName;
		[SerializeField]                           protected string         _nextLevelName;
		[Header("Blocks")] [SerializeField]        protected Transform      _blocksContainer;
		[SerializeField]                           protected SpriteRenderer _blockPrefab;
		[Header("Spawns")] [SerializeField]        protected Transform      _whiteSpawn;
		[SerializeField]                           protected Transform      _blackSpawn;
		[Header("Interactables")] [SerializeField] protected Transform      _interactablesContainer;
		[SerializeField]                           protected LevelExit      _levelExitPrefab;
		[SerializeField]                           protected LevelMalus     _levelMalusPrefab;

		public string levelName => _levelName;

		public string nextLevelName {
			get => _nextLevelName;
			set => _nextLevelName = value;
		}

		public SpriteRenderer blockPrefab            => _blockPrefab;
		public Transform      blocksContainer        => _blocksContainer;
		public Transform      whiteSpawn             => _whiteSpawn;
		public Transform      blackSpawn             => _blackSpawn;
		public Transform      interactablesContainer => _interactablesContainer;
		public LevelExit      levelExitPrefab        => _levelExitPrefab;
		public LevelMalus     levelMalusPrefab       => _levelMalusPrefab;

		[ContextMenu("Save level")] private void SaveLevel() => LevelData.Save(this);
		[ContextMenu("Load level")] private void LoadLevel() => LevelData.Load(this);

		[ContextMenu("Load next level")]
		public void LoadNextLevel() {
			_levelName = _nextLevelName;
			LoadLevel();
		}
	}
}