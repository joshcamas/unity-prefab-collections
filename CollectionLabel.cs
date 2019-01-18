using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Ardenfall
{
    public class CollectionLabel : MonoBehaviour
    {
        public string label = "Label";
        public Color labelColor = Color.white;
        public Vector3 offset;

#if UNITY_EDITOR
        public void OnDrawGizmosSelected()
        {
            Color oldColor = Handles.color;
            Handles.color = labelColor;

            Handles.Label(transform.position + offset, label);

            Handles.color = oldColor;
        }
#endif
    }
}
