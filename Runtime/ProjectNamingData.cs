using System.Collections.Generic;
using UnityEngine;

namespace ProjectNamingTool
{
    [CreateAssetMenu(fileName = "ProjectNamingData", menuName = "Settings/Project Naming Data")]
    public class ProjectNamingData : ScriptableObject
    {
        public List<NamingRule> rules = new List<NamingRule>();
        public List<string> excludedFolders = new List<string>();

        public void LoadDefaults()
        {
            rules = new List<NamingRule>
        {
            new NamingRule { extension = AssetExtension.Material, prefix = "M_" },
            new NamingRule { extension = AssetExtension.Scene, prefix = "S_" },
            new NamingRule { extension = AssetExtension.AudioMixer, prefix = "AM_" },
            new NamingRule { extension = AssetExtension.Shader, prefix = "Shader_" },
            new NamingRule { extension = AssetExtension.ShaderGraph, prefix = "SG_" },
            new NamingRule { extension = AssetExtension.PNG, prefix = "T_" },
            new NamingRule { extension = AssetExtension.JPG, prefix = "T_" },
            new NamingRule { extension = AssetExtension.WAV, prefix = "A_" },
            new NamingRule { extension = AssetExtension.Prefab, prefix = "P_" },
            new NamingRule { extension = AssetExtension.Prefab_Particle, prefix = "SFX_" },
            new NamingRule { extension = AssetExtension.Prefab_UI, prefix = "UI_" },
            new NamingRule { extension = AssetExtension.Model_Static, prefix = "SM_" },
            new NamingRule { extension = AssetExtension.Model_Skeletal, prefix = "SK_" }
        };
        }
    }

    [System.Serializable]
    public struct NamingRule
    {
        public AssetExtension extension;
        public string prefix;
    }

    public enum AssetExtension
    {
        PNG = 0,
        JPG = 1,
        TGA = 2,
        PSD = 3,
        TIFF = 4,
        EXR = 5,
        HDR = 6,

        Material = 100,
        Shader = 101,
        ShaderGraph = 102,
        ShaderSubGraph = 103,

        WAV = 200,
        MP3 = 201,
        OGG = 202,
        AIF = 203,

        Scene = 300,
        AudioMixer = 301,

        Prefab = 400,
        Prefab_Particle = 401,
        Prefab_UI = 402,
        Model_Static = 403,
        Model_Skeletal = 404
    }
}