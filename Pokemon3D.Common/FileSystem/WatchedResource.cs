using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Pokemon3D.Common.FileSystem
{
    abstract class WatchedResource
    {
        public string ResourcePath { get; protected set; }
        
        protected FileSystemWatcher _watcher;

        public WatchedResource(string resourcePath)
        {
            ResourcePath = resourcePath;
        }
    }
}
