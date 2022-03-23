using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using UnityEngine;

[CustomPropertyDrawer(typeof(CountryStartUnit))]
public class CustomUnitInspector : PropertyDrawer
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
        Rect unitRect = new Rect(position.x, position.y, width / 2, height);
        Rect spaceRect = new Rect(position.x + width / 2, position.y, width / 4, height);
        Rect idRect = new Rect(position.x + width / 4 * 3, position.y, width / 4, height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(unitRect, property.FindPropertyRelative("unit"), GUIContent.none);
        EditorGUI.PropertyField(spaceRect, property.FindPropertyRelative("space"), GUIContent.none);
        EditorGUI.PropertyField(idRect, property.FindPropertyRelative("country"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
