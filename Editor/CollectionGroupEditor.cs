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

            DrawChildren(myTarget);

            EditorGUILayout.Space();

            //Children
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("All Children");

            if (GUILayout.Button("<-", GUILayout.ExpandWidth(false)))
                myTarget.Step(-1);

            if (GUILayout.Button("->", GUILayout.ExpandWidth(false)))
                myTarget.Step(1);

            if (GUILayout.Button("?", GUILayout.ExpandWidth(false)))
                myTarget.Randomize();

            EditorGUILayout.EndHorizontal();

            //Scrolling
            if (Event.current.type == EventType.ScrollWheel)
                myTarget.Step(CollectionUtility.GetScrollStep(Event.current));
        }

        public void DrawChildren(Collection collection)
        {
            if (collection.GetChildren().Count == 0)
                return;

            EditorGUI.indentLevel++;
            foreach (Collection c in collection.GetChildren())
            {
                EditorGUILayout.BeginHorizontal();

                EditorGUILayout.LabelField(c.name, EditorStyles.boldLabel);

                if (GUILayout.Button("<-", GUILayout.ExpandWidth(false)))
                    c.Step(-1);

                if (GUILayout.Button("->", GUILayout.ExpandWidth(false)))
                    c.Step(1);

                if (GUILayout.Button("?", GUILayout.ExpandWidth(false)))
                    c.Randomize();
                EditorGUILayout.EndHorizontal();

                DrawChildren(c);

            }
            EditorGUI.indentLevel--;
        }

        private void OnSceneGUI()
        {
            Event e = Event.current;

            if (e.type == EventType.ScrollWheel && e.shift)
            {
                (target as Collection)?.Step(CollectionUtility.GetScrollStep(e));
                e.Use();
            }

            if (e.isMouse && e.button == 2 && e.shift)
            {
                (target as Collection)?.Randomize();
                e.Use();
            }

            Handles.BeginGUI();
            //Add padding
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("Scroll + Shift: Step through collections");
            EditorGUILayout.LabelField("Middle Mouse + Shift: Random collections");

            Handles.EndGUI();

        }
    }

}

