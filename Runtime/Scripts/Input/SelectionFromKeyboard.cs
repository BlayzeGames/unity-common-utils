using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace CommonUtils.Editor.CustomEditors
{
	/// <summary>
	/// Allows objects to be selectable using the keyboard.
	/// </summary>
	public class SelectionFromKeyboard : MonoBehaviour {
		/// <summary>
		/// Object that can be selected through keyboard using this object.
		/// </summary>
		public Animation[] childrenToSelect;
		// Solo en caso de que haya botones que se instancien en runtime
		public GameObject ParentContainer;

		/// <summary>
		/// Objects that can block this button when they're active.
		/// </summary>
		public List<GameObject> IsBlockedBy;

		[Tooltip("Key used to simulate button click.")]
		public KeyCode KeyCode;

		/// <summary>
		/// Únicamente se le debe asignar una tecla si se utiliza la modalidad de seleccion de hijos
		/// </summary>
		[Tooltip("Key to select gameobjects")]
		public KeyCode KeyCodeSelection;

		private int childrenIndex;
	
		#region Unity Lifecycle
		private void Awake() {
			if (IsBlockedBy.Any(blocker => blocker == null)) {
				Debug.LogWarningFormat("{0} is being blocked by an invalid object.", name);
			}
		}

		private void OnEnable() {
			// En caso de que se carguen objetos en timepo de ejecución y deba colocarlo a la lista de elementos seleccionables
			if (ParentContainer != null) {

				int i = childrenToSelect.Length;

				Button[] children = ParentContainer.GetComponentsInChildren<Button>().Where(p => p.IsInteractable()).ToArray();
				Array.Resize(ref childrenToSelect, (childrenToSelect.Length + children.Length));

				foreach (Button c in children) {
					childrenToSelect[i] = c.GetComponent<Animation>();
					i++;
				}

			}
			childrenIndex = -1;
		}

		private void Update() {
			if (KeyCodeSelection != KeyCode && !IsBlocked()) {
				if (UnityEngine.Input.GetKeyUp(KeyCodeSelection)) { // SELECTOR HIJO
					MoveToTheNextChild();
				}
				else {
					if (UnityEngine.Input.GetKeyUp(KeyCode) && 
					    childrenIndex != -1 && 
					    childrenToSelect[childrenIndex].gameObject.activeSelf && 
					    childrenToSelect[childrenIndex].GetComponent<Button>().IsInteractable() && 
					    !IsBlocked()) { // CONFIRMAR SELECCION

						childrenToSelect[childrenIndex].GetComponent<Button>().onClick.Invoke();

						if (childrenToSelect[childrenIndex].name == "Frame") { MoveToTheNextChild(); }
					}

				}
			}
		}
	
		/// <summary>
		/// Reinicio el selector cuando se oculta la pantalla.
		/// </summary>
		public void OnDisable() {
			if (childrenIndex != -1) {
				childrenToSelect[childrenIndex].transform.localScale = new Vector3(1, 1, 1);
				childrenToSelect[childrenIndex].enabled = false;
				childrenIndex = -1;
			}
		}
		#endregion

		/// <summary>
		/// Método que controla el movimiento al siguiente hijo disponible
		/// </summary>
		private void MoveToTheNextChild() {
			Debug.LogWarning($"La funcionalidad de {nameof(MoveToTheNextChild)} ha sido desactivada temporalmente", this);
			/*if (childrenIndex != -1) {
			childrenToSelect[childrenIndex].enabled = false;
			childrenToSelect[childrenIndex].transform.localScale = new Vector3(1, 1, 1);
		}

		bool flag = false;
		while (flag == false) {

			childrenIndex = childrenIndex >= childrenToSelect.Length - 1 || childrenIndex == -1 ? 0 : childrenIndex + 1;

			if (childrenToSelect[childrenIndex].GetComponent<Button>().IsInteractable() && childrenToSelect[childrenIndex].gameObject.activeSelf) {

				if (childrenToSelect[childrenIndex].name == "Frame") { //Control para no seleccionar los reels si no esta funcionando

					try {
						if (childrenToSelect[childrenIndex].GetComponentInChildren<ReelView>().isMoving) {
							childrenToSelect[childrenIndex].enabled = true;
							flag = true;
						}
					}
					catch { }

				}
				else {
					childrenToSelect[childrenIndex].enabled = true;
					flag = true;
				}
			}

		}
*/
		}

		/// <summary>
		/// Gets a value indicating wether any of the blockers are active or not.
		/// </summary>
		/// <returns><c>true</c> when any of the blockers is active, otherwise <c>false</c>.</returns>
		public bool IsBlocked() {
			if (IsBlockedBy == null || !IsBlockedBy.Any()) return false;
			if(IsBlockedBy.Any(blocker => blocker == null)) {
				Debug.LogError($"{name} está bloqueado por uno o más objetos inválidos.", this);
			}
			return IsBlockedBy.Any(blocker => blocker != null && blocker.activeInHierarchy);
		}
	}
}