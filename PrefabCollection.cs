using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ardenfall
{
    [ExecuteInEditMode]
    public class PrefabCollection : Collection
    {
        public List<GameObject> prefabs;
        public bool initialPickRandom;
        
        [HideInInspector] public int selectedIndex;
        [HideInInspector] public bool isPrefab = false;
        [HideInInspector] public GameObject spawnedObject;

#if UNITY_EDITOR
        public void Awake()
        {
            PrefabCollectionUtility.UpdatePrefabCollection(this);
        }

        public override void Randomize()
        {
            PrefabCollectionUtility.SelectRandom(this);
        }

        public override void Step(int step)
        {
            PrefabCollectionUtility.Select(selectedIndex + step,this);
        }

#endif


    }

}
