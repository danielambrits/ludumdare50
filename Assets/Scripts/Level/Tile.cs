using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(MeshRenderer))]
public class Tile : MonoBehaviour, IPointerInteractable
{
    public enum Type {
        Default,
        Wall,
        House,
        Factory,
        Helipad,
        HouseUnderConstruction,
        FactoryUnderConstruction,
    }

    public Type type;
    public bool isCorrupted;

    [SerializeField]
    private Color selectionColor;
    [SerializeField]
    private Color dangerZoneColor;

    [Header("Building meshes")]
    [SerializeField]
    private GameObject wallPrefab;
    [SerializeField]
    private GameObject helipadPrefab;
    [SerializeField]
    private GameObject housePrefab;
    [SerializeField]
    private GameObject factoryPrefab;

    [Header("Building placeholders")]
    [SerializeField]
    private GameObject housePlaceholder;
    [SerializeField]
    private GameObject factoryPlaceholder;

    private MeshRenderer meshRenderer;
    private Color defaultColor;

    private GameObject building;

    private OvertextHandler uiHandler;

    private int cooldown;
    
    static private Tile tileToEvacuate = null;
    static private bool evacuationAvailable = true;


    public static UnityEvent OnWallBuilt = new UnityEvent();
    public static UnityEvent OnHouseBuilt = new UnityEvent();
    public static UnityEvent OnHouseDestroyed = new UnityEvent();
    public static UnityEvent OnDelayedHouseBuilt = new UnityEvent();
    public static UnityEvent OnFactoryBuilt = new UnityEvent();
    public static UnityEvent OnFactoryDestroyed = new UnityEvent();
    public static UnityEvent OnDelayedFactoryBuilt = new UnityEvent();

    void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        defaultColor = meshRenderer.material.color;
        type = Type.Default;
        isCorrupted = false;
        uiHandler = null;
        building = null;

