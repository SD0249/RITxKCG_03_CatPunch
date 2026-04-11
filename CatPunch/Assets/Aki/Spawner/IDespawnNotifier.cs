using UnityEngine;

public interface IDespawnNotifier
{
    public event System.Action OnDespawn;
}

// 敵が消えた時にこの下のコードを呼び出してほしい。以下英訳
// When the enemy despawns, please call the code below.
//  ↓
// これ持ってて(please have this)
// public event System.Action OnDespawn;
// 
// そして敵が消えるときにこれを呼び出してほしい(And when the enemy despawns, please call this)
// OnDespawn?.Invoke();
//