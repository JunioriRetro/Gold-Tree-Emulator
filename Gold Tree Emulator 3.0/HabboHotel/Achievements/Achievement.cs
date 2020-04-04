using System;
namespace GoldTree.HabboHotel.Achievements
{
	internal sealed class Achievement
	{
		public uint Id;
		public string Type;
		public int Levels;
		public string BadgeCode;
		public int Dynamic_badgelevel;
		public int ScoreBase;
		public double PixelMultiplier;
		public bool DynamicBadgeLevel;
        public int PixelBase;
		public Achievement(uint mId, string mType, int mLevels, string mBadgeCode, int mDynamic_badgelevel, double mPixelMultiplier, bool mDynamicBadgeLevel, int mScoreBase, int mPixelBase)
		{
			this.Id = mId;
			this.Type = mType;
			this.Levels = mLevels;
			this.BadgeCode = mBadgeCode;
			this.Dynamic_badgelevel = mDynamic_badgelevel;
			this.ScoreBase = mScoreBase;
			this.PixelMultiplier = mPixelMultiplier;
			this.DynamicBadgeLevel = mDynamicBadgeLevel;
            this.PixelBase = mPixelBase;
		}
	}
}
