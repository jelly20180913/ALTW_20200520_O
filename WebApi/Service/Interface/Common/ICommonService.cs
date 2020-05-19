namespace WebApi.Service.Interface.Common
{
    public interface ICommonService
    {
         void UpdatePassword(int loginId, string password);
        void InsetButtonLog(int loginId, string buttonName, string remark,string page);
    }
}
