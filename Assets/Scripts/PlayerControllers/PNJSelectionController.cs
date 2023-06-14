using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PNJSelectionController : MonoBehaviour
{
    private int _pnjLayer = 1 << 7;
    private Camera _mainCamera = default;
    private Vector3 _startPosition;
    private PlayertControlActions _playerActions;
    private List<ISelectable> _selectedPNJ = new List<ISelectable>();

    #region Box
    RaycastHit hit;

    bool dragSelect;

    //Collider variables
    //=======================================================//

    MeshCollider selectionBox;
    Mesh selectionMesh;

    Vector3 p1;
    Vector3 p2;

    //the corners of our 2d selection box
    Vector2[] corners;

    //the vertices of our meshcollider
    Vector3[] verts;
    Vector3[] vecs;
    #endregion Box

    private void Awake()
    {
        _playerActions = new PlayertControlActions();
    }

    private void Start()
    {
        _mainCamera = FindObjectOfType<CameraController>().Camera;
        dragSelect = false;
    }

    private void OnEnable()
    {
        _playerActions.Enable();
    }

    private void OnDisable()
    {
        _playerActions.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        GetMousePositions();
    }

    private void GetMousePositions()
    {
        //Click
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            p1 = Mouse.current.position.ReadValue();
        }

        //Release
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            if (dragSelect == false) //single select
            {
                Ray ray = _mainCamera.ScreenPointToRay(p1);

                if (Physics.Raycast(ray, out hit, 5000.0f, _pnjLayer))
                {
                    ISelectable pnj;
                    hit.transform.TryGetComponent<ISelectable>(out pnj);

                    if (pnj == null)
                    {
                        return;
                    }

                    if (_playerActions.Player.Shift.WasPerformedThisFrame()) //inclusive select
                    {
                        AddSelected(pnj);
                    }
                    else //exclusive selected
                    {
                        ClearSelected();
                        AddSelected(pnj);
                    }
                }
                else //if we didnt hit something
                {
                    if (_playerActions.Player.Shift.WasPerformedThisFrame())
                    {
                        //do nothing
                    }
                    else
                    {
                        ClearSelected();
                    }
                }
            }
            else //marquee select
            {
                verts = new Vector3[4];
                vecs = new Vector3[4];
                int i = 0;
                p2 = Input.mousePosition;
                corners = getBoundingBox(p1, p2);

                foreach (Vector2 corner in corners)
                {
                    Ray ray = Camera.main.ScreenPointToRay(corner);

                    if (Physics.Raycast(ray, out hit, 50000.0f, (1 << 8)))
                    {
                        verts[i] = new Vector3(hit.point.x, hit.point.y, hit.point.z);
                        vecs[i] = ray.origin - hit.point;
                        Debug.DrawLine(Camera.main.ScreenToWorldPoint(corner), hit.point, Color.red, 1.0f);
                    }
                    i++;
                }

                //generate the mesh
                selectionMesh = generateSelectionMesh(verts, vecs);

                selectionBox = gameObject.AddComponent<MeshCollider>();
                selectionBox.sharedMesh = selectionMesh;
                selectionBox.convex = true;
                selectionBox.isTrigger = true;

                if (!_playerActions.Player.Shift.WasPerformedThisFrame())
                {
                    ClearSelected();
                }

                Destroy(selectionBox, 0.02f);

            }//end marquee select

            dragSelect = false;
        }

        //Held
        if(Mouse.current.leftButton.IsPressed())
        {
            if ((p1 - Input.mousePosition).magnitude > 40)
            {
                dragSelect = true;
            }
        }
    }

    private void OnGUI()
    {
        if (dragSelect == true)
        {
            var rect = Utils.GetScreenRect(p1, Input.mousePosition);
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }

    //create a bounding box (4 corners in order) from the start and end mouse position
    Vector2[] getBoundingBox(Vector2 p1, Vector2 p2)
    {
        // Min and Max to get 2 corners of rectangle regardless of drag direction.
        var bottomLeft = Vector3.Min(p1, p2);
        var topRight = Vector3.Max(p1, p2);

        // 0 = top left; 1 = top right; 2 = bottom left; 3 = bottom right;
        Vector2[] corners =
        {
            new Vector2(bottomLeft.x, topRight.y),
            new Vector2(topRight.x, topRight.y),
            new Vector2(bottomLeft.x, bottomLeft.y),
            new Vector2(topRight.x, bottomLeft.y)
        };
        return corners;

    }

    //generate a mesh from the 4 bottom points
    Mesh generateSelectionMesh(Vector3[] corners, Vector3[] vecs)
    {
        Vector3[] verts = new Vector3[8];
        int[] tris = { 0, 1, 2, 2, 1, 3, 4, 6, 0, 0, 6, 2, 6, 7, 2, 2, 7, 3, 7, 5, 3, 3, 5, 1, 5, 0, 1, 1, 4, 0, 4, 5, 6, 6, 5, 7 }; //map the tris of our cube

        for (int i = 0; i < 4; i++)
        {
            verts[i] = corners[i];
        }

        for (int j = 4; j < 8; j++)
        {
            verts[j] = corners[j - 4] + vecs[j - 4];
        }

        Mesh selectionMesh = new Mesh();
        selectionMesh.vertices = verts;
        selectionMesh.triangles = tris;

        return selectionMesh;
    }

    private void OnTriggerEnter(Collider other)
    {
        ISelectable pnj;
        if (other.TryGetComponent<ISelectable>(out pnj))
        {
            AddSelected(pnj);
        }
    }

    private void AddSelected(ISelectable selected)
    {
        selected.Select();
        _selectedPNJ.Add(selected);
    }

    private void RemoveSelected(ISelectable selected)
    {
        selected.Deselect();
        _selectedPNJ.Remove(selected);
    }

    private void ClearSelected()
    {
        for (int i = 0; i < _selectedPNJ.Count; i++)
        {
            _selectedPNJ[i].Deselect();
        }

        _selectedPNJ.Clear();
    }
}
