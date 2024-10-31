using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Character))]
public class CharacterEditor : Editor
{
    protected SerializedProperty nameTextProp;
    protected SerializedProperty nameColourProp;
    protected SerializedProperty soundEffectProp;
    protected SerializedProperty portraitsProp;
    protected SerializedProperty portraitsFaceProp;
    protected SerializedProperty unlockedProp;
    protected SerializedProperty talkSpeedProp;

    protected virtual void OnEnable()
    {
        nameTextProp = serializedObject.FindProperty("characterName");
        nameColourProp = serializedObject.FindProperty("nameColour");
        soundEffectProp = serializedObject.FindProperty("characterSound");
        portraitsProp = serializedObject.FindProperty("characterPortraits");
        portraitsFaceProp = serializedObject.FindProperty("facingDirection");
        unlockedProp = serializedObject.FindProperty("unlocked");
        talkSpeedProp = serializedObject.FindProperty("talkSpeed");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        Character t = target as Character;
        EditorGUI.BeginChangeCheck();

        EditorGUILayout.PropertyField(nameTextProp, new GUIContent("Character Name", "Name of the character"));
        EditorGUILayout.PropertyField(nameColourProp, new GUIContent("Name Colour", "Colour of name text display"));
        EditorGUILayout.PropertyField(soundEffectProp, new GUIContent("Character Sound", "Sound to play when the character is talking. Overrides the setting in the dialogue box."));

        if (t.Portraits != null && t.Portraits.Count > 0)
        {
            t.ProfileSprite = t.Portraits[0];
        }
        else
        {
            t.ProfileSprite = null;
        }

        if (t.ProfileSprite != null)
        {
            Texture2D characterTexture = t.ProfileSprite.texture;
            float aspect = (float)characterTexture.width / (float)characterTexture.height;
            Rect previewRect = GUILayoutUtility.GetAspectRect(aspect, GUILayout.Width(100), GUILayout.ExpandWidth(true));
            if (characterTexture != null)
                GUI.DrawTexture(previewRect, characterTexture, ScaleMode.ScaleAndCrop, true, aspect);
        }

        EditorGUILayout.PropertyField(portraitsProp, new GUIContent("Portraits", "Character image sprites to choose from in dialogue box"), true);

        EditorGUILayout.HelpBox("All portrait images should use the exact same resolution to avoid positioning and tiling issues.", MessageType.Info);

        EditorGUILayout.Separator();

        string[] facingArrows = new string[]
        {
                "FRONT",
                "<--",
                "-->",
        };
        portraitsFaceProp.enumValueIndex = EditorGUILayout.Popup("Portraits Face", (int)portraitsFaceProp.enumValueIndex, facingArrows);

        EditorGUILayout.PropertyField(unlockedProp, new GUIContent("Unlocked", "Whether this character is unlocked (Profile is viewable) by default."));
        
        EditorGUILayout.PropertyField(talkSpeedProp, new GUIContent("Talk Speed", "A multiplier for how quickly the character talks."));

        EditorGUILayout.Separator();

        if (EditorGUI.EndChangeCheck())
            EditorUtility.SetDirty(t);

        serializedObject.ApplyModifiedProperties();
    }
}
