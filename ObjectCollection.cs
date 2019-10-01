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
    public class ObjectCollection : Collection
    {
        public List<GameObject> objects;
        public bool initialPickRandom;

        [HideInInspector] public int selectedIndex;

        public override void UpdateCollection()
        {
            Select(selectedIndex);
        }

        public override void Step(int step)
        {
            Select(selectedIndex + step);
        }

        public override void Randomize()
        {
            if (objects.Count == 0)
                return;

            int index = UnityEngine.Random.Range(0, objects.Count);
            Select(index);
        }

        public override void Select(int index)
        {
            if (objects.Count == 0)
                return;

            if (index >= objects.Count)
                index = 0;

            if (index < 0)
                index = objects.Count - 1;

            selectedIndex = index;

            //Disable
            foreach (GameObject obj in objects)
            {
                if(obj != null)
                {
                    obj.hideFlags = HideFlags.DontSaveInBuild;
                    obj.SetActive(false);
                }

            }

            //Enable
            if (objects[index] != null)
            {
                objects[index].hideFlags = HideFlags.None;
                objects[index].SetActive(true);
            }

#if UNITY_EDITOR
            if(gameObject.scene != null)
                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(gameObject.scene);

            EditorUtility.SetDirty(this);
#endif
        }
    }
}