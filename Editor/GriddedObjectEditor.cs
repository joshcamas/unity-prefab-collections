using UnityEngine;
using UnityEditor;

namespace Ardenfall
{
    [CustomEditor(typeof(GriddedObject), true)]
    public class GriddedObjectEditor : Editor
    {
        private bool dropdownVisual = false;

        public override void OnInspectorGUI()
        {
            GriddedObject myTarget = (GriddedObject)target;

            myTarget.gridSize = EditorGUILayout.Vector3Field("Grid Size", myTarget.gridSize);
            myTarget.offset = EditorGUILayout.Vector3Field("Offset", myTarget.offset);

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.PrefixLabel("Enabling");

            float oldWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 12;

            myTarget.enableX = EditorGUILayout.Toggle("X", myTarget.enableX,GUILayout.Width(30));
            myTarget.enableY = EditorGUILayout.Toggle("Y", myTarget.enableY, GUILayout.Width(30));
            myTarget.enableZ = EditorGUILayout.Toggle("Z", myTarget.enableZ, GUILayout.Width(30));

            EditorGUIUtility.labelWidth = oldWidth;

            EditorGUILayout.EndHorizontal();

            myTarget.enableOnlyInGriddedContainer = EditorGUILayout.Toggle("Only enable in GriddedContainer", myTarget.enableOnlyInGriddedContainer);

            dropdownVisual = EditorGUILayout.Foldout(dropdownVisual, "Grid Visual",true);

            if(dropdownVisual)
            {
                myTarget.gridVisualSize = EditorGUILayout.IntField("Size", myTarget.gridVisualSize);
                myTarget.gridColor = EditorGUILayout.ColorField("Color", myTarget.gridColor);
            }
        }
    }
}

