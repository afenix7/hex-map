using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

[RequiresEntityConversion]
[AddComponentMenu("Hex ECS/Map")]
[ConverterVersion("aphx", 1)]
public class HexMap : MonoBehaviour, IDeclareReferencedPrefabs, IConvertGameObjectToEntity
{

    int mapSize;
    int players;

    private void OnDrawGizmosSelected()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    void createMapGrids(int gridsPerDir)
    {
        
    }

    public void DeclareReferencedPrefabs(List<GameObject> referencedPrefabs)
    {

    }

    public void Convert(Entity entity, EntityManager dstManager, GameObjectConversionSystem conversionSystem)
    {

    }
}
