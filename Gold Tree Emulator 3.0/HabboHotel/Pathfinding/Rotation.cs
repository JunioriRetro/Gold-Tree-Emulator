using System;
namespace GoldTree.HabboHotel.Pathfinding
{
	internal sealed class Class107
	{
		public static int smethod_0(int int_0, int int_1, int int_2, int int_3)
		{
			int result = 0;
			if (int_0 > int_2 && int_1 > int_3)
			{
				result = 7;
			}
			else
			{
				if (int_0 < int_2 && int_1 < int_3)
				{
					result = 3;
				}
				else
				{
					if (int_0 > int_2 && int_1 < int_3)
					{
						result = 5;
					}
					else
					{
						if (int_0 < int_2 && int_1 > int_3)
						{
							result = 1;
						}
						else
						{
							if (int_0 > int_2)
							{
								result = 6;
							}
							else
							{
								if (int_0 < int_2)
								{
									result = 2;
								}
								else
								{
									if (int_1 < int_3)
									{
										result = 4;
									}
									else
									{
										if (int_1 > int_3)
										{
											result = 0;
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}
		public static int smethod_1(int int_0, int int_1, int int_2, int int_3)
		{
			int result = 0;
			if (int_0 > int_2 && int_1 > int_3)
			{
				result = 3;
			}
			else
			{
				if (int_0 < int_2 && int_1 < int_3)
				{
					result = 7;
				}
				else
				{
					if (int_0 > int_2 && int_1 < int_3)
					{
						result = 1;
					}
					else
					{
						if (int_0 < int_2 && int_1 > int_3)
						{
							result = 5;
						}
						else
						{
							if (int_0 > int_2)
							{
								result = 2;
							}
							else
							{
								if (int_0 < int_2)
								{
									result = 6;
								}
								else
								{
									if (int_1 < int_3)
									{
										result = 0;
									}
									else
									{
										if (int_1 > int_3)
										{
											result = 4;
										}
									}
								}
							}
						}
					}
				}
			}
			return result;
		}
	}
}
