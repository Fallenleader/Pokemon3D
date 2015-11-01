namespace Pokemon3D.Rendering.Data
{
    public interface PrimitiveDataProvider
    {
        GeometryData GetPrimitiveData(string primitiveName);
    }
}