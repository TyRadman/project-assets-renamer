using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace ProjectNamingTool
{
    public class ProjectNamingProcessor : AssetPostprocessor
    {
        private static Dictionary<string, string> _extensionPrefixMap = new Dictionary<string, string>();
        private const string FolderPath = "Assets/Editor Default Resources";
        private const string AssetPath = FolderPath + "/ProjectNamingData.asset";

        private static void EnsureDataInitialized()
        {
            if (_extensionPrefixMap.Count > 0)
            {
                return;
            }

            if (!AssetDatabase.IsValidFolder(FolderPath))
            {
                AssetDatabase.CreateFolder("Assets", "Editor Default Resources");
            }

            ProjectNamingData data = AssetDatabase.LoadAssetAtPath<ProjectNamingData>(AssetPath);

            if (data == null)
            {
                data = ScriptableObject.CreateInstance<ProjectNamingData>();
                data.LoadDefaults();
                AssetDatabase.CreateAsset(data, AssetPath);
                AssetDatabase.SaveAssets();
            }

            RefreshDictionary(data);
        }

        private static void RefreshDictionary(ProjectNamingData data)
        {
            _extensionPrefixMap.Clear();

            foreach (var rule in data.rules)
            {
                string extString = GetExtensionString(rule.extension);
             
                if (!_extensionPrefixMap.ContainsKey(extString))
                {
                    _extensionPrefixMap.Add(extString, rule.prefix);
                }
            }
        }

        private static string GetExtensionString(AssetExtension ext)
        {
            return ext switch
            {
                AssetExtension.Material => ".mat",
                AssetExtension.Scene => ".unity",
                AssetExtension.AudioMixer => ".mixer",
                AssetExtension.Shader => ".shader",
                AssetExtension.ShaderGraph => ".shadergraph",
                AssetExtension.ShaderSubGraph => ".shadersubgraph",
                AssetExtension.Prefab => ".prefab",
                AssetExtension.Prefab_Particle => ".v_sfx",
                AssetExtension.Prefab_UI => ".v_ui",
                AssetExtension.Model_Static => ".v_sm",
                AssetExtension.Model_Skeletal => ".v_sk",
                _ => "." + ext.ToString().ToLower()
            };
        }

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (string path in importedAssets)
            {
                if (path == AssetPath)
                {
                    _extensionPrefixMap.Clear();
                    break;
                }
            }

            foreach (string path in importedAssets)
            {
                if (path == AssetPath)
                {
                    continue;
                }

                Object mainAsset = AssetDatabase.LoadMainAssetAtPath(path);
                
                if (mainAsset == null || !AssetDatabase.IsMainAsset(mainAsset))
                {
                    continue;
                }

                string ext = Path.GetExtension(path).ToLower();
                string fileName = Path.GetFileNameWithoutExtension(path);

                EnsureDataInitialized();

                if (_extensionPrefixMap.TryGetValue(ext, out string prefix))
                {
                    ApplyRename(path, fileName, prefix);
                }
                else if (ext == ".prefab")
                {
                    EditorApplication.delayCall += () => HandlePrefabRename(path);
                }
                else if (ext == ".fbx" || ext == ".obj")
                {
                    EditorApplication.delayCall += () => HandleModelRename(path);
                }
            }
        }

        private static void ApplyRename(string path, string name, string prefix)
        {
            if (!name.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase))
            {
                EditorApplication.delayCall += () => SafeRename(path, prefix);
            }
        }

        private static void HandlePrefabRename(string path)
        {
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (go == null)
            {
                return;
            }

            EnsureDataInitialized();
            string key = GetExtensionString(AssetExtension.Prefab);

            if (go.GetComponent<ParticleSystem>() != null)
            {
                key = GetExtensionString(AssetExtension.Prefab_Particle);
            }
            else if (go.GetComponent<RectTransform>() != null)
            {
                key = GetExtensionString(AssetExtension.Prefab_UI);
            }

            if (_extensionPrefixMap.TryGetValue(key, out string prefix))
            {
                SafeRename(path, prefix);
            }
        }

        private static void HandleModelRename(string path)
        {
            GameObject model = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (model == null)
            {
                return;
            }

            EnsureDataInitialized();
            bool isSkeletal = model.GetComponentInChildren<SkinnedMeshRenderer>() != null;

            string key = isSkeletal ?
                GetExtensionString(AssetExtension.Model_Skeletal) :
                GetExtensionString(AssetExtension.Model_Static);

            if (_extensionPrefixMap.TryGetValue(key, out string prefix))
            {
                SafeRename(path, prefix);
            }
        }

        private static void SafeRename(string path, string prefix)
        {
            string fileName = Path.GetFileNameWithoutExtension(path);
            
            if (fileName.StartsWith(prefix, System.StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            AssetDatabase.RenameAsset(path, prefix + fileName);
        }
    }
}