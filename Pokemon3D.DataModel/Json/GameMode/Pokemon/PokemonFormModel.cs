﻿using System.Runtime.Serialization;

// Disable Code Analysis for warning CS0649: Field is never assigned to, and will always have its default value.
#pragma warning disable 0649

namespace Pokemon3D.DataModel.Json.GameMode.Pokemon
{
    [DataContract]
    public class PokemonFormModel : JsonDataModel<PokemonFormModel>
    {
        [DataMember(Order = 0)]
        public string FormName;

        [DataMember(Order = 1)]
        public PokemonFormActivationModel Activation;

        [DataMember(Order = 2)]
        public PokemonStatSetModel BaseStats;

        [DataMember(Order = 3)]
        public TextureSourceModel MenuTexture;

        [DataMember(Order = 4)]
        public TextureSourceModel SpriteTexture;

        [DataMember(Order = 5)]
        public TextureSourceModel OverworldTexture;

        [DataMember(Order = 6)]
        public string Type1;

        [DataMember(Order = 7)]
        public string Type2;

        [DataMember(Order = 8)]
        public int CatchRate;

        [DataMember(Order = 9)]
        public int[] Abilities;

        [DataMember(Order = 10)]
        public LevelUpMoveModel[] LevelMoves;

        [DataMember(Order = 11)]
        public int[] MachineMoves;

        [DataMember(Order = 12)]
        public int[] EggMoves;

        [DataMember(Order = 13)]
        public int[] TutorMoves;

        public override object Clone()
        {
            var clone = (PokemonFormModel)MemberwiseClone();
            clone.Activation = Activation.CloneModel();
            clone.BaseStats = BaseStats.CloneModel();
            clone.MenuTexture = MenuTexture.CloneModel();
            clone.SpriteTexture = SpriteTexture.CloneModel();
            clone.OverworldTexture = OverworldTexture.CloneModel();
            clone.Abilities = (int[])Abilities.Clone();
            clone.LevelMoves = (LevelUpMoveModel[])LevelMoves.Clone();
            clone.MachineMoves = (int[])MachineMoves.Clone();
            clone.EggMoves = (int[])EggMoves.Clone();
            clone.TutorMoves = (int[])TutorMoves.Clone();
            return clone;
        }
    }
}
