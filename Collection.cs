using System.Collections.Generic;
using UnityEngine;

namespace Ardenfall
{
    [ExecuteInEditMode]
    public class Collection : MonoBehaviour
    {
        //Randomize Collection
        public virtual void Randomize() { }

        public virtual void StepRotation(int step) { }

        //Step Collection by step amount - negative means backwards
        public virtual void Step(int step) { }

        public virtual void Select(int index) { }

        //Run when prefab is dropped into a scene
        public virtual void OnAddToScene() { }

        public virtual void UpdateCollection() { }

        //Returns all direct Collection children
        public List<Collection> GetChildren()
        {
            List<Collection> children = new List<Collection>();

            for (int i =0;i<transform.childCount;i++)
            {
                Collection c = transform.GetChild(i).gameObject.GetComponent<Collection>();
                if (c != null)
                    children.Add(c);
            }

            return children;
        }

#if UNITY_EDITOR
        private void Awake()
        {
            if(!Application.isPlaying)
                UpdateCollection();
        }

#endif
    }
}