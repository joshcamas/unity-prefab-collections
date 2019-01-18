using UnityEngine;
using UnityEditor;

namespace Ardenfall
{
    [CustomEditor(typeof(PrefabCollection), true)]
    public class PrefabCollectionEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (Application.isPlaying)
                return;

            Event e = Event.current;
            PrefabCollection myTarget = (PrefabCollection)target;

            PrefabCollectionUtility.UpdatePrefabCollection(myTarget);

            if (myTarget.prefabs == null || myTarget.prefabs.Count == 0)
            {
                EditorGUILayout.LabelField("Please add some prefab collections", EditorStyles.boldLabel);
                return;
            }

            if (e.type == EventType.ScrollWheel)
                myTarget.Step(PrefabCollectionUtility.GetScrollStep(e));
        }

        /* Scrolling is weird in scene gui, since it's used for zooming
        
        private void OnSceneGUI()
        {
            Event e = Event.current;
            PrefabCollection myTarget = (PrefabCollection)target;

            if (myTarget.prefabs.Count == 0)
                return;

            if (e.type == EventType.ScrollWheel)
                myTarget.Step(PrefabCollectionUtility.GetScrollStep(e));
        }*/

    }
}

