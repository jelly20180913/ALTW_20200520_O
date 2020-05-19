namespace WebApi.Service.Interface.Common
{
    public interface ICommonFileService
    {
        string  Upload();
        string SaveToSuccess(int loginId, string sourceFile, string tableName);
        string GetQuarter(int month);
        string GetCountryEngName(string countryCode);
        string InsertUploadLog(int loginID, string fileSavePath, string tableName, bool success, string serverFileName);
        int GetExcelMinusNumber(string input_number);
    }
}
