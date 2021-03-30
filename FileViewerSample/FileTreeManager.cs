using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace FileViewerSample
{
    public class FileTreeManager
    {
        public BaseViewItem Selected { get; private set; }
        public List<BaseViewItem> Items { get; private set; }

        public FileTreeManager()
            : this(Path.GetPathRoot(Directory.GetCurrentDirectory()))
        { }

        public FileTreeManager(string initPath)
        {
            if (!ChangePath(initPath))
                ChangePath(Path.GetPathRoot(Directory.GetCurrentDirectory()));
        }

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

        public void First()
        {
            Selected = Items[0];
        }

        public void Last()
        {
            Selected = Items[Items.Count - 1];
        }

        public void Previous()
        {
            var index = Items.FindIndex(o => o.Equals(Selected));
            Selected = index <= 0 ? Items[0] : Items[index - 1];
        }

        public void Next()
        {
            var index = Items.FindIndex(o => o.Equals(Selected));
            Selected = index >= Items.Count - 1 ? Items[Items.Count - 1] : Items[index + 1];

        }

        public bool SelectOpen()
        {
            if (Selected != null)
                return ChangePath(Selected.MainPath);
            else
                return false;
        }

        private void FillItems(string path)
        {
            List<BaseViewItem> list = new List<BaseViewItem>();

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

            Items = list.OrderBy(o => o.Size.HasValue).ThenBy(o => o.Name).ToList();
        }
    }
}
