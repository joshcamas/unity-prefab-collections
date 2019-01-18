using UnityEngine;

namespace Ardenfall
{
    [ExecuteInEditMode]
    public class GriddedObject : MonoBehaviour
    {
        public Vector3 gridSize = new Vector3(30,30,30);
        public bool enableX = true;
        public bool enableY = false;
        public bool enableZ = true;

        public Vector3 offset;
        public bool enableOnlyInGriddedContainer = false;
        public int gridVisualSize = 2;
        public Color gridColor = new Color(0.1f, 0.1f, 0.1f);

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.isPlaying)
                return;

            if (!EnableGrid())
                return;

            Vector3 position = transform.localPosition;

            if (enableX)
                position.x = offset.x + Mathf.Round(position.x / gridSize.x) * gridSize.x;

            if (enableY)
                position.y = offset.y + Mathf.Round(position.y / gridSize.y) * gridSize.y;

            if (enableZ)
                position.z = offset.z + Mathf.Round(position.z / gridSize.z) * gridSize.z;

            transform.localPosition = position;
        }

        private bool EnableGrid()
        {
            //Gridded container check
            if (enableOnlyInGriddedContainer)
                if (!GetComponentInParent<GriddedContainer>())
                    return false;
            return true;
        }

        private void OnDrawGizmosSelected()
        {
            if (!EnableGrid())
                return;

            Color oldColor = Gizmos.color;
            Gizmos.color = gridColor;

            if (enableX)
            {
                for(int i=0;i< gridVisualSize;i++)
                {
                    float v = 0.5f + i;
                    float b = gridVisualSize;

                    Gizmos.DrawLine(transform.position + new Vector3(gridSize.x*v, 0, -gridSize.y * b), transform.position + new Vector3(gridSize.x*v, 0, gridSize.y * b));
                    Gizmos.DrawLine(transform.position + new Vector3(-gridSize.x*v, 0, -gridSize.y * b), transform.position + new Vector3(-gridSize.x*v, 0, gridSize.y * b));
                }
            }

            if (enableZ)
            {
                for (int i = 0; i < gridVisualSize; i++)
                {
                    float v = 0.5f + i;
                    float b = gridVisualSize;

                    Gizmos.DrawLine(transform.position + new Vector3(-gridSize.x * b, 0, gridSize.y*v), transform.position + new Vector3(gridSize.x * b, 0, gridSize.y*v));
                    Gizmos.DrawLine(transform.position + new Vector3(-gridSize.x * b, 0, -gridSize.y*v), transform.position + new Vector3(gridSize.x * b, 0, -gridSize.y*v));
                }
                
            }

            Gizmos.color = oldColor;
        }

#endif
    }
}
