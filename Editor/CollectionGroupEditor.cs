using UnityEngine;
using UnityEditor;

namespace Ardenfall
{
    [CustomEditor(typeof(CollectionGroup), true)]
    public class CollectionGroupEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
                return;

            CollectionGroup myTarget = (CollectionGroup)target;

            foreach (Collection c in myTarget.GetChildren())
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(c.name,EditorStyles.boldLabel);

                if (GUILayout.Button("<-", GUILayout.ExpandWidth(false)))
                    c.Step(-1);

                if (GUILayout.Button("->", GUILayout.ExpandWidth(false)))
                    c.Step(1);

                if (GUILayout.Button("Randomize", GUILayout.ExpandWidth(false)))
                    c.Randomize();

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();

            //Children
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("All Children");

            if (GUILayout.Button("<-", GUILayout.ExpandWidth(false)))
                myTarget.Step(-1);

            if (GUILayout.Button("->", GUILayout.ExpandWidth(false)))
                myTarget.Step(1);

            if (GUILayout.Button("Randomize", GUILayout.ExpandWidth(false)))
                myTarget.Randomize();

            EditorGUILayout.EndHorizontal();

            //Scrolling
            if (Event.current.type == EventType.ScrollWheel)
                myTarget.Step(PrefabCollectionUtility.GetScrollStep(Event.current));
        }

    }
}

