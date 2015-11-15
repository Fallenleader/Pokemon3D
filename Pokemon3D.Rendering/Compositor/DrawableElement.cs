using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Pokemon3D.Rendering.Data;

namespace Pokemon3D.Rendering.Compositor
{
    interface DrawableElement
    {
        Mesh Mesh { get; }
        Material Material { get; }
    }
}
