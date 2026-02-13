# Unity Project Assets Rename

A Unity editor tool that automatically renames imported assets and newly created prefabs using configurable filename prefixes based on file type.
With this tool the room for error is eliminated and teams can focus on working on the assets rather than referring to documents to check how to name them.

## Setup
To import the tool package into the project:
- Go to `Window->Package Manager` or, for Unity 6, `Window->Package Management->Package Manager`.
- Click on the `+` button at the top left of the package manager window.
- Select `Import package from Git URL...`.
- Paste this as the URL: `https://github.com/TyRadman/project-assets-renamer.git` 

## How to use
Once the package is installed, assets imported or dragged from the scene will be automatically prefixed with what I normally prefix my assets with.<br>
You can view, change, and add settings using these steps:
- Open the scriptable object at `Assets/Editor Default Resources/ProjectNamingData`.
- The scriptable object has a list of entries where every entry has an file extension type, prefix text field, and a delete button to remove the entry.
- Use the `Add new rule +` button to add a new asset naming convention.

## Default namings

| File type| Prefix|
|-|-|
| Material         | M_       |
| Scene            | S_       |
| Audio Mixer      | AM_      |
| Shader           | Shader_  |
| Shader Graph     | SG_      |
| PNG              | T_       |
| JPG              | T_       |
| WAV              | A_       |
| Prefab           | P_       |
| Prefab_Particle  | SFX_     |
| Prefab_UI        | UI_      |
| Model_Static     | SM_      |
| Model_Skeletal   | SK_      |

## Contribution
Feel free to push this as far as possible. The sky is the limit :)