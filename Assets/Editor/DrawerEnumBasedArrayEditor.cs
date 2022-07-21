using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomPropertyDrawer(typeof(EnumBasedArrayEditor))]
public class DrawerEnumBasedArrayEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EnumBasedArrayEditor enumNames = attribute as EnumBasedArrayEditor;
        //propertyPath returns something like component_hp_max.Array.data[4]
        //so get the index from there
        int index = System.Convert.ToInt32(property.propertyPath.Substring(property.propertyPath.IndexOf("[")).Replace("[", "").Replace("]", ""));
        //change the label
        label.text = enumNames.names[index];
        //draw field
        EditorGUI.PropertyField(position, property, label, true);
    }
}

