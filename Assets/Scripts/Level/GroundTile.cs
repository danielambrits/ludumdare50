using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
public class GroundTile : MonoBehaviour, IPointerInteractable
{
    private MeshRenderer meshRenderer;
    private Color defaultColor;

    public bool isCorrupted;
    public bool hasWall;

    public static UnityEvent OnWallBuild = new UnityEvent();

    void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
        hasWall = false;
        isCorrupted = false;
    }

    public void OnPointerEnter() {
        meshRenderer.material.color = Color.green;
    }

    public void OnPointerExit() {
        meshRenderer.material.color = defaultColor;
    }

    public void OnPointerDown() {
        BuildWall();
    }

    public void Corrupt() {
        if (isCorrupted) {
            return;
        }
        isCorrupted = true;
        defaultColor = Color.yellow;
        meshRenderer.material.color = defaultColor;
    }

    private void BuildWall() {
        hasWall = true;
        defaultColor = Color.gray;
        meshRenderer.material.color = defaultColor;
        OnWallBuild.Invoke();
    }

}
