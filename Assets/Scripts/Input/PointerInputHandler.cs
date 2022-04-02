using UnityEngine;

public class PointerInputHandler : MonoBehaviour
{
    [SerializeField]
    private Camera _camera;
    [SerializeField]
    private LayerMask layerMask;

    private InputActions inputActions;

    private IPointerInteractable lastInteractable;

    void Awake() {
        inputActions = new InputActions();
        lastInteractable = null;
    }

    void OnEnable() {
        inputActions.Player.Enable();
        inputActions.Player.PointerPosition.performed += ctx => OnPointerPositionChanged(ctx.ReadValue<Vector2>());
        inputActions.Player.PointerPress.performed += ctx => OnPointerDown();
    }

    void OnDisable() {
        inputActions.Player.Disable();
        inputActions.Player.PointerPosition.performed -= ctx => OnPointerPositionChanged(ctx.ReadValue<Vector2>());
        inputActions.Player.PointerPress.performed -= ctx => OnPointerDown();
    }

    private void OnPointerPositionChanged(Vector2 newPosition) {
        RaycastHit hit;
        if (Physics.Raycast(_camera.ScreenPointToRay(newPosition), out hit, 100f, layerMask.value)) {
            IPointerInteractable interactable = hit.collider.gameObject.GetComponent<IPointerInteractable>();

            if (interactable != null && interactable != lastInteractable) {
                lastInteractable?.OnPointerExit();
                interactable.OnPointerEnter();
                lastInteractable = interactable;
            }
        }
    }

    private void OnPointerDown() {
        lastInteractable?.OnPointerDown();
    }

}
