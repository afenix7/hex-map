using System;
using Unity.Entities;
using Unity.Mathematics;

namespace aphx.Hex.Cpt
{
    public struct AxialCoord : IComponentData, IEquatable<AxialCoord>
    {
        public int2 Value;

        public bool Equals(AxialCoord other)
        {
            return this.Value.Equals(other.Value);
        }
    }
}