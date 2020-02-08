using UnityEditor;
using UnityEngine;

public class PrefabReplacer : EditorWindow {
	[SerializeField]
	private GameObject prefab;

	[MenuItem("Tools/Prefab Replacer")]
	private static void CreateReplaceWithPrefab() { GetWindow<PrefabReplacer>(); }

	private void OnGUI() {
		prefab = (GameObject) EditorGUILayout.ObjectField("Replace selected with", prefab, typeof(GameObject), false);

		if (prefab) {
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
					newObject.transform.localPosition = selected.transform.localPosition;
					newObject.transform.localRotation = selected.transform.localRotation;
					newObject.transform.localScale    = selected.transform.localScale;
					newObject.transform.SetSiblingIndex(selected.transform.GetSiblingIndex());
					Undo.DestroyObjectImmediate(selected);
				}
			}
		}
		
		EditorGUILayout.LabelField("Selected objects: " + Selection.objects.Length);
	}

	private void OnSelectionChange() {
		Repaint();
	}
}