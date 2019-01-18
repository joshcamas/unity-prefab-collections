using System.Collections.Generic;
using UnityEngine;

namespace Ardenfall
{
    public class Collection : MonoBehaviour
    {
        //Randomize Collection
        public virtual void Randomize() { }

        //Step Collection by step amount - negative means backwards
        public virtual void Step(int step) { }

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
    }
}
