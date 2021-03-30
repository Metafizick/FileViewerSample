using System;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace FileViewerSample
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.CursorVisible = false;
            var manager = new FileTreeManager();
            ShowList(manager.Items, manager.Selected);
            while (true)
            {
                var inputKey = Console.ReadKey(false);
                switch (inputKey.Key)
                {
                    case ConsoleKey.PageUp:
                        manager.First();
                        break;
                    case ConsoleKey.PageDown:
                        manager.Last();
                        break;
                    case ConsoleKey.UpArrow:
                        manager.Previous();
                        break;
                    case ConsoleKey.DownArrow:
                        manager.Next();
                        break;
                    case ConsoleKey.Enter:
                        manager.SelectOpen();
                        break;
                }
                ShowList(manager.Items, manager.Selected);
            }
        }


        static void ShowList(IEnumerable<BaseViewItem> items, BaseViewItem selected = null)
        {
            Console.Clear();
            foreach (var item in items)
            {
                if (item.Equals(selected))
                {
                    Console.BackgroundColor = ConsoleColor.White;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.ForegroundColor = ConsoleColor.White;
                }
                PrintItem(item);
                Console.ResetColor();
            }
        }

        static void PrintItem(BaseViewItem item)
        {
            var name = item.Name.Length <= 50 ? item.Name : $"{item.Name.Substring(0, 47)}...";
            var itemType = item.Size.HasValue ? string.Empty : "dir";
            var size = item.Size.HasValue ? BytesSizeForamt(item.Size.Value) : string.Empty;
            Console.WriteLine($"{name,-50} {itemType,3} {size,15}");
        }

        static string BytesSizeForamt(long size)
        {
            string[] suffixes = { "B", "KB", "MB", "TB" };
            string suffix = suffixes[0];

            for (int i = 0; i < 4; i++)
            {
                suffix = suffixes[i];
                if (size > 1024)
                    size /= 1024;
                else
                    break;
            }
            return $"{size:N1} {suffix}";
        }
    }
}
