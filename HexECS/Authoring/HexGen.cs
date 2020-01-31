using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using aphx.Hex;

// ReSharper disable once InconsistentNaming
//[RequiresEntityConversion]
[AddComponentMenu("Hex/HexGen")]
//[ConverterVersion("aphx", 1)]
public class HexGen: MonoBehaviour//, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{
    public GameObject GrassPrefab;
    public GameObject SandPrefab;
    public int SandPoss;
    public GameObject ForestPrefab;
    public int ForestPoss;
    public int Players;
    public int HexOuterRadius;
    public int MapSize;
    public float tileLayerHeight;
    public float unitLayerHeight;
    public float resLayerHeight;

    private void Start()
    {
        float3 startPos = transform.position;
        var tilePossArr = new int[] { SandPoss };
        var tilePrefabs = new GameObject[] { SandPrefab };
        HexMgr.Instance.RandomGO(HexMgr.Instance.Tiles,MapSize,
           tilePossArr, HexOuterRadius, tileLayerHeight, startPos,
           GrassPrefab, tilePrefabs);
    }

    /*
    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {
        referencedPrefabs.Add(GrassPrefab);
        referencedPrefabs.Add(SandPrefab);
        referencedPrefabs.Add(ForestPrefab);
    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {
        var mapStyle = new MapStyle
        {
            ForestPoss = ForestPoss,
            SandPoss = SandPoss,
            Players = Players,
            HexOuterRadius = HexOuterRadius,
            MapSize = MapSize,
            tileLayerHeight = tileLayerHeight,
            unitLayerHeight = unitLayerHeight,
            resLayerHeight = resLayerHeight,
            ForestPrefab = conversionSystem.GetPrimaryEntity(ForestPrefab),
            SandPrefab = conversionSystem.GetPrimaryEntity(SandPrefab),
            GrassPrefab = conversionSystem.GetPrimaryEntity(GrassPrefab)
        };
        dstManager.AddSharedComponentData(entity, mapStyle);
    }
    */



}
