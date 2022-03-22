using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

[CustomPropertyDrawer(typeof(MapSpaceInfo))]
public class CustomSpaceInspector : PropertyDrawer
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
        Rect spaceRect = new Rect(position.x, position.y, width / 10 * 6, height);
        Rect xRect = new Rect(position.x + width / 10 * 6, position.y, width / 10, height);
        Rect yRect = new Rect(position.x + width / 10 * 7, position.y, width / 10, height);
        Rect idRect = new Rect(position.x + width / 10 * 8, position.y, width / 10, height);
        Rect recruitRect = new Rect(position.x + width / 10 * 9, position.y, width / 10, height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(spaceRect, property.FindPropertyRelative("space"), GUIContent.none);
        EditorGUI.PropertyField(xRect, property.FindPropertyRelative("xPos"), GUIContent.none);
        EditorGUI.PropertyField(yRect, property.FindPropertyRelative("yPos"), GUIContent.none);
        EditorGUI.PropertyField(idRect, property.FindPropertyRelative("country"), GUIContent.none);
        EditorGUI.PropertyField(recruitRect, property.FindPropertyRelative("recruitType"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
