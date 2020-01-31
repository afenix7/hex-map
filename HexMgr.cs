using aphx.Hex.Cpt;
using System;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace aphx.Hex
{
    public class HexMgr
    {
        const float HALF_SQRT_THREE = 0.866025404f;
        const float SQRT_THREE = HALF_SQRT_THREE * 2;
        const float SQRT_THREE_OF_THREE = SQRT_THREE / 3;
        const float TWO_OF_THREE = 2.0f / 3.0f;
        const float NEG_ONE_OF_THREE = -1.0f / 3.0f;

        public int2[] Dirs = new int2[] {new int2(+1, 0), new int2(+1, -1), new int2(0, -1),
                new int2(-1, 0), new int2(-1, +1), new int2(0, +1) };

        static HexMgr instance;
        float innerRadius;


        public static HexMgr Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new HexMgr();
                }
                return instance;
            }
        }

        private HexMgr()
        {
            Tiles = new Dictionary<AxialCoord, GameObject>();
            MapResources = new Dictionary<AxialCoord, GameObject>();
            Units = new Dictionary<AxialCoord, GameObject>();
        }

        /*
        public NativeHashMap<AxialCoord,Entity> Tiles { get; set; }
        public NativeHashMap<AxialCoord, Entity> MapResources { get; set; }
        public NativeHashMap<AxialCoord, Entity> Units { get; set; }
        */

        public Dictionary<AxialCoord, GameObject> Tiles { get; set; }
        public Dictionary<AxialCoord, GameObject> MapResources { get; set; }
        public Dictionary<AxialCoord, GameObject> Units { get; set; }

        public float WaterLevel { get; set; }

        public float OuterRadius
        {
            get; set;
        }

        public int Size { get; set; }

        public float InnerRadius
        {
            get
            {
                if (innerRadius < 1)
                {
                    innerRadius = OuterRadius * SQRT_THREE;
                }
                return innerRadius;
            }
        }

        public void Init(float outerRadius,int size, float waterLevel)
        {
            OuterRadius = outerRadius;
            Size = size;
            WaterLevel = waterLevel;
            
        }

        public int3 AxialToCube(ref AxialCoord ac)
        {
            return new int3(ac.Value.x, -ac.Value.x - ac.Value.y, ac.Value.y);
        }

        public int Distance(ref AxialCoord a, ref AxialCoord b)
        {
            int3 cubeA = AxialToCube(ref a);
            int3 cubeB = AxialToCube(ref b);
            return math.abs(cubeA.x - cubeB.x) + math.abs(cubeA.y - cubeB.y) + math.abs(cubeA.x - cubeB.y);
        }

        public AxialCoord PosToAxialCoord(float3 pos,float outerRadius)
        {
            float2 result;
            int2 coord = int2.zero;
            float2x2 matConvert;
            matConvert = new float2x2(SQRT_THREE_OF_THREE, NEG_ONE_OF_THREE, 0, TWO_OF_THREE);
            result = (matConvert * new float2x2(pos.x, 0, pos.z, 0) / outerRadius).c0;
            coord.x = math.asint(math.round(result.x));
            coord.y = math.asint(math.round(result.y));
            AxialCoord ac = new AxialCoord();
            ac.Value = coord;
            return ac;
        }

        public float3 AxialCoordToPos(ref AxialCoord coord,float outerRadius,float height)
        {
            float2 result;
            float3 pos = float3.zero;
            float2x2 matConvert;
            matConvert = new float2x2(1.5f, 0, HALF_SQRT_THREE, SQRT_THREE);
            result = (outerRadius * matConvert * new float2x2(coord.Value.x, 0, coord.Value.y, 0)).c0;
            pos.x = result.x;
            pos.z = result.y;
            pos.y = height;
            return pos;
        }

        public NativeList<AxialCoord> NeighbourAxialCoords(ref AxialCoord center)
        {
            NativeList<AxialCoord> acList = new NativeList<AxialCoord>();
            for (int i=0; i < Dirs.Length; i++){
                AxialCoord ac = new AxialCoord()
                {
                    Value = new int2(center.Value.x + Dirs[i].x, center.Value.y + Dirs[i].y)
                };
                acList.Add(ac);
            }
            return acList;
        }

        public NativeList<Entity> NeighbourAxialCoords(NativeHashMap<AxialCoord, Entity> entityMap,ref AxialCoord center)
        {
            NativeList<Entity> entities = new NativeList<Entity>();
            for (int i = 0; i < Dirs.Length; i++)
            {
                AxialCoord ac = new AxialCoord()
                {
                    Value = new int2(center.Value.x + Dirs[i].x, center.Value.y + Dirs[i].y)
                };
                if (entityMap.ContainsKey(ac))
                    entities.Add(entityMap[ac]);
            }
            return entities;
        }

        public NativeList<Entity> EntitiesInRange(NativeHashMap<AxialCoord,Entity> entityMap,ref AxialCoord center,int range)
        {
            NativeList<Entity> ret = new NativeList<Entity>();
            AxialCoord ac;
            for (int dx = -range; dx <= range; dx++)
            {
                for (int dy = math.max(-range, -dx - range); dy <= math.min(range, -dx + range); dy++)
                {
                    ac = new AxialCoord() { Value = new int2(dx, -dx - dy) + center.Value };
                    if (entityMap.ContainsKey(ac))
                        ret.Add(entityMap[ac]);
                }
            }
            return ret;
        }

        public List<GameObject> GOInRange(Dictionary<AxialCoord, GameObject> entityMap, ref AxialCoord center, int range)
        {
            List<GameObject> ret = new List<GameObject>();
            AxialCoord ac;
            for (int dx = -range; dx <= range; dx++)
            {
                for (int dy = math.max(-range, -dx - range); dy <= math.min(range, -dx + range); dy++)
                {
                    ac = new AxialCoord() { Value = new int2(dx, -dx - dy) + center.Value };
                    if (entityMap.ContainsKey(ac))
                        ret.Add(entityMap[ac]);
                }
            }
            return ret;
        }

        public NativeList<AxialCoord> AxialCoordInRange(NativeHashMap<AxialCoord, Entity> entityMap, ref AxialCoord center, int range)
        {
            NativeList<AxialCoord> ret = new NativeList<AxialCoord>();
            AxialCoord ac;

            for (int dx = -range; dx <= range; dx++)
            {
                for (int dy = math.max(-range, -dx - range); dy <= math.min(range, -dx + range); dy++)
                {
                    ac = new AxialCoord() { Value = new int2(dx, -dx - dy) + center.Value };
                    if (entityMap.ContainsKey(ac))
                        ret.Add(ac);
                }
            }
            return ret;
        }

        public NativeList<AxialCoord> RandomAxialCoords(int size,int num)
        {
            NativeList<AxialCoord> acList = new NativeList<AxialCoord>();
            for (int i = 0; i < num; i++)
            {
                var random = new Unity.Mathematics.Random(1);
                //random.InitState();
                int2 coord = random.NextInt2(-size, size);
                acList.Add(new AxialCoord() { Value = coord });
            }
            return acList;
        }

        public void RandomGO(Dictionary<AxialCoord,GameObject> entityMap, int mapSize, int[] entityPossArr, float outerRadius, float layerHeight, float3 startPos, GameObject basePrefab, GameObject[] placedPrefabs)
        {
            var random = new Unity.Mathematics.Random(1);
            for (int q = -mapSize; q <= mapSize; q++)
            {
                int r1 = Mathf.Max(-mapSize, -q - mapSize);
                int r2 = Mathf.Min(mapSize, -q + mapSize);
                for (int r = r1; r <= r2; r++)
                {
                    AxialCoord ac = new AxialCoord() { Value = new int2(q, r) };
                    if (entityMap.Count > 0 && entityMap.ContainsKey(ac))
                    {
                        continue;
                    }
                    float3 pos = startPos + AxialCoordToPos(ref ac, outerRadius, layerHeight);
                    int gameResult = random.NextInt(0, 100);
                    GameObject instance;
                    bool alreadyCreated = false;
                    GameObject instancePrefab = basePrefab;
                    for (int i = 0; i < placedPrefabs.Length; i++)
                    {
                        if (!alreadyCreated && gameResult >= entityPossArr[i])
                        {
                            instancePrefab = placedPrefabs[i];
                            alreadyCreated = true;
                            break;
                        }
                    }
                    instance = UnityEngine.Object.Instantiate(instancePrefab);
                    instance.transform.position = pos;
                    HexAxialCoord axialCoord = instance.GetComponent<HexAxialCoord>();
                    axialCoord.q = q;
                    axialCoord.r = r;
                    entityMap.Add(ac, instance);
                }
            }
        }

        public void RandomEntities(NativeHashMap<AxialCoord,Entity> entityMap,int mapSize,int[] entityPossArr, float outerRadius,float layerHeight,float3 startPos,Entity basePrefab,Entity[] placedPrefabs,int jobIdx,EntityCommandBuffer.Concurrent commandBuffer)
        {
            var random = new Unity.Mathematics.Random(1);
            for (int q = -mapSize; q <= mapSize; q++)
            {
                int r1 = Mathf.Max(-mapSize, -q - mapSize);
                int r2 = Mathf.Min(mapSize, -q + mapSize);
                for (int r = r1; r <= r2; r++)
                {
                    AxialCoord ac = new AxialCoord() { Value = new int2(q, r) };
                    if (entityMap.Length>0&&entityMap.ContainsKey(ac))
                    {
                        continue;
                    }
                    float3 pos = startPos + AxialCoordToPos(ref ac, outerRadius, layerHeight);
                    int gameResult = random.NextInt(0, 100);
                    Entity instance;
                    bool alreadyCreated = false;
                    Entity instancePrefab = basePrefab;
                    for(int i = 0; i < placedPrefabs.Length; i++)
                    {
                        if (!alreadyCreated&&gameResult >= entityPossArr[i])
                        {
                            instancePrefab = placedPrefabs[i];
                            alreadyCreated = true;
                            break;
                        }
                    }
                    instance = commandBuffer.Instantiate(jobIdx, instancePrefab);
                    commandBuffer.SetComponent(jobIdx, instance, new Translation { Value = pos });
                    commandBuffer.SetComponent(jobIdx, instance, ac);
                    entityMap.Add(ac, instance);
                }
            }
        }

        public bool IsCoordInRange(int2 ac,int2 center, int range)
        {
            return ac.x >= -center.x - range && ac.x <= center.x + range
                &&ac.y>=-center.y-range&&ac.y<=center.y+range;
        }

        public void RandomRangeEntity(NativeHashMap<AxialCoord, Entity> entityMap,int mapSize, float outerRadius, float layerHeight, Entity placedPrefab, int jobIdx, EntityCommandBuffer.Concurrent commandBuffer)
        {
            var random = new Unity.Mathematics.Random(1);
            int range = random.NextInt(mapSize);
            int2 center = random.NextInt2(-mapSize-range, mapSize+range);
            for (int dx = -range; dx <= range; dx++)
            {
                for (int dy = math.max(-range, -dx - range); dy <= math.min(range, -dx + range); dy++)
                {
                    AxialCoord ac = new AxialCoord() { Value = new int2(dx, -dx - dy) + center };
                    float3 pos = AxialCoordToPos(ref ac, outerRadius, layerHeight);
                    var instance = commandBuffer.Instantiate(jobIdx, placedPrefab);
                    commandBuffer.SetComponent(jobIdx, instance, new Translation { Value = pos });
                    commandBuffer.SetComponent(jobIdx, instance, ac);
                    entityMap.Add(ac, instance);
                }
            }
        }

        //eg.lake, forest, moutain, greeland
        //before RandomEntities
        public void RandomRangeEntites(NativeHashMap<AxialCoord, Entity> entityMap,int maxNum, int mapSize, float outerRadius, float layerHeight, Entity placedPrefab, int jobIdx, EntityCommandBuffer.Concurrent commandBuffer)
        {
            var random = new Unity.Mathematics.Random(1);
            //AxialCoord center = new AxialCoord() { Value = random.NextInt2(-mapSize+range,mapSize-range) };
            //NativeList<Entity> entities = EntitiesInRange(entityMap, ref center, range);
            //不规则物体: 算出最大和最小, 每次超过最小时按概率是否显示
            //多个: 数量(单个最大要小于地图size/数量) 和重叠问题(每个的center要在各自的range之外)
            //因此前面的range要重新算
            int num = random.NextInt(maxNum);
            int maxRange = math.asint(math.round(mapSize / num));
            NativeList<int2> centerList = new NativeList<int2>();
            NativeList<int> rangeList = new NativeList<int>();
            for (int i = 0; i < num; i++)
            {
                int range = random.NextInt(maxRange);
                int2 center = random.NextInt2(-mapSize-range,mapSize+range);
                for (int dx = -range; dx <= range; dx++)
                {
                    for (int dy = math.max(-range, -dx - range); dy <= math.min(range, -dx + range); dy++)
                    {
                        AxialCoord ac = new AxialCoord() { Value = new int2(dx, -dx - dy) + center };
                        float3 pos = AxialCoordToPos(ref ac, outerRadius, layerHeight);
                        var instance = commandBuffer.Instantiate(jobIdx, placedPrefab);
                        commandBuffer.SetComponent(jobIdx, instance, new Translation { Value = pos });
                        commandBuffer.SetComponent(jobIdx, instance, ac);
                        entityMap.Add(ac, instance);
                    }
                }
            }
            
        }
    }
}
