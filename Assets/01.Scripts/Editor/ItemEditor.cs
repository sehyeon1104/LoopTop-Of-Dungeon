using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemEditor : EditorWindow
{
    [MenuItem("CustomEditor/Item")]
    public static void Init()
    {
        Debug.Log("ItemEditor Init");
        GetWindow<ItemEditor>();
    }

    private void OnGUI()
    {
        
    }
}
