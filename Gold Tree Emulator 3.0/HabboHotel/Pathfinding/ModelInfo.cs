using System;
namespace GoldTree.HabboHotel.Pathfinding
{
	internal struct ModelInfo
	{
        private byte[,] mMap;
        private int mMaxX;
        private int mMaxY;
        public ModelInfo(int MaxX, int MaxY, byte[,] Map)
        {
            this.mMap = Map;
            this.mMaxX = MaxX;
            this.mMaxY = MaxY;
        }

        internal byte GetState(int x, int y)
        {
            byte result;
            if (x >= this.mMaxX || x < 0)
            {
                result = 0;
            }
            else
            {
                if (y >= this.mMaxY || y < 0)
                {
                    result = 0;
                }
                else
                {
                    result = this.mMap[x, y];
                }
            }
            return result;
        }
	}
}
