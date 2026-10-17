using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Small singleton to keep track of players/enemies and provide multiplayer hooks.
/// Networking systems can call RegisterPlayer/UnregisterPlayer and broadcast events.
/// </summary>
public class GameManager : MonoBehaviour
{
    public static GameManager I { get; private set; }

    private List<PlayerController> players = new List<PlayerController>();

    void Awake()
    {
        if (I == null) I = this;
        else Destroy(gameObject);
    }

    public void RegisterPlayer(PlayerController p)
    {
        if (!players.Contains(p)) players.Add(p);
    }

    public void UnregisterPlayer(PlayerController p)
    {
        if (players.Contains(p)) players.Remove(p);
    }

    public IReadOnlyList<PlayerController> GetPlayers() => players.AsReadOnly();

    // network-friendly spawn hook
    public virtual void RequestSpawnPlayer(Vector2 position)
    {
        // network layer should implement spawn logic and then call RegisterPlayer on spawned instance
    }
}
