using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumBasedArrayEditor : PropertyAttribute
{
    public string[] names;
    public EnumBasedArrayEditor(System.Type names_enum_type)
    {
        this.names = System.Enum.GetNames(names_enum_type);
    }
}
