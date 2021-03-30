using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FileViewerSample
{
    /// <summary>
    /// Класс файлового менеджера
    /// </summary>
    public class FileTreeManager
    {
        /// <summary>
        /// Выбранный элемент
        /// </summary>
        public BaseViewItem Selected { get; private set; }
        /// <summary>
        /// Список элементов текущий папки
        /// </summary>
        public List<BaseViewItem> Items { get; private set; }

        /// <summary>
        /// Инициализация менеджера файлов с начальной папкой корня текущего диска
        /// </summary>
        public FileTreeManager()
            : this(Path.GetPathRoot(Directory.GetCurrentDirectory()))
        { }

        public FileTreeManager(string initPath)
        {
            if (!ChangePath(initPath))
                ChangePath(Path.GetPathRoot(Directory.GetCurrentDirectory()));
        }

        /// <summary>
        /// Изменение текущей папки
        /// </summary>
        /// <param name="path">Папку, в которую нужной перейти</param>
        /// <returns>true - если удалось перейти в папку</returns>
        public bool ChangePath(string path)
        {
            if (path == null)
            {
                FillDisk();
                First();
            }
            else if (Directory.Exists(path))
            {
                FillItems(path);
                First();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Выбор первого элемента текущего списка
        /// </summary>
        public void First()
        {
            Selected = Items[0];
        }

        /// <summary>
        /// Выбор последнего элемента текущего списка
        /// </summary>
        public void Last()
        {
            Selected = Items[Items.Count - 1];
        }

        /// <summary>
        /// Выбор предыдущего элемента
        /// </summary>
        public void Previous()
        {
            var index = Items.FindIndex(o => o.Equals(Selected));
            Selected = index <= 0 ? Items[0] : Items[index - 1];
        }

        /// <summary>
        /// Выбор следующего элемента
        /// </summary>
        public void Next()
        {
            var index = Items.FindIndex(o => o.Equals(Selected));
            Selected = index >= Items.Count - 1 ? Items[Items.Count - 1] : Items[index + 1];

        }

        /// <summary>
        /// Открыть текущий элемент (папку)
        /// </summary>
        /// <returns></returns>
        public bool SelectOpen()
        {
            if (Selected != null)
                return ChangePath(Selected.MainPath);
            else
                return false;
        }

        public void Home()
        {
            FillDisk();
            First();
        }

        /// <summary>
        /// удаление выбранной папки/файла
        /// </summary>
        /// <returns></returns>
        public bool Delete()
        {
            if (Selected != null && !Items.First().Equals(Selected))
            {
                try
                {
                    // Удаляем физические файлы/папки
                    if (Selected.Size.HasValue)
                        File.Delete(Selected.MainPath);
                    else
                        Directory.Delete(Selected.MainPath, true);
                    // Удаляем элемент из списка
                    Items.Remove(Selected);
                    // Выбираем другой элемент
                    Previous();
                    return true;
                }
                catch (Exception ex)
                {
                    // skip error
                }
            }
            return false;
        }

        /// <summary>
        /// Заполнение списка элементами выбранной папки
        /// </summary>
        /// <param name="path"></param>
        private void FillItems(string path)
        {
            List<BaseViewItem> list = new List<BaseViewItem>();

            try
            {
                // перебираем элементы текущий папки и наполняем список элементами
                foreach (var entry in Directory.GetFileSystemEntries(path))
                {
                    try
                    {
                        var item = new BaseViewItem()
                        {
                            MainPath = entry,
                            Name = Path.GetFileName(entry),
                            Size = File.Exists(entry) ? new FileInfo(entry).Length : (long?)null
                        };
                        list.Add(item);
                    }
                    catch
                    {
                        // skip error
                    }
                }
            }
            catch
            {
                // skip error
            }
            
            // сортировка по типу (папка/файл), а потом по имени
            Items = list.OrderBy(o => o.Size.HasValue).ThenBy(o => o.Name).ToList();

            // поиск предыдущий папки
            var itemParent = new BaseViewItem() { Name = "." };
            var parent = Directory.GetParent(path);
            if (parent != null && !parent.FullName.Equals(path))
                itemParent.MainPath = parent.FullName;
            Items.Insert(0, itemParent);
        }

        private void FillDisk()
        {
            List<BaseViewItem> list = new List<BaseViewItem>();
            try
            {
                // перебираем элементы текущий папки и наполняем список элементами
                foreach (var entry in Environment.GetLogicalDrives())
                {
                    try
                    {
                        var item = new BaseViewItem()
                        {
                            MainPath = entry,
                            Name = entry
                        };
                        list.Add(item);
                    }
                    catch
                    {
                        // skip error
                    }
                }
            }
            catch
            {
                // skip error
            }
            // сортировка по типу (папка/файл), а потом по имени
            Items = list.OrderBy(o => o.Name).ToList();
        }
    }
}
