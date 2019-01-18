#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;

namespace Ardenfall
{
    public class PrefabCollectionUtility
    {
        public static void UpdatePrefabCollection(PrefabCollection collection)
        {
            if (Application.isPlaying)
                return;

            //Reset index if prefab mode
            if (!PrefabCollectionUtility.IsPrefabMode() && collection.isPrefab)
            {
                collection.isPrefab = false;
                collection.selectedIndex = -1;

                if (collection.spawnedObject != null)
                    GameObject.DestroyImmediate(collection.spawnedObject);
            }
            
            //Auto Selection
            if (collection.spawnedObject == null && collection.prefabs != null && collection.prefabs.Count > 0)
            {
                if (collection.selectedIndex == -1)
                    PrefabCollectionUtility.SelectInitial(collection);
                else
                    PrefabCollectionUtility.Select(collection.selectedIndex, collection);
            }

            //Detect no prefabs
            if (collection.spawnedObject == null && (collection.prefabs == null || collection.prefabs.Count == 0))
            {
                GameObject.DestroyImmediate(collection.spawnedObject);
                collection.spawnedObject = null;
                collection.selectedIndex = -1;
                EditorUtility.SetDirty(collection);
            }

            bool isSamePrefab = true;

            if(collection.spawnedObject != null)
                isSamePrefab = PrefabUtility.GetCorrespondingObjectFromSource(collection.spawnedObject) == collection.prefabs[collection.selectedIndex];

            //Out of range
            if (collection.selectedIndex != -1 && collection.selectedIndex >= collection.prefabs.Count)
                PrefabCollectionUtility.Select(collection.prefabs.Count - 1, collection);

            //Detect change in current prefab
            else if (collection.selectedIndex != -1 && !isSamePrefab)
                PrefabCollectionUtility.Select(collection.selectedIndex, collection);


        }
        public static bool IsPrefabMode()
        {
            return PrefabStageUtility.GetCurrentPrefabStage() != null;
        }

        public static void SelectInitial(PrefabCollection target)
        {
            if (target.prefabs.Count == 0)
                return;

            int index = 0;

            if(target.initialPickRandom)
                index = UnityEngine.Random.Range(0, target.prefabs.Count);

            Select(index, target);
        }

        public static void SelectRandom(PrefabCollection target)
        {

            if (target.prefabs.Count == 0)
                return;

            int index = UnityEngine.Random.Range(0, target.prefabs.Count);

            Select(index, target);
        }

        public static int GetScrollStep(Event e)
        {
            int index = 0;

            if (e.delta.y > 0)
                index += 1;

            if (e.delta.y < 0)
                index -= 1;

            return index;

        }

        public static void Select(int index, PrefabCollection target)
        {
            if (index >= target.prefabs.Count)
                index = 0;

            if (index < 0)
                index = target.prefabs.Count - 1;

            target.selectedIndex = index;

            if(target.spawnedObject != null)
                GameObject.DestroyImmediate(target.spawnedObject);

            //Detect empty
            if (target.prefabs[index] != null)
                target.spawnedObject = (GameObject)PrefabUtility.InstantiatePrefab(target.prefabs[index]);
            else
                target.spawnedObject = new GameObject("empty");
            target.spawnedObject.transform.parent = target.transform;
            target.spawnedObject.transform.localPosition = Vector3.zero;
            target.spawnedObject.transform.localRotation = Quaternion.Euler(Vector3.zero);

            target.hideFlags = HideFlags.None;


            if (IsPrefabMode())
            {
                target.spawnedObject.hideFlags = HideFlags.DontSave | HideFlags.HideInHierarchy;
                target.isPrefab = true;
            } else
            {
                target.spawnedObject.hideFlags = HideFlags.HideInHierarchy;
                target.isPrefab = false;
            }
                

            EditorUtility.SetDirty(target);
        }

    }

}

#endif