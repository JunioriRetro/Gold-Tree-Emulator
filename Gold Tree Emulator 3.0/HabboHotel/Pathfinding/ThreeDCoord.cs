using System;
namespace GoldTree.HabboHotel.Pathfinding
{
    public struct ThreeDCoord
    {
        internal int x;
        internal int y;
        internal ThreeDCoord(int _x, int _y)
        {
            this.x = _x;
            this.y = _y;
        }
        public static bool smethod_0(ThreeDCoord a, ThreeDCoord b)
        {
            return object.ReferenceEquals(a, b) || ((object)a != null && (object)b != null && a.x == b.x && a.y == b.y);
        }
        public static bool smethod_1(ThreeDCoord a, ThreeDCoord b)
        {
            return !ThreeDCoord.smethod_0(a, b);
        }
        public override int GetHashCode()
        {
            return this.x ^ this.y;
        }
        public override bool Equals(object obj)
        {
            return base.GetHashCode().Equals(obj.GetHashCode());
        }
    }
}
