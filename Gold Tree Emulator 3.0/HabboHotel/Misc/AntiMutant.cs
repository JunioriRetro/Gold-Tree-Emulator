using GoldTree.Core;
using GoldTree.HabboHotel.Rooms;
using System;
using System.Text.RegularExpressions;
namespace GoldTree.HabboHotel.Misc
{
    internal sealed class AntiMutant
    {
        public static bool ValidateLook(string Look, string Gender)
        {
            bool flag = false;
            bool result;
            if (Look.Length < 1)
            {
                result = false;
            }
            else
            {
                try
                {
                    string[] array = Look.Split(new char[]
					{
						'.'
					});
                    if (array.Length < 2)
                    {
                        result = false;
                        return result;
                    }
                    string[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        string text = array2[i];
                        string[] array3 = text.Split(new char[]
						{
							'-'
						});
                        if (array3.Length < 3)
                        {
                            result = false;
                            return result;
                        }
                        string text2 = array3[0];
                        int num = int.Parse(array3[1]);
                        int num2 = int.Parse(array3[1]);
                        if (num <= 0 || num2 < 0)
                        {
                            result = false;
                            return result;
                        }
                        if (text2.Length != 2)
                        {
                            result = false;
                            return result;
                        }
                        if (text2 == "hd")
                        {
                            flag = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logging.LogThreadException(ex.ToString(), "AntiMutant.ValidateLook");
                    result = false;
                    return result;
                }
                result = (flag && (!(Gender != "M") || !(Gender != "F")));
            }
            return result;
        }
        public static void smethod_1(RoomUser RoomUser_0, string string_0)
        {
            if (string_0.Contains("hr-"))
            {
                string text = Regex.Split(string_0, "hr-")[1];
                text = "hr-" + text.Split(new char[]
				{
					'.'
				})[0];
                int num = RoomUser_0.GetClient().GetHabbo().Figure.IndexOf("hr-");
                if (num == -1)
                {
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure + text;
                }
                else
                {
                    string text2 = Regex.Split(RoomUser_0.GetClient().GetHabbo().Figure, "hr-")[1];
                    text2 = "hr-" + text2.Split(new char[]
					{
						'.'
					})[0];
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure.Replace(text2, text);
                }
            }
            if (string_0.Contains("hd-"))
            {
                string text = Regex.Split(string_0, "hd-")[1];
                text = "hd-" + text.Split(new char[]
				{
					'.'
				})[0];
                int num = RoomUser_0.GetClient().GetHabbo().Figure.IndexOf("hd-");
                if (num == -1)
                {
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure + text;
                }
                else
                {
                    string text2 = Regex.Split(RoomUser_0.GetClient().GetHabbo().Figure, "hd-")[1];
                    text2 = "hd-" + text2.Split(new char[]
					{
						'.'
					})[0];
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure.Replace(text2, text);
                }
            }
            if (string_0.Contains("ch-"))
            {
                string text = Regex.Split(string_0, "ch-")[1];
                text = "ch-" + text.Split(new char[]
				{
					'.'
				})[0];
                int num = RoomUser_0.GetClient().GetHabbo().Figure.IndexOf("ch-");
                if (num == -1)
                {
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure + text;
                }
                else
                {
                    string text2 = Regex.Split(RoomUser_0.GetClient().GetHabbo().Figure, "ch-")[1];
                    text2 = "ch-" + text2.Split(new char[]
					{
						'.'
					})[0];
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure.Replace(text2, text);
                }
            }
            if (string_0.Contains("lg-"))
            {
                string text = Regex.Split(string_0, "lg-")[1];
                text = "lg-" + text.Split(new char[]
				{
					'.'
				})[0];
                int num = RoomUser_0.GetClient().GetHabbo().Figure.IndexOf("lg-");
                if (num == -1)
                {
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure + text;
                }
                else
                {
                    string text2 = Regex.Split(RoomUser_0.GetClient().GetHabbo().Figure, "lg-")[1];
                    text2 = "lg-" + text2.Split(new char[]
					{
						'.'
					})[0];
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure.Replace(text2, text);
                }
            }
            if (string_0.Contains("sh-"))
            {
                string text = Regex.Split(string_0, "sh-")[1];
                text = "sh-" + text.Split(new char[]
				{
					'.'
				})[0];
                int num = RoomUser_0.GetClient().GetHabbo().Figure.IndexOf("sh-");
                if (num == -1)
                {
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure + text;
                }
                else
                {
                    string text2 = Regex.Split(RoomUser_0.GetClient().GetHabbo().Figure, "sh-")[1];
                    text2 = "sh-" + text2.Split(new char[]
					{
						'.'
					})[0];
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure.Replace(text2, text);
                }
            }
            if (string_0.Contains("ea-"))
            {
                string text = Regex.Split(string_0, "ea-")[1];
                text = "ea-" + text.Split(new char[]
				{
					'.'
				})[0];
                int num = RoomUser_0.GetClient().GetHabbo().Figure.IndexOf("ea-");
                if (num == -1)
                {
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure + text;
                }
                else
                {
                    string text2 = Regex.Split(RoomUser_0.GetClient().GetHabbo().Figure, "ea-")[1];
                    text2 = "ea-" + text2.Split(new char[]
					{
						'.'
					})[0];
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure.Replace(text2, text);
                }
            }
            if (string_0.Contains("ca-"))
            {
                string text = Regex.Split(string_0, "ca-")[1];
                text = "ca-" + text.Split(new char[]
				{
					'.'
				})[0];
                int num = RoomUser_0.GetClient().GetHabbo().Figure.IndexOf("ca-");
                if (num == -1)
                {
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure + text;
                }
                else
                {
                    string text2 = Regex.Split(RoomUser_0.GetClient().GetHabbo().Figure, "ca-")[1];
                    text2 = "ca-" + text2.Split(new char[]
					{
						'.'
					})[0];
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure.Replace(text2, text);
                }
            }
            if (string_0.Contains("ha-"))
            {
                string text = Regex.Split(string_0, "ha-")[1];
                text = "ha-" + text.Split(new char[]
				{
					'.'
				})[0];
                int num = RoomUser_0.GetClient().GetHabbo().Figure.IndexOf("ha-");
                if (num == -1)
                {
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure + text;
                }
                else
                {
                    string text2 = Regex.Split(RoomUser_0.GetClient().GetHabbo().Figure, "ha-")[1];
                    text2 = "ha-" + text2.Split(new char[]
					{
						'.'
					})[0];
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure.Replace(text2, text);
                }
            }
            if (string_0.Contains("he-"))
            {
                string text = Regex.Split(string_0, "he-")[1];
                text = "he-" + text.Split(new char[]
				{
					'.'
				})[0];
                int num = RoomUser_0.GetClient().GetHabbo().Figure.IndexOf("he-");
                if (num == -1)
                {
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure + text;
                }
                else
                {
                    string text2 = Regex.Split(RoomUser_0.GetClient().GetHabbo().Figure, "he-")[1];
                    text2 = "he-" + text2.Split(new char[]
					{
						'.'
					})[0];
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure.Replace(text2, text);
                }
            }
            if (string_0.Contains("wa-"))
            {
                string text = Regex.Split(string_0, "wa-")[1];
                text = "wa-" + text.Split(new char[]
				{
					'.'
				})[0];
                int num = RoomUser_0.GetClient().GetHabbo().Figure.IndexOf("wa-");
                if (num == -1)
                {
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure + text;
                }
                else
                {
                    string text2 = Regex.Split(RoomUser_0.GetClient().GetHabbo().Figure, "wa-")[1];
                    text2 = "wa-" + text2.Split(new char[]
					{
						'.'
					})[0];
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure.Replace(text2, text);
                }
            }
            if (string_0.Contains("fa-"))
            {
                string text = Regex.Split(string_0, "fa-")[1];
                text = "fa-" + text.Split(new char[]
				{
					'.'
				})[0];
                int num = RoomUser_0.GetClient().GetHabbo().Figure.IndexOf("fa-");
                if (num == -1)
                {
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure + text;
                }
                else
                {
                    string text2 = Regex.Split(RoomUser_0.GetClient().GetHabbo().Figure, "fa-")[1];
                    text2 = "fa-" + text2.Split(new char[]
					{
						'.'
					})[0];
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure.Replace(text2, text);
                }
            }
            if (string_0.Contains("cc-"))
            {
                string text = Regex.Split(string_0, "cc-")[1];
                text = "cc-" + text.Split(new char[]
				{
					'.'
				})[0];
                int num = RoomUser_0.GetClient().GetHabbo().Figure.IndexOf("cc-");
                if (num == -1)
                {
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure + text;
                }
                else
                {
                    string text2 = Regex.Split(RoomUser_0.GetClient().GetHabbo().Figure, "cc-")[1];
                    text2 = "cc-" + text2.Split(new char[]
					{
						'.'
					})[0];
                    RoomUser_0.GetClient().GetHabbo().Figure = RoomUser_0.GetClient().GetHabbo().Figure.Replace(text2, text);
                }
            }
        }
    }
}
