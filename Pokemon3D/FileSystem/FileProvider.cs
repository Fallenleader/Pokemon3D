using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pokemon3D.FileSystem
{
    /// <summary>
    /// The base class for file providers.
    /// </summary>
    abstract class FileProvider
    {
        /// <summary>
        /// Returns the start up path of the executable.
        /// </summary>
        protected static string StartupPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }
    }
}