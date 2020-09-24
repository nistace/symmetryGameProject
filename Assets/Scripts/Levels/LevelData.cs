using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Symmetry {
	[CreateAssetMenu(menuName = "LevelData")]
	public class LevelData : ScriptableObject {
		private const              string           assetPath = "Assets/Levels";
		[SerializeField] protected string           _nextLevelName;
		[SerializeField] protected Vector2          _whiteSpawnPosition;
		[SerializeField] protected Vector2          _blackSpawnPosition;
		[SerializeField] protected LevelBlockData[] _blocks;
		[SerializeField] protected Vector2[]        _exits;
		[SerializeField] protected Vector2[]        _malus;

		public static void Save(Level level) {
			var data = CreateInstance<LevelData>();
			data._nextLevelName = level.nextLevelName;
			data._blocks = level.blocksContainer.Children().SelectNotNullComponents<SpriteRenderer>().Select(LevelBlockData.Create).ToArray();
			data._exits = level.interactablesContainer.Children().SelectNotNullComponents<LevelExit>().Select(t => (Vector2) t.transform.position).ToArray();
			data._malus = level.interactablesContainer.Children().SelectNotNullComponents<LevelMalus>().Select(t => (Vector2) t.transform.position).ToArray();
			data._whiteSpawnPosition = level.whiteSpawn.position;
			data._blackSpawnPosition = level.blackSpawn.position;
			AssetDatabase.CreateAsset(data, $"{assetPath}/{level.levelName}.asset");
			AssetDatabase.SaveAssets();
		}

		public static void Load(Level level) {
			var data = AssetDatabase.LoadAssetAtPath<LevelData>($"{assetPath}/{level.levelName}.asset");
			if (!data) {
				Debug.LogError($"No data named {level.levelName}. Abort.");
				return;
			}
			level.blocksContainer.ClearChildren();
			level.interactablesContainer.ClearChildren();
			level.nextLevelName = data._nextLevelName;
			level.whiteSpawn.position = data._whiteSpawnPosition;
			level.blackSpawn.position = data._blackSpawnPosition;
			data._blocks?.ForEach(t => t.Load(Instantiate(level.blockPrefab, level.blocksContainer)));
			data._exits?.ForEach(t => Instantiate(level.levelExitPrefab, t, Quaternion.identity, level.interactablesContainer));
			data._malus?.ForEach(t => Instantiate(level.levelMalusPrefab, t, Quaternion.identity, level.interactablesContainer));
		}

		[Serializable]
		public class LevelBlockData {
			[SerializeField] protected int     _layer;
			[SerializeField] protected Color   _color    = Color.white;
			[SerializeField] protected Vector2 _scale    = Vector2.one;
			[SerializeField] protected Vector2 _position = Vector2.one;
			[SerializeField] protected int     _orderInLayer;

			public static LevelBlockData Create(SpriteRenderer blockRenderer) {
				var blockTransform = blockRenderer.transform;
				return new LevelBlockData {
					_layer = blockRenderer.gameObject.layer,
					_color = blockRenderer.color,
					_scale = blockTransform.localScale,
					_position = blockTransform.position,
					_orderInLayer = blockRenderer.sortingOrder
				};
			}

			public void Load(SpriteRenderer blockRenderer) {
				var blockTransform = blockRenderer.transform;
				blockRenderer.gameObject.layer = _layer;
				blockRenderer.color = _color;
				blockTransform.localScale = _scale;
				blockTransform.position = _position;
				blockRenderer.sortingOrder = _orderInLayer;
			}
		}
	}
}