using UnityEditor;
using UnityEngine;

namespace GOAP {
    [CustomPropertyDrawer(typeof(EffectState))]
    public class EffectStatePropertyDrawer : PropertyDrawer {

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            Rect valueRect = new Rect(position.x, position.y, 30, position.height);
            Rect effectRect = new Rect(position.x + 20, position.y, position.width - 20, position.height);

            EditorGUI.PropertyField(valueRect, property.FindPropertyRelative("value"), GUIContent.none);
            EditorGUI.PropertyField(effectRect, property.FindPropertyRelative("effect"), GUIContent.none);

            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
}