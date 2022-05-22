using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SfxPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] wallBuildClips;
    [SerializeField]
    private AudioClip[] helicopterClips;
    [SerializeField]
    private AudioClip[] evacuationStartClips;
    [SerializeField]
    private AudioClip[] evacuationEndClips;
    [SerializeField]
    private AudioClip[] buildingDestroyedClips;
    [SerializeField]
    private AudioClip[] turnStartClips;
    [SerializeField]
    private AudioClip[] turnEndClips;
    [SerializeField]
    private AudioClip[] spreadClips;
    [SerializeField]
    private AudioClip[] cancelActionClips;
    [SerializeField]
    private AudioClip[] invalidActionClips;
    [SerializeField]
    private AudioClip[] victoryClips;
    [SerializeField]
    private AudioClip[] failureClips;

    private AudioSource player;

    void Awake() {
        player = GetComponent<AudioSource>();
    }

    void OnEnable() {
        Tile.OnWallBuilt.AddListener(() => PlayRandomSfx(wallBuildClips));
        Tile.OnDelayedHouseBuilt.AddListener(() => PlayRandomSfx(evacuationStartClips));
        Tile.OnDelayedFactoryBuilt.AddListener(() => PlayRandomSfx(evacuationStartClips));
        Tile.OnHouseBuilt.AddListener(() => PlayRandomSfx(evacuationEndClips));
        Tile.OnFactoryBuilt.AddListener(() => PlayRandomSfx(evacuationEndClips));
        Tile.OnFactoryDestroyed.AddListener(() => PlayRandomSfx(buildingDestroyedClips));
        Tile.OnHouseDestroyed.AddListener(() => PlayRandomSfx(buildingDestroyedClips));
        Helipad.OnHelicopterActivated.AddListener(() => PlayRandomSfx(helicopterClips));
        Helipad.OnInvalidActivation.AddListener(() => PlayRandomSfx(invalidActionClips));
        WallResourceBuilding.OnInvalidActivation.AddListener(() => PlayRandomSfx(invalidActionClips));
        WallResourceBuilding.OnEvacuationCancelled.AddListener(() => PlayRandomSfx(cancelActionClips));
        CorruptionHandler.OnSpread.AddListener(() => PlayRandomSfx(spreadClips));
        TurnManager.OnCorruptionTurnEnded.AddListener(() => PlayRandomSfx(turnStartClips));
        TurnManager.OnPlayerTurnEnded.AddListener(() => PlayRandomSfx(turnEndClips));
        TurnManager.OnGameWon.AddListener(() => PlayRandomSfx(victoryClips));
        TurnManager.OnGameLost.AddListener(() => PlayRandomSfx(failureClips));
    }

    void OnDisable() {
        Tile.OnWallBuilt.RemoveListener(() => PlayRandomSfx(wallBuildClips));
        Tile.OnDelayedHouseBuilt.RemoveListener(() => PlayRandomSfx(evacuationStartClips));
        Tile.OnDelayedFactoryBuilt.RemoveListener(() => PlayRandomSfx(evacuationStartClips));
        Tile.OnHouseBuilt.RemoveListener(() => PlayRandomSfx(evacuationEndClips));
        Tile.OnFactoryBuilt.RemoveListener(() => PlayRandomSfx(evacuationEndClips));
        Tile.OnFactoryDestroyed.RemoveListener(() => PlayRandomSfx(buildingDestroyedClips));
        Tile.OnHouseDestroyed.RemoveListener(() => PlayRandomSfx(buildingDestroyedClips));
        Helipad.OnHelicopterActivated.RemoveListener(() => PlayRandomSfx(helicopterClips));
        Helipad.OnInvalidActivation.RemoveListener(() => PlayRandomSfx(invalidActionClips));
        WallResourceBuilding.OnInvalidActivation.RemoveListener(() => PlayRandomSfx(invalidActionClips));
        WallResourceBuilding.OnEvacuationCancelled.AddListener(() => PlayRandomSfx(cancelActionClips));
        CorruptionHandler.OnSpread.RemoveListener(() => PlayRandomSfx(spreadClips));
        TurnManager.OnCorruptionTurnEnded.RemoveListener(() => PlayRandomSfx(turnStartClips));
        TurnManager.OnPlayerTurnEnded.RemoveListener(() => PlayRandomSfx(turnEndClips));
        TurnManager.OnGameWon.RemoveListener(() => PlayRandomSfx(victoryClips));
        TurnManager.OnGameLost.RemoveListener(() => PlayRandomSfx(failureClips));
    }

    private void PlayRandomSfx(AudioClip[] clips) {
        if (clips.Length == 0) {
            return;
        }
        player.PlayOneShot(clips[Random.Range(0, clips.Length)]);
    }

}
