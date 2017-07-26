using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(AttackInfo))]
public class AttackInfoDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var contentPosition = EditorGUI.PrefixLabel(position, label);

        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 1;

        var descriptionRect = new Rect(position.x, position.y, 100, position.height);
        var damageRect = new Rect(position.x + 100, position.y, 50, position.height);
        var damageTypeRect = new Rect(position.x + 150, position.y, 100, position.height);


        EditorGUI.PropertyField(descriptionRect, property.FindPropertyRelative("Description"), GUIContent.none);
        EditorGUI.PropertyField(damageRect, property.FindPropertyRelative("Damage"), GUIContent.none);
        EditorGUI.PropertyField(damageTypeRect, property.FindPropertyRelative("DamageType"), GUIContent.none);


        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
