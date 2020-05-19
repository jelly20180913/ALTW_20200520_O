namespace WebApi.Models.CustomModel.SAP
{
    public class Budget
    {
        public string MANDT { get; set; }
        public string KOKRS { get; set; }
        public string AUFNR { get; set; }
        public string WAERS { get; set; }
        public string START_AMT { get; set; }
        public string USED_AMT { get; set; }
        public string END_AMT { get; set; }
        public string COMMIT_AMT { get; set; }
        public string START_AMT_TOL { get; set; }
        public string USED_AMT_TOL { get; set; }
        public string END_AMT_TOL { get; set; }
        public string COMMIT_AMT_TOL { get; set; }
    }
}