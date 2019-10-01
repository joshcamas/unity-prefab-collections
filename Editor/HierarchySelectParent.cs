using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Ardenfall
{
    [InitializeOnLoad]
    public class HierarchySelectParent
    {
        static HierarchySelectParent()
        {
            EditorApplication.hierarchyWindowItemOnGUI += HandleHierarchyWindowItemOnGUI;
        }
        private static void HandleHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            if (Application.isPlaying)
                return;

            GameObject theObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (theObject == null)
                return;

            if (theObject.name.Contains("[SelectParent]") && Selection.Contains(instanceID) && Event.current.shift == false)
            {
                if (theObject.transform.parent == null)
                    return;

                List<Object> objects = new List<Object>(Selection.objects);
                objects.Remove(theObject);
                objects.Add(theObject.transform.parent.gameObject);
                Selection.objects = objects.ToArray();
            }
                
        }
    }

}