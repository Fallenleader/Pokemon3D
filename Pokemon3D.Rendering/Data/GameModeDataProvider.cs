namespace Pokemon3D.Rendering.Data
{
    public interface GameModeDataProvider
    {
        GeometryData GetPrimitiveData(string primitiveName);

        string TexturePath { get; }
    }
}