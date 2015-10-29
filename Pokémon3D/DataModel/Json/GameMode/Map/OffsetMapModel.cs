﻿using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokémon3D.DataModel.Json.GameMode.Map
{
    /// <summary>
    /// A data model for an offset map.
    /// </summary>
    [DataContract]
    class OffsetMapModel : JsonDataModel
    {
        [DataMember(Order = 0)]
        public string MapFile;

        [DataMember(Order = 1)]
        public Vector3Model Offset;
    }
}
