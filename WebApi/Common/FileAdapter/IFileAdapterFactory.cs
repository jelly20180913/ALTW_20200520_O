namespace WebApi.Common.FileAdapter
{
    public  interface IFileAdapterFactory
    {
          FileBase CreateFileAdapter(string ext);
    }
}
