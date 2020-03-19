using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class PrefabReplacer : EditorWindow {
	[SerializeField] private GameObject prefab;
	[SerializeField] private Vector3 positionOffset;
	[SerializeField] private Vector3 rotationOffset;
	[SerializeField] private Vector3 scaleOffset;
	[SerializeField] private bool doNotRemoveOriginal;

	[MenuItem("Tools/Prefab Replacer")]
	private static void CreateReplaceWithPrefab() { GetWindow<PrefabReplacer>(); }

	private void OnGUI() {
		prefab = (GameObject) EditorGUILayout.ObjectField("Replace selected with", prefab, typeof(GameObject), false);

		if (prefab) {
			positionOffset = EditorGUILayout.Vector3Field("Position offset", positionOffset);
			rotationOffset = EditorGUILayout.Vector3Field("Rotation offset", rotationOffset);
			scaleOffset = EditorGUILayout.Vector3Field("Scale offset", scaleOffset);
			doNotRemoveOriginal = EditorGUILayout.Toggle("Do not remove original objects", doNotRemoveOriginal);
			
			if (GUILayout.Button("Replace")) {
				var selection = Selection.gameObjects;

				for (var i = selection.Length - 1; i >= 0; --i) {
					var        selected   = selection[i];
					GameObject newObject;
					
					var prefabType = PrefabUtility.GetPrefabInstanceStatus(prefab);
					if (prefabType == PrefabInstanceStatus.NotAPrefab) {
						newObject = (GameObject) PrefabUtility.InstantiatePrefab(prefab);
					} else {
						newObject      = Instantiate(prefab);
						newObject.name = prefab.name;
					}

					if (newObject == null) {
						Debug.LogError("Error instantiating prefab");
						break;
					}

					Undo.RegisterCreatedObjectUndo(newObject, "Replace Prefabs");
					newObject.transform.parent        = selected.transform.parent;
					newObject.transform.localPosition = selected.transform.localPosition + positionOffset;
					newObject.transform.localRotation = Quaternion.Euler(selected.transform.localRotation.eulerAngles + rotationOffset);
					newObject.transform.localScale    = selected.transform.localScale + scaleOffset;
					newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());
					newObject.SetActive(selected.activeSelf);
					if(!doNotRemoveOriginal) Undo.DestroyObjectImmediate(selected);
				}
			}
		}
		
		EditorGUILayout.LabelField("Selected objects: " + Selection.objects.Length);
	}

	private void OnSelectionChange() {
		Repaint();
	}
}