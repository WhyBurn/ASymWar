using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

[CustomPropertyDrawer(typeof(MapBorderInfo))]
public class CustomBorderDisplay : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        float width = position.width;
        float height = position.height;
        Rect spaceRect = new Rect(position.x, position.y, width / 2, height);
        Rect sourceRect = new Rect(position.x + width / 2, position.y, width / 4, height);
        Rect destRect = new Rect(position.x + 3 * width / 4, position.y, width / 4, height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(spaceRect, property.FindPropertyRelative("border"), GUIContent.none);
        EditorGUI.PropertyField(sourceRect, property.FindPropertyRelative("source"), GUIContent.none);
        EditorGUI.PropertyField(destRect, property.FindPropertyRelative("destination"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
