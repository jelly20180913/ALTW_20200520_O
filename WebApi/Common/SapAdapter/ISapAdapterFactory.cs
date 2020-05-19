namespace WebApi.Common.SapAdapter
{
    public  interface ISapAdapterFactory
    {
        SapBase CreateSapAdapter(string type);
    }
}
