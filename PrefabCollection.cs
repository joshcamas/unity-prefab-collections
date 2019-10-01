using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
#endif

namespace Ardenfall
{
    [ExecuteInEditMode, SelectionBase]
    public class PrefabCollection : Collection
    {
        public List<GameObject> prefabs;

        public bool initialPickRandom;


        public bool autoSelection = true;

        [Header("Rotation")]
        public Vector3 rotationOrigin;
        public float rotationAmount = 90;

        [Header("Debug Values")]
        public int rotatedIndex;
        public int selectedIndex;

        [HideInInspector] public bool isPrefab = false;
        [HideInInspector] public GameObject spawnedObject;

        private int PrefabCount
        {
            get
            {
                return prefabs.Count;
            }
        }

#if UNITY_EDITOR

        private GameObject GetPrefabGameObject(int index)
        {
            return prefabs[index];
        }

        private GameObject CreatePrefabGameObject(int index)
        {
            GameObject obj = GetPrefabGameObject(index);
            GameObject spawnedGameObject = null;

            if (obj == null)
                spawnedGameObject = new GameObject("empty");
            else
                spawnedGameObject = (GameObject)PrefabUtility.InstantiatePrefab(obj);

            spawnedGameObject.transform.parent = transform;
            spawnedGameObject.transform.localScale = Vector3.one;
            spawnedGameObject.transform.localPosition = Vector3.zero;

            spawnedGameObject.transform.localRotation = Quaternion.Euler(Vector3.zero);
            spawnedGameObject.transform.RotateAround(transform.position + rotationOrigin, Vector3.up, rotatedIndex * rotationAmount);

            return spawnedGameObject;
        }

        public override void Randomize()
        {
            if (PrefabCount == 0)
                return;

            int index = UnityEngine.Random.Range(0, PrefabCount);
            Select(index);
        }

        public override void StepRotation(int step)
        {
            Rotate(step);
        }

        private void Rotate(int step)
        {
            if (spawnedObject == null)
                return;

            rotatedIndex += step;

            spawnedObject.transform.RotateAround(transform.position + rotationOrigin, Vector3.up, step * rotationAmount);

            EditorUtility.SetDirty(this);
        }

        public override void Step(int step)
        {
            Select(selectedIndex + step);
        }

        public override void Select(int index)
        {
            if (index >= PrefabCount)
                index = 0;

            if (index < 0)
                index = PrefabCount - 1;

            selectedIndex = index;

            ClearChildrenPrefabs();

            spawnedObject = CreatePrefabGameObject(index);

            if (PrefabStageUtility.GetCurrentPrefabStage() != null)
            {
                spawnedObject.hideFlags = HideFlags.DontSave;// | HideFlags.HideInHierarchy;

                if(autoSelection)
                    spawnedObject.name = "[SelectParent] " + index;
                else
                    spawnedObject.name = "[Prefab] " + index;

                isPrefab = true;
            }
            else
            {
                //target.spawnedObject.hideFlags = HideFlags.HideInInspector;
                isPrefab = false;

                if (autoSelection)
                    spawnedObject.name = "[SelectParent] " + index + "^";
                else
                    spawnedObject.name = "[Prefab] " + index;
            }

            EditorUtility.SetDirty(this);
        }

        public override void UpdateCollection()
        {
            if (Application.isPlaying)
                return;

            //Reset index if prefab mode
            if (!CollectionUtility.IsPrefabMode() && isPrefab)
            {
                isPrefab = false;
                selectedIndex = -1;

                ClearChildrenPrefabs();
            }

            //Auto Selection
            if (spawnedObject == null && prefabs != null && PrefabCount > 0)
            {
                if (selectedIndex == -1)
                    SelectInitial();
                else
                    Select(selectedIndex);
            }

            //Detect no prefabs
            if (spawnedObject == null && (prefabs == null || prefabs.Count == 0))
            {
                ClearChildrenPrefabs();
                spawnedObject = null;
                selectedIndex = -1;
                EditorUtility.SetDirty(this);
            }

            //Out of range
            if (selectedIndex != -1 && selectedIndex >= PrefabCount)
                Select(PrefabCount - 1);

            else
            {
                //Detect change in current prefab
                bool isSamePrefab = true;

#if UNITY_EDITOR
                if (spawnedObject != null)
                    isSamePrefab = PrefabUtility.GetCorrespondingObjectFromSource(spawnedObject) == prefabs[selectedIndex];
#endif

                if (selectedIndex != -1 && !isSamePrefab)
                    Select(selectedIndex);
            }

        }

        public void SelectInitial()
        {
            if (prefabs.Count == 0)
                return;

            int index = 0;

            if (initialPickRandom)
                index = UnityEngine.Random.Range(0, prefabs.Count);

            Select(index);
        }

        private void ClearChildrenPrefabs()
        {
            if (spawnedObject != null)
                GameObject.DestroyImmediate(spawnedObject);

            for(int i=0;i<transform.childCount;i++)
            {
                if (transform.GetChild(i) == spawnedObject)
                    continue;

                string name = transform.GetChild(i).name;

                if (name.StartsWith("[SelectParent]") || name.StartsWith("[Prefab]"))
                {
                    GameObject.DestroyImmediate(transform.GetChild(i).gameObject);
                }

            }
        }

#endif

    }

}