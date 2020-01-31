using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct UnitTypes : ISharedComponentData
{
    public Entity BaseMale;
    public Entity BaseFemale;
    public Entity ShangMaleCivilian;
    public Entity ShangFemaleCivilian;
    public Entity ShangMaleNoble;
    public Entity ShangFemaleNoble;
    public Entity ShangFarmer;
    public Entity ShanWorker;
    public Entity ShangHunter;
    public Entity ShangWarrior;
    public Entity ShangElite;
    public Entity ShangCalvary;
    public Entity ShangChariot;
    public Entity ShangWitcher;
    public Entity ShangKing;

    public Entity ZhouMaleCivilian;
    public Entity ZhouFemaleCivilian;
    public Entity ZhouMaleNoble;
    public Entity ZhouFemaleNoble;
    public Entity ZhouFarmer;
    public Entity ZhouWorker;
    public Entity ZhouHunter;
    public Entity ZhouWarrior;
    public Entity ZhouElite;
    public Entity ZhouCalvary;
    public Entity ZhouChariot;
    public Entity ZhouTianzi;

    public Entity ChuMaleCivilian;
    public Entity ChuFemaleCivilian;
    public Entity ChuMaleNoble;
    public Entity ChuFemaleNoble;
    public Entity ChuFarmer;
    public Entity ChuWorker;
    public Entity ChuHunter;
    public Entity ChuWarrior;
    public Entity ChuElite;
    public Entity ChuChariot;
    public Entity ChuWitcher;

    public Entity YueMaleCivilian;
    public Entity YueFemaleCivilian;
    public Entity YueMaleNoble;
    public Entity YueFemaleNoble;
    public Entity YueHunter;
    public Entity YueWarrior;
    public Entity YueElite;
    public Entity YueWitcher;

    public Entity QiangMale;
    public Entity QiangFemale;
    public Entity QiangWitcher;

    public Entity NomadicMale;
    public Entity NomadicHunter;
    public Entity NomadicWarrior;
    public Entity NomadicCalvary;
}
