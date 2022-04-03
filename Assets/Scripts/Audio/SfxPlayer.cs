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
    private AudioClip[] turnStartClips;
    [SerializeField]
    private AudioClip[] turnEndClips;
    [SerializeField]
    private AudioClip[] spreadClips;
    [SerializeField]
    private AudioClip[] victoryClips;
    [SerializeField]
    private AudioClip[] failureClips;

    private AudioSource player;

    void Awake() {
        player = GetComponent<AudioSource>();
    }

    void OnEnable() {
        GroundTile.OnWallBuilt.AddListener(() => PlayRandomSfx(wallBuildClips));
        GroundTile.OnHelicopterActivated.AddListener(() => PlayRandomSfx(helicopterClips));
        GroundTile.OnDelayedHouseBuilt.AddListener(() => PlayRandomSfx(evacuationStartClips));
        GroundTile.OnDelayedFactoryBuilt.AddListener(() => PlayRandomSfx(evacuationStartClips));
        GroundTile.OnHouseBuilt.AddListener(() => PlayRandomSfx(evacuationEndClips));
        GroundTile.OnFactoryBuilt.AddListener(() => PlayRandomSfx(evacuationEndClips));
        CorruptionHandler.OnSpread.AddListener(() => PlayRandomSfx(spreadClips));
        TurnManager.OnCorruptionTurnEnded.AddListener(() => PlayRandomSfx(turnStartClips));
        TurnManager.OnPlayerTurnEnded.AddListener(() => PlayRandomSfx(turnEndClips));
        TurnManager.OnGameWon.AddListener(() => PlayRandomSfx(victoryClips));
        TurnManager.OnGameLost.AddListener(() => PlayRandomSfx(failureClips));
    }

    void OnDisable() {
        GroundTile.OnWallBuilt.RemoveListener(() => PlayRandomSfx(wallBuildClips));
        GroundTile.OnHelicopterActivated.RemoveListener(() => PlayRandomSfx(helicopterClips));
        GroundTile.OnDelayedHouseBuilt.RemoveListener(() => PlayRandomSfx(evacuationStartClips));
        GroundTile.OnDelayedFactoryBuilt.RemoveListener(() => PlayRandomSfx(evacuationStartClips));
        GroundTile.OnHouseBuilt.RemoveListener(() => PlayRandomSfx(evacuationEndClips));
        GroundTile.OnFactoryBuilt.RemoveListener(() => PlayRandomSfx(evacuationEndClips));
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