        tileToEvacuate = null;
        evacuationAvailable = true;
    }

    void OnEnable() {
        WallResourceBuilding.OnEvacuationCancelled.AddListener(HidePlaceholderBuildings);
    }

    void OnDisable() {
        WallResourceBuilding.OnEvacuationCancelled.RemoveListener(HidePlaceholderBuildings);
    }

    public void OnPointerEnter() {
        meshRenderer.material.SetInt("_Selected", 1);
        if (tileToEvacuate != null) {
            if (type == Type.Default && !isCorrupted) {
                Color materialColor;
                switch (tileToEvacuate.type) {
                    case Type.House:
                        housePlaceholder.SetActive(true);
                        // TODO why do I need to do this???
                        materialColor = housePlaceholder.GetComponent<MeshRenderer>().material.color;
                        materialColor.a = 0.5f;
                        housePlaceholder.GetComponent<MeshRenderer>().material.color = materialColor;
                        break;
                    case Type.Factory:
                        factoryPlaceholder.SetActive(true);
                        // TODO why do I need to do this???
                        materialColor = factoryPlaceholder.GetComponent<MeshRenderer>().material.color;
                        materialColor.a = 0.5f;
                        factoryPlaceholder.GetComponent<MeshRenderer>().material.color = materialColor;
                        break;
                    default:
                        // NOP
                        break;
                }
            }
        }
    }

    public void OnPointerExit() {
        meshRenderer.material.SetInt("_Selected", 0);
        if (tileToEvacuate != null) {
            HidePlaceholderBuildings();
        }
    }

    public void OnPointerDown() {
        if (tileToEvacuate != null) {
            if (type == Type.Default && !isCorrupted) {
                switch (tileToEvacuate.type) {
                    case Type.House:
                        building = tileToEvacuate.building;
                        building.GetComponent<WallResourceBuilding>().Relocate(this);
                        tileToEvacuate.Evacuate();
                        BuildDelayedHouse();
                        break;
                    case Type.Factory:
                        building = tileToEvacuate.building;
                        building.GetComponent<WallResourceBuilding>().Relocate(this);
                        tileToEvacuate.Evacuate();
                        BuildDelayedFactory();
                        break;
                    default:
                        // NOP
                        break;
                }
                tileToEvacuate = null;
            }
        } else {
            switch (type) {
                case Type.Default:
                    if (!isCorrupted) {
                        BuildWall();
                    }
                    break;
                case Type.House:
                    if (evacuationAvailable) {
                        tileToEvacuate = this;
                    }
                    break;
                case Type.Factory:
                    if (evacuationAvailable) {
                        tileToEvacuate = this;
                    }
                    break;
                default:
                    // NOP
                    break;
            }
        }
    }

    public void Corrupt() {
        if (isCorrupted) {
            return;
        }
        isCorrupted = true;
        if (building != null) {
            building.SetActive(false);
        }
        switch (type) {
            case Type.Wall:
                return;
            case Type.House:
                OnHouseDestroyed.Invoke();
                break;
            case Type.Factory:
                OnFactoryDestroyed.Invoke();
                break;
        }
        defaultColor = Color.black;
        meshRenderer.material.color = defaultColor;
        meshRenderer.material.SetInt("_DiagonalOn", 0);
        if (uiHandler) {
            uiHandler.SetValue(0);
        }
    }

    public void Evacuate() {
        switch (type) {
            case Type.House:
                type = Type.Default;
                building = null;
                OnHouseDestroyed.Invoke();
                break;
            case Type.Factory:
                type = Type.Default;
                building = null;
                OnFactoryDestroyed.Invoke();
                break;
            default:
                // NOP
                break;
        }
    }

    public void TagAsDangerZone() {
        meshRenderer.material.SetInt("_DiagonalOn", 1);
    }

    private void BuildWall() {
        type = Type.Wall;
        GameObject wallObject = Instantiate(wallPrefab, transform.position, Quaternion.identity);
        wallObject.transform.SetParent(transform);
        OnWallBuilt.Invoke();
    }

    public void BuildHouse() {
        type = Type.House;
        building = Instantiate(housePrefab, transform.position, Quaternion.identity);
        building.transform.SetParent(transform);
        building.GetComponent<WallResourceBuilding>().SetBaseTile(this);
        housePlaceholder.SetActive(false);
        OnHouseBuilt.Invoke();
    }

    private void BuildDelayedHouse() {
        type = Type.HouseUnderConstruction;
        evacuationAvailable = false;
        OnDelayedHouseBuilt.Invoke();
    }

    public void BuildFactory() {
        type = Type.Factory;
        building = Instantiate(factoryPrefab, transform.position, Quaternion.identity);
        building.transform.SetParent(transform);
        building.GetComponent<WallResourceBuilding>().SetBaseTile(this);
        factoryPlaceholder.SetActive(false);
        OnFactoryBuilt.Invoke();
    }

    private void BuildDelayedFactory() {
        type = Type.FactoryUnderConstruction;
        evacuationAvailable = false;
        OnDelayedFactoryBuilt.Invoke();
    }

    public void BuildHelipad() {
        type = Type.Helipad;
        building = Instantiate(helipadPrefab, transform.position, Quaternion.identity);
        building.transform.SetParent(transform);
    }

    public bool NotifyToEvacuate() {
        if (evacuationAvailable) {
            tileToEvacuate = this;
            return true;
        }
        return false;
    }

    public void NotifyEvacuationCancel() {
        if (tileToEvacuate) {
            tileToEvacuate = null;
            evacuationAvailable = true;
        }
    }

    public void NotifySuccesfulEvacuation() {
        evacuationAvailable = true;
        switch (type) {
            case Type.HouseUnderConstruction:
                type = Type.House;
                housePlaceholder.SetActive(false);
                OnHouseBuilt.Invoke();
                break;
            case Type.FactoryUnderConstruction:
                type = Type.Factory;
                factoryPlaceholder.SetActive(false);
                OnFactoryBuilt.Invoke();
                break;
            default:
                // NOP
                break;
        }
    }

    private void HidePlaceholderBuildings() {
        housePlaceholder.SetActive(false);
        factoryPlaceholder.SetActive(false);
    }

}
