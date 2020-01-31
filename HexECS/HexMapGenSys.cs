using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;
using aphx.Hex.Cpt;

namespace aphx.Hex
{
    [UpdateInGroup(typeof(SimulationSystemGroup))]
    public class HexMapGenSys : JobComponentSystem
    {
        // Grid不需要在ECS里生成, Behavior里即可
        // This declares a new kind of job, which is a unit of work to do.
        // The job is declared as an IJobForEach<Translation, Rotation>,
        // meaning it will process all entities in the world that have both
        // Translation and Rotation components. Change it to process the component
        // types you want.
        //
        // The job is also tagged with the BurstCompile attribute, which means
        // that the Burst compiler will optimize it for the best performance.
        /*
         * how ecs update ui:
         * protected override void OnUpdate(){
         *  var h = EntityManager.GetComponentData<HealthStateData>(localPlayer.controlledEntity);
                    var hud = EntityManager.GetComponentObject<IngameHUD>(localPlayer.hudEntity);
                    hud.FrameUpdate();
                    hud.m_Health.UpdateUI(ref h);
         * }
         */
        public NativeHashMap<AxialCoord, Entity> Tiles { get; set; }
        public NativeHashMap<AxialCoord, Entity> MapResources { get; set; }
        public NativeHashMap<AxialCoord, Entity> Units { get; set; }


        BeginInitializationEntityCommandBufferSystem entityCommandBufferSystem;

        protected override void OnCreate()
        {
            // Cache the BeginInitializationEntityCommandBufferSystem in a field, so we don't have to create it every frame
            entityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();
            Tiles = new NativeHashMap<AxialCoord, Entity>();
            MapResources = new NativeHashMap<AxialCoord, Entity>();
            Units = new NativeHashMap<AxialCoord, Entity>();
        }

        protected override JobHandle OnUpdate(JobHandle inputDependencies)
        {
            return inputDependencies;
            /*

                var commandBuffer = entityCommandBufferSystem.CreateCommandBuffer().ToConcurrent();
                var jobHandle = Entities
                   .WithName("HexMapGen")
                   .WithBurst(FloatMode.Default, FloatPrecision.Standard, true)
                   .ForEach((Entity entity, int entityInQueryIndex, in LocalToWorld location, in MapStyle mapStyle ) =>
                   {
                       float3 startPos = location.Position;
                       float3 pos = float3.zero;
                       var tilePossArr = new int[] { mapStyle.SandPoss};
                       var tilePrefabs = new Entity[] { mapStyle.SandPrefab};
                       HexMgr.Instance.RandomEntities(Tiles, mapStyle.MapSize, 
                           tilePossArr, mapStyle.HexOuterRadius, mapStyle.tileLayerHeight, location.Position, 
                           mapStyle.GrassPrefab, tilePrefabs, entityInQueryIndex, commandBuffer);

                   }).Schedule(inputDependencies);
                return jobHandle;
            */

        }
    }
    }