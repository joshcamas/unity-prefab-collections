#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using System.Linq;
using System.Collections.Generic;
using System.IO;

namespace Ardenfall
{
    public class CollectionUtility
    {
        [MenuItem("Assets/Asset Tools/Create Collection")]
        public static void CreateCollection()
        {
            GameObject root = Selection.gameObjects[0];

            //Default name: root name
            string collectionName = root.name + "_collection";
            string saveFolder = Path.GetDirectoryName(AssetDatabase.GetAssetPath(root.GetInstanceID()));

            //Try to find common name between all selection objects
            List<string> names = new List<string>(Selection.gameObjects.Select(n => n.name));

            //Aggregate substring
            List<string> result = new List<string>(names.Select(s => s.Split())
            .Aggregate(
                 names[0].Split().AsEnumerable(), // init accum with words from first string
                 (a, words) => a.Intersect(words),       // intersect with next set of words
                 a => a));

            if (result.Count > 0)
                collectionName = result[0] + "_collection";

            //Create collection
            GameObject collectionObject = new GameObject();
            collectionObject.name = collectionName;
            PrefabCollection c = collectionObject.AddComponent<PrefabCollection>();
            c.prefabs = new List<GameObject>(Selection.gameObjects);
            c.isPrefab = true;

            //Save collection
            GameObject prefab = PrefabUtility.SaveAsPrefabAsset(collectionObject, saveFolder + "/" + collectionName + ".prefab");
            Selection.activeObject = prefab;

            GameObject.DestroyImmediate(collectionObject);
        }

        [MenuItem("Assets/Asset Tools/Create Collection", validate =true)]
        public static bool CreateCollectionValidate()
        {
            return (Selection.gameObjects.Length > 0);

        }

        public static bool IsPrefabMode()
        {
            return PrefabStageUtility.GetCurrentPrefabStage() != null;
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


    }

}

#endif