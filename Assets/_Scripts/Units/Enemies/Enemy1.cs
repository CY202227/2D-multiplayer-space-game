using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : BaseEnemy
{
    // Start is called before the first frame update
    private Renderer[] _enemyRenderers;

    private void Start()
    {
        _enemyRenderers = GetComponentsInChildren<Renderer>();
    }

    public void SetEnemyVisibility(bool isVisible)
    {
        foreach (Renderer renderer in _enemyRenderers)
        {
            renderer.enabled = isVisible;
        }
    }
}
