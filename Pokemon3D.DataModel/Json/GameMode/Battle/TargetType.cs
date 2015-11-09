namespace Pokemon3D.DataModel.Json.GameMode.Battle
{
    /// <summary>
    /// Which Pokémon a move can target.
    /// </summary>
    public enum TargetType
    {
        OneAdjacentTarget, // One adjacent target, excluding itself.
        OneAdjacentFoe, // One adjacent foe.
        OneAdjacentAlly, // One adjacent ally, excluding itself.

        OneTarget, // One target, excluding itself.
        OneFoe, // One Foe.
        OneAlly, // One ally, excluding itself.

        Self, // Only self

        AllAdjacentTargets, // All adjacent targets, exluding itself
        AllAdjacentFoes, // All adjacent foes
        AllAdjacentAllies, // All adjacent allies, excluding itself.

        AllTargets, // All Targets, excluding itself.
        AllFoes, // All Foes
        AllAllies, // All allies, excluding itself.
        AllOwn, // All own Pokémon (allies + itself)

        All  // All Pokémon, including itself
    }
}
