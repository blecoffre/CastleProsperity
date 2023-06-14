/**
 *  Source code from Youtube channel: TheScreamingFedora. 
 *  
 *  Extra features and bug fixes by: Erick Luis de Souza.
 *  Code by : Erick Luis de Souza.
 *  
 *  email me at: erickluiss@gmail.com 
 *  for aditional information.
 * 
 */

using System;
using System.Collections;
using UnityEngine;

namespace SLE.Systems.Selection
{
    using SLE.Systems.Selection.Modules.UI;
    using SLE.Systems.Selection.Modules.MeshGeneration;
    using UnityEngine.InputSystem;
    using SLE.Systems.Selection.Collection;

    /// <summary>
    /// The <see cref="SelectionHandler"/> handles the major part of the entire <seealso cref="SelectionSystem">System</seealso>. <br/>
    /// In order to this to recoginize any selectable object, it's needed to implement <seealso cref="ISelectable"/> interface.
    /// <para>The <seealso cref="SelectionSystem">System</seealso> already brings to you a ready-to-use <seealso cref="Selector">Component</seealso> with all functionalities needed implemented. <br/>
    /// But it does not implements <seealso cref="ISelectable"/> for flexibility reason. </para>
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(MeshCollider))]
    public sealed class SelectionHandler : MonoBehaviour
    {
        [SerializeField]
        private RectangleSettings _uiRectSettings;

        private Predicate<ISelectable> _multiSelectionRule;

        private WaitForSeconds _shortDelay = new WaitForSeconds(0.1f);

        [HideInInspector]
        private MeshCollider _meshCollider;
        [HideInInspector]
        private Vector3 _cursorPosition;
        [HideInInspector]
        private Vector3 _lastClickedCursorPosition;
        [HideInInspector]
        private bool _dragSelectionPerformed;
        [HideInInspector]
        private bool _shiftIsDown;

        private PlayertControlActions _playerActions;

        /// <summary>
        /// The camera used in selection operations.
        /// </summary>
        private Camera _mainCamera = default;        

        /// <summary>
        /// Access all the current selected objects. (Read Only)
        /// </summary>
        public SelectionList<ISelectable> currentSelection { get; } = new SelectionList<ISelectable>();

        private void Awake()
        {
            _playerActions = new PlayertControlActions();

            if (!_meshCollider)
                _meshCollider = GetComponent<MeshCollider>();

            Selector.onDestroy += AutoRemove;

            Initializer.Run();
        }

        private void Start()
        {
            _mainCamera = FindObjectOfType<CameraController>().Camera;
        }

        private void OnEnable()
        {
            _playerActions.Enable();
        }

        private void OnDisable()
        {
            _playerActions.Disable();
        }

        private void OnGUI()
        {
            if (_dragSelectionPerformed)
            {
                var uiRectangle = RectangleGenerator.GetScreenRect(_lastClickedCursorPosition, _cursorPosition);

                RectangleGenerator.DrawScreenRect(uiRectangle, _uiRectSettings.rectColor);
                RectangleGenerator.DrawScreenRectBorder(uiRectangle, _uiRectSettings.borderThickness, _uiRectSettings.borderColor);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out ISelectable selectable)) return;

            switch (_multiSelectionRule)
            {
                case null:
                    currentSelection.Add(selectable);
                    break;

                default:
                    currentSelection.AddOnly(selectable, _multiSelectionRule);
                    break;
            }
        }

        private void Update()
        {
            _cursorPosition = Mouse.current.position.ReadValue();

            /* 

             Because shift key most of the times represents the switch for a multiple selection attempt 
             It's by default checked instead of a generic key (Version 1.0.0.0). 
             In the future it might be changed to save a some sort of generic key, making it possible to check for a key based on the project's setting instead.

            */

            _shiftIsDown = _playerActions.Player.Shift.IsPressed();

            ProcessSelections();
        }

        private IEnumerator DisableMeshCollider()
        {
            yield return _shortDelay;
            _meshCollider.enabled = false;
        }

        private void DeselectAll()
        {
            currentSelection.Clear();
        }

        private void HandleSingleSelection(ISelectable selectable)
        {
            if (selectable == null)
                return;

            if (_shiftIsDown)
            {
                if (_multiSelectionRule != null)
                {
                    currentSelection.AddOnly(selectable, _multiSelectionRule);
                    return;
                }
            }

            currentSelection.Add(selectable);
        }

        private void AutoRemove(ISelectable obj)
        {
            currentSelection.Remove(obj);
        }

        private void OnLeftMousePressed()
        {
            _lastClickedCursorPosition = _cursorPosition;
        }

        private void OnLeftMouseHeld()
        {
            _dragSelectionPerformed = (_lastClickedCursorPosition - _cursorPosition).sqrMagnitude > 0;
        }

        private void OnLeftMouseReleased()
        {
            if (!_shiftIsDown)
                DeselectAll();

            // Multi-selection
            if (_dragSelectionPerformed)
            {
                if (SelectionBoxGenerator.Generate(_lastClickedCursorPosition, _cursorPosition, _mainCamera, ref _meshCollider))
                    StartCoroutine(DisableMeshCollider());

                // Check the multiple selection logic in the 'OnTriggerEnter(Collider)' method.
            }
            // Single-selection
            else
            {
                Ray ray = _mainCamera.ScreenPointToRay(_cursorPosition);

                if (!Physics.Raycast(ray, out var hit, Constants.MAX_RAY_TRAVEL_DISTANCE)) return;

                if (!hit.collider.TryGetComponent(out ISelectable selectable)) return;

                HandleSingleSelection(selectable);
            }

            _dragSelectionPerformed = false;
        }

        /// <summary>
        /// Automatically handle the selection process based on mouse button press events.
        /// <para>It switches automatically between modes based on active Input System.</para>
        /// </summary>
        public void ProcessSelections()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
                OnLeftMousePressed();

            if (Mouse.current.leftButton.IsPressed())
                OnLeftMouseHeld();

            if (Mouse.current.leftButton.wasReleasedThisFrame)
                OnLeftMouseReleased();
        }

        /// <summary>
        /// Sets a new condition that will be verified when trying to select more than one object. (Either in sequence or at once) <br/>
        /// If it's <keyworkd>null</keyworkd> no condition will be verified and everything will be able to be multi selected.
        /// </summary>
        /// <param name="condition"> 
        /// The specified method that will be invoked on <see cref="ISelectable">Selectable</see> types to verify if it can be selected or not.
        /// </param>
        public void SetMultiSelectionRule(Predicate<ISelectable> condition)
        {
            _multiSelectionRule = condition;
        }
    }
}