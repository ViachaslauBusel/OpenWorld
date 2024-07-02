using OpenWorld;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadTestMap : MonoBehaviour
{
    [SerializeField]
    private MapLoader _mapLoader;

    private void Start()
    {
        _mapLoader.SetTarget(transform);
        _mapLoader.LoadMap();
    }
}
