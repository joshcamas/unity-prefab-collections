using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;


namespace Ardenfall
{
    public class CollectionGroup : Collection
    {

        //Step children
        public override void Step(int step)
        {
            foreach (Collection c in GetChildren())
            {
                c.Step(step);
            }
        }

        //Randomize children
        public override void Randomize()
        {
            
            foreach(Collection c in GetChildren())
            {
                c.Randomize();
            }
        }
    }
}
