using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Pokémon3D.GameModes.Maps.EntityComponents
{
    /// <summary>
    /// Contains static methods to convert the string data of entity components to other data types.
    /// </summary>
    static class TypeConverter
    {

        public static T Convert<T>(string data)
        {
            switch (typeof(T).Name)
            {
                case "Int32":
                    return (T)ToInt32(data);
                case "Vector3":
                    return (T)ToVector3(data);
                case "Vector2":
                    return (T)ToVector2(data);
                case "Single":

                case "Int32[]":
                    return (T)ToArray<int>(data);
                default:
                    return default(T);
            }
        }

        private static object ToArray<T>(string data)
        {
            return data.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(x => Convert<string>(x)).ToArray();
        }

        private static object ToVector3(string data)
        {
            string[] dataParts = data.Split(',');

            if (dataParts.Length == 3)
            {
                float xResult, yResult, zResult;

                if (float.TryParse(dataParts[0], out xResult))
                    if (float.TryParse(dataParts[1], out yResult))
                        if (float.TryParse(dataParts[2], out zResult))
                            return new Vector3(xResult, yResult, zResult);

            }

            return Vector3.Zero;
        }

        private static object ToVector2(string data)
        {
            string[] dataParts = data.Split(',');

            if (dataParts.Length == 2)
            {
                float xResult, yResult;

                if (float.TryParse(dataParts[0], out xResult))
                    if (float.TryParse(dataParts[1], out yResult))
                        return new Vector2(xResult, yResult);

            }

            return Vector2.Zero;
        }

        private static object ToSingle(string data)
        {
            float result = 0;
            float.TryParse(data, out result);
            return result;
        }

        private static object ToInt32(string data)
        {
            int result = 0;
            int.TryParse(data, out result);
            return result;
        }

    }
}
