using System;
using System.Collections.Generic;
using System.Text;

namespace FileViewerSample
{
    /// <summary>
    /// Элемент отображэения файлового мендежера
    /// </summary>
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
