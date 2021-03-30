using System;
using System.Collections.Generic;
using System.Text;

namespace FileViewerSample
{
    public class BaseViewItem
    {
        /// <summary>
        /// Полный путь к элементу
        /// </summary>
        public string MainPath { get; set; }
        /// <summary>
        /// Имя элемента
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Размер
        /// </summary>
        public long? Size { get; set; } = null;
    }
}
