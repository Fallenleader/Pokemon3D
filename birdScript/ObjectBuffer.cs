using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace birdScript
{
    /// <summary>
    /// The object buffer assigns Ids to object instances and stores them so they can be retrieved by their Ids.
    /// </summary>
    internal static class ObjectBuffer
    {
        private static object _syncRoot = new object();

        private static List<object> _buffer = new List<object>();

        internal static int GetObjectId(object obj)
        {
            int objId;

            lock (_syncRoot)
            {
                if (!_buffer.Contains(obj))
                    _buffer.Add(obj);

                objId = _buffer.IndexOf(obj);
            }

            return objId;
        }

        internal static bool HasObject(int id)
        {
            bool result;

            lock (_syncRoot)
            {
                result = _buffer.Count > id;
            }

            return result;
        }

        internal static object GetObject(int id)
        {
            object result;

            lock (_syncRoot)
            {
                result = _buffer[id];
            }

            return result;
        }
    }
}
