using System;
using System.Globalization;
using System.Text;
using GoldTree.Messages;
using GoldTree.Core;
namespace GoldTree.HabboHotel.Rooms
{
	internal sealed class RoomModel
	{
		public string Name;
		public int int_0;
		public int int_1;
		public double double_0;
		public int int_2;
		public string string_1;
		public SquareState[,] squareState;
		public double[,] double_1;
		public int[,] int_3;
		public int int_4;
		public int int_5;
		public string string_2;
		public bool bool_0;
		public RoomModel(string string_3, int int_6, int int_7, double double_2, int int_8, string string_4, string string_5, bool bool_1)
		{
            try
            {
                this.Name = string_3;
                this.int_0 = int_6;
                this.int_1 = int_7;
                this.double_0 = double_2;
                this.int_2 = int_8;
                this.string_1 = string_4.ToLower();
                this.string_2 = string_5;
                string[] array = string_4.Split(new char[]
			{
				Convert.ToChar(13)
			});
                this.int_4 = array[0].Length;
                this.int_5 = array.Length;
                this.bool_0 = bool_1;
                this.squareState = new SquareState[this.int_4, this.int_5];
                this.double_1 = new double[this.int_4, this.int_5];
                this.int_3 = new int[this.int_4, this.int_5];
                for (int i = 0; i < this.int_5; i++)
                {
                    if (i > 0)
                    {
                        array[i] = array[i].Substring(1);
                    }
                    for (int j = 0; j < this.int_4; j++)
                    {
                        string text = array[i].Substring(j, 1).Trim().ToLower();
                        if (text == "x")
                        {
                            this.squareState[j, i] = SquareState.BLOCKED;
                        }
                        else
                        {
                            if (this.method_0(text, NumberStyles.Integer))
                            {
                                this.squareState[j, i] = SquareState.OPEN;
                                this.double_1[j, i] = double.Parse(text);
                            }
                            //else
                            //{
                            //tähän tulee jotain shaieessee sikku tulee xD
                            //}
                        }
                    }
                }
                this.double_1[int_6, int_7] = double_2;
                int num = 0;
                int num2 = 0;
                if (string_5 != "")
                {
                    num2 = OldEncoding.decodeVL64(string_5);
                }
                num += OldEncoding.encodeVL64(num2).Length;
                for (int k = 0; k < num2; k++)
                {
                    string_5.Substring(num);
                    int num3 = OldEncoding.decodeVL64(string_5.Substring(num));
                    num += OldEncoding.encodeVL64(num3).Length;
                    string_5.Substring(num, 1);
                    num++;
                    int.Parse(string_5.Substring(num).Split(new char[]
				{
					Convert.ToChar(2)
				})[0]);
                    num += string_5.Substring(num).Split(new char[]
				{
					Convert.ToChar(2)
				})[0].Length;
                    num++;
                    string text2 = string_5.Substring(num).Split(new char[]
				{
					Convert.ToChar(2)
				})[0];
                    num += string_5.Substring(num).Split(new char[]
				{
					Convert.ToChar(2)
				})[0].Length;
                    num++;
                    int j = OldEncoding.decodeVL64(string_5.Substring(num));
                    num += OldEncoding.encodeVL64(j).Length;
                    int i = OldEncoding.decodeVL64(string_5.Substring(num));
                    num += OldEncoding.encodeVL64(i).Length;
                    int num4 = OldEncoding.decodeVL64(string_5.Substring(num));
                    num += OldEncoding.encodeVL64(num4).Length;
                    int num5 = OldEncoding.decodeVL64(string_5.Substring(num));
                    num += OldEncoding.encodeVL64(num5).Length;
                    this.squareState[j, i] = SquareState.BLOCKED;
                    if (text2.Contains("bench") || text2.Contains("chair") || text2.Contains("stool") || text2.Contains("seat") || text2.Contains("sofa"))
                    {
                        this.squareState[j, i] = SquareState.SEAT;
                        this.int_3[j, i] = num5;
                    }
                }
            }
            catch (Exception ex)
            {
                Logging.LogRoomError(ex.ToString());
            }
		}
		public bool method_0(string string_3, NumberStyles numberStyles_0)
		{
			double num;
			return double.TryParse(string_3, numberStyles_0, CultureInfo.InvariantCulture, out num);
		}
		public ServerMessage method_1()
		{
            try
            {
                StringBuilder stringBuilder = new StringBuilder();
                string[] array = this.string_1.Split("\r\n".ToCharArray());
                for (int i = 0; i < array.Length; i++)
                {
                    string text = array[i];
                    if (!(text == ""))
                    {
                        stringBuilder.Append(text);
                        stringBuilder.Append(Convert.ToChar(13));
                    }
                }
                ServerMessage Message = new ServerMessage(31u);
                Message.AppendStringWithBreak(stringBuilder.ToString());
                return Message;
            }
            catch (Exception ex)
            {
                Logging.LogRoomError(ex.ToString());
                return null;
            }
		}
		public ServerMessage method_2()
		{
            try
            {
                ServerMessage Message = new ServerMessage(470u);
                string[] array = this.string_1.Split(new char[]
			{
				Convert.ToChar(13)
			});
                for (int i = 0; i < this.int_5; i++)
                {
                    if (i > 0)
                    {
                        array[i] = array[i].Substring(1);
                    }
                    for (int j = 0; j < this.int_4; j++)
                    {
                        string text = array[i].Substring(j, 1).Trim().ToLower();
                        if (this.int_0 == j && this.int_1 == i)
                        {
                            text = string.Concat((int)this.double_0);
                        }
                        Message.AppendString(text);
                    }
                    Message.AppendString(string.Concat(Convert.ToChar(13)));
                }
                return Message;
            }
            catch (Exception ex)
            {
                Logging.LogRoomError(ex.ToString());
                return null;
            }
		}
	}
}
