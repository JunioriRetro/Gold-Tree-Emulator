using System;
using System.IO;
using System.Net;
using System.Text;

namespace GoldTree.Core
{
	public sealed class Logging
	{
		private static bool IsDisabled = false;

		internal static void Write(string str)
		{
			if (!Logging.IsDisabled)
				Console.Write(str);
		}

        internal static void Write(string str, ConsoleColor color)
        {
            if (Logging.IsDisabled)
                return;

            Console.ForegroundColor = color;
            Console.Write(str);
            Console.ResetColor();
        }

		internal static void WriteLine(string str)
		{
            if (!Logging.IsDisabled)
				Console.WriteLine(str);
		}

        internal static void WriteLine(string str, ConsoleColor color)
        {
            if (Logging.IsDisabled)
                return;

            Console.ForegroundColor = color;
            Console.WriteLine(str);
            Console.ResetColor();
        }

		internal static void LogException(string logText)
		{
			try
			{
				FileStream fileStream = new FileStream("exceptions.err", FileMode.Append, FileAccess.Write);
				byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
				{
					DateTime.Now,
					": ",
					logText,
					"\r\n\r\n"
				}));
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Close();
                /*if (!logText.Contains("Unknown baseID"))
                {
                    try
                    {
                        if (int.Parse(GoldTree.GetConfig().data["automatic-error-report"]) == 1)
                        {
                            WebClient client = new WebClient();
                            Stream stream = client.OpenRead("AUTOMATIC ERROR REPORTING DISABLED");
                            StreamReader reader = new StreamReader(stream);
                            String content = reader.ReadLine();
                            Console.ForegroundColor = ConsoleColor.Green;
                            Logging.WriteLine(content);
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                    }
                    catch
                    {
                    }
                }*/
			}
			catch (Exception)
			{
				Logging.WriteLine(DateTime.Now + ": " + logText);
			}

			Console.ForegroundColor = ConsoleColor.Red;
			Logging.WriteLine("Exception has been saved");
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		internal static void LogCriticalException(string logText)
		{
			try
			{
				FileStream fileStream = new FileStream("criticalexceptions.err", FileMode.Append, FileAccess.Write);
				byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
				{
					DateTime.Now,
					": ",
					logText,
					"\r\n\r\n"
				}));
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Close();
                /*try
                {
                    if (int.Parse(GoldTree.GetConfig().data["automatic-error-report"]) == 1)
                    {
                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead("AUTOMATIC ERROR REPORTING DISABLED");
                        StreamReader reader = new StreamReader(stream);
                        String content = reader.ReadLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Logging.WriteLine(content);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                catch
                {
                }*/
				Console.ForegroundColor = ConsoleColor.Red;
				Logging.WriteLine("CRITICAL ERROR LOGGED");
				Console.ForegroundColor = ConsoleColor.Gray;
			}
			catch (Exception)
			{
				Logging.WriteLine(DateTime.Now + ": " + logText);
			}
		}

		internal static void LogCacheError(string logText)
		{
			try
			{
				FileStream fileStream = new FileStream("cacheerror.err", FileMode.Append, FileAccess.Write);
				byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
				{
					DateTime.Now,
					": ",
					logText,
					"\r\n\r\n"
				}));
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Close();
                /*try
                {
                    if (int.Parse(GoldTree.GetConfig().data["automatic-error-report"]) == 1)
                    {
                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead("AUTOMATIC ERROR REPORTING DISABLED");
                        StreamReader reader = new StreamReader(stream);
                        String content = reader.ReadLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Logging.WriteLine(content);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                catch
                {
                }*/
			}
			catch (Exception)
			{
				Logging.WriteLine(DateTime.Now + ": " + logText);
			}
			Console.ForegroundColor = ConsoleColor.Red;
			Logging.WriteLine("Critical error saved");
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		internal static void LogDDoS(string logText)
		{
            try
            {
                FileStream fileStream = new FileStream("ddos.txt", FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
				{
					DateTime.Now,
					": ",
					logText,
					"\r\n\r\n"
				}));
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
            catch { }

			Logging.WriteLine(DateTime.Now + ": " + logText);
		}

		internal static void LogThreadException(string Exception, string Threadname)
		{
			try
			{
				FileStream fileStream = new FileStream("threaderror.err", FileMode.Append, FileAccess.Write);
				byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
				{
					DateTime.Now,
					": Error in thread ",
					Threadname,
					": \r\n",
					Exception,
					"\r\n\r\n"
				}));
				fileStream.Write(bytes, 0, bytes.Length);
				fileStream.Close();
                /*try
                {
                    if (int.Parse(GoldTree.GetConfig().data["automatic-error-report"]) == 1)
                    {
                        WebClient client = new WebClient();
                        Stream stream = client.OpenRead("AUTOMATIC ERROR REPORTING DISABLED");
                        StreamReader reader = new StreamReader(stream);
                        String content = reader.ReadLine();
                        Console.ForegroundColor = ConsoleColor.Green;
                        Logging.WriteLine(content);
                        Console.ForegroundColor = ConsoleColor.Gray;
                    }
                }
                catch
                {
                }*/
				Console.ForegroundColor = ConsoleColor.Red;
				Logging.WriteLine("Error in " + Threadname + " caught");
				Console.ForegroundColor = ConsoleColor.Gray;
			}
			catch (Exception)
			{
				Logging.WriteLine(DateTime.Now + ": " + Exception);
			}
		}

		internal static void Disable()
		{
			Logging.IsDisabled = true;
		}

		internal static void HandleException(string logText)
		{
            Logging.WriteLine("[EXCEPTION] " + logText, ConsoleColor.Yellow);
		}

        internal static void LogItemError(string logText)
        {
            try
            {
                FileStream fileStream = new FileStream("itemexceptions.err", FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
				{
					DateTime.Now,
					": ",
					logText,
					"\r\n\r\n"
				}));
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
                Console.ForegroundColor = ConsoleColor.Red;
                Logging.WriteLine("Item error saved");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (Exception)
            {
                Logging.WriteLine(DateTime.Now + ": " + logText);
            }
        }

        internal static void LogItemUpdateError(string logText)
        {
            try
            {
                FileStream fileStream = new FileStream("itemupdatexceptions.err", FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
				{
					DateTime.Now,
					": ",
					logText,
					"\r\n\r\n"
				}));
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
                Console.ForegroundColor = ConsoleColor.Red;
                Logging.WriteLine("Item update error saved");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (Exception)
            {
                Logging.WriteLine(DateTime.Now + ": " + logText);
            }
        }

        internal static void LogSocketError(string logText)
        {
            try
            {
                FileStream fileStream = new FileStream("socket.err", FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
				{
					DateTime.Now,
					": ",
					logText,
					"\r\n\r\n"
				}));
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }
            catch
            {
            }
            Logging.WriteLine(DateTime.Now + ": " + logText);
        }

        internal static void LogRoomError(string logText)
        {
            try
            {
                FileStream fileStream = new FileStream("roomexceptions.err", FileMode.Append, FileAccess.Write);
                byte[] bytes = Encoding.ASCII.GetBytes(string.Concat(new object[]
				{
					DateTime.Now,
					": ",
					logText,
					"\r\n\r\n"
				}));
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
                Console.ForegroundColor = ConsoleColor.Red;
                Logging.WriteLine("Room error saved");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (Exception)
            {
                Logging.WriteLine(DateTime.Now + ": " + logText);
            }
        }
	}
}
