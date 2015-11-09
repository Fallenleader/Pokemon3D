namespace Pokemon3D.DataModel.Json.GameMode.Definitions.World
{
    /// <summary>
    /// The different orientations and visual representations of routes on the map.
    /// </summary>
    public enum RouteDirection
    {
        Horizontal,
        Vertical,

        HorizontalEndRight,
        HorizontalEndLeft,

        VerticalEndUp,
        VerticalEndDown,

        CurveDownRight,
        CurveDownLeft,
        CurveUpLeft,
        CurveUpRight,

        TUp,
        TDown,
        TRight,
        TLeft,

        HorizontalConnection,
        VerticalConnection
    }
}
