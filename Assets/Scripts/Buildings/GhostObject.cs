using UnityEngine;

namespace CastleProsperity.Building
{
    public class GhostObject : MonoBehaviour
    {
        private BoxCollider _collider = default;
        private MeshRenderer _renderer = default;
        private int _nCollisions = 0;
        public int nCollisions => _nCollisions;

        private int _terrainLayerMask = 1 << 8;

        private void Awake()
        {
            _collider = GetComponent<BoxCollider>();
            _renderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            Color baseColor = _renderer.materials[0].color;
            baseColor.a = baseColor.a / 3.0f;
            _renderer.materials[0].color = baseColor;
        }

        public void IsInValidPlacement()
        {
            _renderer.materials[0].color = Color.green;
        }

        public void IsInInvalidPlacement()
        {
            _renderer.materials[0].color = Color.red;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "Terrain")
            {
                return;
            }

            _nCollisions++;
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.tag == "Terrain")
            {
                return;
            }

            _nCollisions--;
        }

        public bool CheckFourCornerDistanceWithTerrain()
        {
            if (_nCollisions > 0) return false;

            // get 4 bottom corner positions
            Vector3 p = transform.position;
            Vector3 c = _collider.center;
            Vector3 e = _collider.size / 2f;
            float bottomHeight = c.y - e.y + 0.5f;
            Vector3[] bottomCorners = new Vector3[]
            {
        new Vector3(c.x - e.x, bottomHeight, c.z - e.z),
        new Vector3(c.x - e.x, bottomHeight, c.z + e.z),
        new Vector3(c.x + e.x, bottomHeight, c.z - e.z),
        new Vector3(c.x + e.x, bottomHeight, c.z + e.z)
            };
            // cast a small ray beneath the corner to check for a close ground
            // (if at least two are not valid, then placement is invalid)
            int invalidCornersCount = 0;
            foreach (Vector3 corner in bottomCorners)
            {
                if (!Physics.Raycast(
                    p + corner,
                    Vector3.up * -1f,
                    2f,
                    _terrainLayerMask
                ))
                    invalidCornersCount++;
            }
            return invalidCornersCount == 0;
        }
    }
}
