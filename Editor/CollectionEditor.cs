using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector.Editor;
using UnityEditor.Experimental.SceneManagement;

namespace Ardenfall
{
    [CustomEditor(typeof(Collection), true), CanEditMultipleObjects]
    public class CollectionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
                return;

            Event e = Event.current;

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("<-", GUILayout.ExpandWidth(false)))
            {
                foreach (Object t in targets)
                    (t as Collection)?.Step(-1);
            }
            if (GUILayout.Button("->", GUILayout.ExpandWidth(false)))
            {
                foreach (Object t in targets)
                    (t as Collection)?.Step(1);
            }

            if (GUILayout.Button("?", GUILayout.ExpandWidth(false)))
            {
                foreach (Object t in targets)
                    (t as Collection)?.Randomize();
            }

            EditorGUILayout.EndHorizontal();

            //Require prefab stage mode
            if (PrefabStageUtility.GetCurrentPrefabStage() == null)
                return;

            foreach (Object t in targets)
            {
                Collection target = t as Collection;

                if (target == null)
                    continue;

                if (AssetDatabase.GetAssetPath(target) != null)
                    continue;

                target.UpdateCollection();
            }

        }

        private void OnSceneGUI()
        {
            Event e = Event.current;

            //Step Object
            if (e.type == EventType.ScrollWheel && e.shift)
            {
                (target as Collection)?.Step(CollectionUtility.GetScrollStep(e));
                e.Use();
            }

            //Rotate Object
            if (e.type == EventType.ScrollWheel && e.alt)
            {
                (target as Collection)?.StepRotation(CollectionUtility.GetScrollStep(e));
                e.Use();
            }

            //Randomize Object
            if (e.isMouse && e.button == 2 && e.shift)
            {
                (target as Collection)?.Randomize();
                e.Use();
            }

            Handles.BeginGUI();
            //Add padding
            EditorGUILayout.LabelField("");
            EditorGUILayout.LabelField("Scroll + Shift: Step through objects");
            EditorGUILayout.LabelField("Scroll + Alt: Rotate Object");
            EditorGUILayout.LabelField("Middle Mouse + Shift: Random objects");

            Handles.EndGUI();

        }

    }
}

