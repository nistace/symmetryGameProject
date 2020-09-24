using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Utils.Audio;

namespace Symmetry {
	public class Player : MonoBehaviour {
		[SerializeField] protected Rigidbody2D    _rigidbody;
		[SerializeField] protected GameObject     _visuals;
		[SerializeField] protected float          _acceleration = 1;
		[SerializeField] protected float          _jumpForce;
		[SerializeField] protected ParticleSystem _destructionParticles;
		[SerializeField] protected AudioClip      _destructionAudioClip;

		private float               movementInput  { get; set; }
		private List<CollisionInfo> collisionsInfo { get; } = new List<CollisionInfo>();
		private bool                isJumpAllowed  => collisionsInfo.Count > 0;

		private void OnEnable() => SetListenersEnabled(true);
		private void OnDisable() => SetListenersEnabled(false);

		private void SetListenersEnabled(bool enabled) {
			Inputs.controls.Player.HorizontalMovement.SetAnyListenerOnce(HandleMovementChanged, enabled);
			Inputs.controls.Player.Jump.SetPerformListenerOnce(HandleJump, enabled);
		}

		private void HandleJump(InputAction.CallbackContext obj) {
			if (!isJumpAllowed) return;
			_rigidbody.AddForce(_jumpForce * collisionsInfo[0].normal);
		}

		private void HandleMovementChanged(InputAction.CallbackContext obj) => movementInput = obj.ReadValue<float>();

		private void FixedUpdate() {
			if (movementInput == 0) return;
			_rigidbody.AddForce(_acceleration * movementInput * Vector2.right);
		}

		private void OnCollisionEnter2D(Collision2D other) {
			var collisionInfo = new CollisionInfo(other);
			var insertIndex = 0;
			while (insertIndex < collisionsInfo.Count && collisionsInfo[insertIndex].absNormalCosine < collisionInfo.absNormalCosine) insertIndex++;
			collisionsInfo.Insert(insertIndex, collisionInfo);
		}

		private void OnCollisionExit2D(Collision2D other) {
			var collisionInfo = new CollisionInfo(other);
			for (var i = 0; i < collisionsInfo.Count; ++i) {
				if (!Equals(collisionInfo, collisionsInfo[i])) continue;
				collisionsInfo.RemoveAt(i);
				i--;
			}
		}

		private class CollisionInfo {
			private GameObject gameObject      { get; }
			public  Vector2    normal          { get; }
			public  float      absNormalCosine { get; }

			public CollisionInfo(Collision2D collision2D) {
				gameObject = collision2D.gameObject;
				normal = collision2D.contacts.Length > 0 ? collision2D.contacts[0].normal : Vector2.up;
				absNormalCosine = Mathf.Abs(Mathf.Cos(Vector2.Angle(normal, Vector2.right)));
			}

			public override bool Equals(object obj) => obj is CollisionInfo info && Equals(info);
			private bool Equals(CollisionInfo other) => Equals(gameObject, other.gameObject);
			public override int GetHashCode() => (gameObject != null ? gameObject.GetHashCode() : 0);
		}

		public void ResetPositionAndVelocity(Transform spawn) {
			transform.position = spawn.position;
			_rigidbody.velocity = Vector2.zero;
			_rigidbody.angularVelocity = 0;
			_visuals.SetActive(true);
			movementInput = 0;
			enabled = true;
		}

		public void Destroy() {
			_destructionParticles.Play();
			AudioManager.Sfx.Play(_destructionAudioClip);
			_visuals.SetActive(false);
			_rigidbody.velocity = Vector2.zero;
			_rigidbody.angularVelocity = 0;
			movementInput = 0;
			enabled = false;
		}
	}
}