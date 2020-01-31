using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct BuildingTypes : ISharedComponentData
{
    // Add fields to your component here. Remember that:
    //
    // * A component itself is for storing data and doesn't 'do' anything.
    //
    // * To act on the data, you will need a System.
    //
    // * Data in a component must be blittable, which means a component can
    //   only contain fields which are primitive types or other blittable
    //   structs; they cannot contain references to classes.
    //
    // * You should focus on the data structure that makes the most sense
    //   for runtime use here. Authoring Components will be used for 
    //   authoring the data in the Editor.
    public Entity Hut;  //低级住房
    public Entity House;  //
    public Entity SouthernHut;  //高脚低级住房
    public Entity SouthernHouse;  //高脚高级住房
    public Entity Palace;
    public Entity Workshop; //低级工厂
    public Entity Factory;  //高级工厂
    public Entity Altar;  //低级祭祀
    public Entity Temple;  //高级祭祀
    public Entity Field;  //粟稷田
    public Entity RiceField;  //水稻田
    public Entity Camp;
    public Entity Tower;  //低级塔楼
    public Entity Fortress;  //高级塔楼
    public Entity Gate;  //城门
}
