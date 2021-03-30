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
            if (Directory.Exists(path))
            {
                FillItems(path);
                Selected = Items.FirstOrDefault();
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

        /// <summary>
        /// Заполнение списка элементами выбранной папки
        /// </summary>
        /// <param name="path"></param>
        private void FillItems(string path)
        {
            List<BaseViewItem> list = new List<BaseViewItem>();

            // поиск предыдущий папки
            var parent = Directory.GetParent(path);
            if (parent != null && !parent.FullName.Equals(path))
            {
                var item = new BaseViewItem()
                {
                    MainPath = parent.FullName,
                    Name = "."
                };
                list.Add(item);
            }

            try
            {
                // перебираем элементы текйщий папки и наполняем список элементами
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

            Items = list.OrderBy(o => o.Size.HasValue).ThenBy(o => o.Name).ToList();
        }
    }
}
