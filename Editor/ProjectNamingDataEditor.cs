using UnityEditor;
using UnityEngine;
using ProjectNamingTool;

[CustomEditor(typeof(ProjectNamingData))]
public class ProjectNamingDataEditor : Editor
{
    private ProjectNamingData _data;

    private void OnEnable() => _data = (ProjectNamingData)target;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Project Naming Rules", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Define prefixes for asset extensions. Complex types like Prefabs and Models are handled by internal logic.", MessageType.Info);
        EditorGUILayout.Space(5);

        // Header Row
        using (new EditorGUILayout.HorizontalScope(EditorStyles.helpBox))
        {
            EditorGUILayout.LabelField("Extension", EditorStyles.miniBoldLabel, GUILayout.Width(120));
            EditorGUILayout.LabelField("Prefix", EditorStyles.miniBoldLabel);
            GUILayout.Space(30); // Space for delete button
        }

        // List Rows
        for (int i = 0; i < _data.rules.Count; i++)
        {
            var rule = _data.rules[i];

            using (new EditorGUILayout.HorizontalScope())
            {
                // Enum Field
                AssetExtension newExt = (AssetExtension)EditorGUILayout.EnumPopup(rule.extension, GUILayout.Width(120));

                // Prefix Field (Text Area style but single line)
                string newPrefix = EditorGUILayout.TextField(rule.prefix);

                // Update data if changed
                if (newExt != rule.extension || newPrefix != rule.prefix)
                {
                    Undo.RecordObject(_data, "Update Naming Rule");
                    _data.rules[i] = new NamingRule { extension = newExt, prefix = newPrefix };
                }

                // Delete Button
                GUI.backgroundColor = new Color(1f, 0.4f, 0.4f);
                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    Undo.RecordObject(_data, "Delete Naming Rule");
                    _data.rules.RemoveAt(i);
                    break; // Exit loop to avoid index errors
                }
                GUI.backgroundColor = Color.white;
            }
        }

        EditorGUILayout.Space(10);

        // Add Button at Bottom
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+ Add New Rule", GUILayout.Width(150), GUILayout.Height(25)))
            {
                Undo.RecordObject(_data, "Add Naming Rule");
                _data.rules.Add(new NamingRule());
            }
            GUILayout.FlexibleSpace();
        }

        serializedObject.ApplyModifiedProperties();

        // Excluded Folders Section
        EditorGUILayout.Space(15);
        EditorGUILayout.LabelField("Excluded Folders", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Assets inside these folders will not be renamed. Drag folders from the Project window or type paths manually (e.g. Assets/Plugins/FMOD).", MessageType.Info);
        EditorGUILayout.Space(5);

        for (int i = 0; i < _data.excludedFolders.Count; i++)
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                // Folder object field for drag-and-drop
                string currentPath = _data.excludedFolders[i];
                Object folderObj = string.IsNullOrEmpty(currentPath) ? null : AssetDatabase.LoadAssetAtPath<Object>(currentPath);
                Object newFolderObj = EditorGUILayout.ObjectField(folderObj, typeof(DefaultAsset), false);

                if (newFolderObj != folderObj)
                {
                    string newPath = AssetDatabase.GetAssetPath(newFolderObj);
                    if (AssetDatabase.IsValidFolder(newPath))
                    {
                        Undo.RecordObject(_data, "Update Excluded Folder");
                        _data.excludedFolders[i] = newPath;
                    }
                }

                // Also allow manual text editing
                string manualPath = EditorGUILayout.TextField(currentPath, GUILayout.MinWidth(150));
                if (manualPath != currentPath)
                {
                    Undo.RecordObject(_data, "Update Excluded Folder");
                    _data.excludedFolders[i] = manualPath;
                }

                // Delete Button
                GUI.backgroundColor = new Color(1f, 0.4f, 0.4f);
                if (GUILayout.Button("X", GUILayout.Width(25)))
                {
                    Undo.RecordObject(_data, "Remove Excluded Folder");
                    _data.excludedFolders.RemoveAt(i);
                    break;
                }
                GUI.backgroundColor = Color.white;
            }
        }

        EditorGUILayout.Space(5);
        using (new EditorGUILayout.HorizontalScope())
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("+ Add Excluded Folder", GUILayout.Width(180), GUILayout.Height(25)))
            {
                Undo.RecordObject(_data, "Add Excluded Folder");
                _data.excludedFolders.Add("");
            }
            GUILayout.FlexibleSpace();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(_data);
        }
    }
}
