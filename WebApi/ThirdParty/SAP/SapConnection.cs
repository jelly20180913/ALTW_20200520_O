using System;

namespace WebApi.ThirdParty.SAP
{
    //sap com singleto patten has trouble when you second call
    public class SapConnection
    {
        public bool SapLogin
        {
            get
            {
                return conn.Logon(0, true);
            }
        }
        private static SapConnection instance;
        /// <summary>
        /// p.s can abstract to setting.js
        /// </summary>
        private SapConnection()
        {
            login = new SAPLogonCtrl.SAPLogonControlClass();
            //login.ApplicationServer = "10.0.0.79";
            //login.Client = "800";
            //login.Language = "ZF";
            //login.User = "L180067";
            //login.Password = "2819Jelly";
            login.ApplicationServer = "10.0.0.80";
            login.Client = "250";
            login.Language = "ZF";
            login.User = "IT001";
            login.Password = "654321";
            login.SystemNumber = 00;
        }
        public static SapConnection GetInstance
        {
            get
            {
                //if (instance == null)
               // {
                    instance = new SapConnection();
                    conn = (SAPLogonCtrl.Connection)login.NewConnection();
                    if(!conn.Logon(0, true)) throw new Exception("登入SAP失敗");
              //  }
                return instance;
            }
        }
        private static SAPLogonCtrl.SAPLogonControlClass login;
        private static SAPLogonCtrl.Connection conn;
        public SAPLogonCtrl.Connection Conn
        {
            get { return conn; }
        }
        private static SAPFunctionsOCX.SAPFunctionsClass instance_SAPFunctionsClass;
        public static SAPFunctionsOCX.SAPFunctionsClass GetInstance_SAPFunctionsClass
        {
            get
            {
                //if (instance_SAPFunctionsClass == null)
                //{ 
                    instance_SAPFunctionsClass = new SAPFunctionsOCX.SAPFunctionsClass();
                    instance_SAPFunctionsClass.Connection = conn;
               // }
                return instance_SAPFunctionsClass;
            }
        }

        public void ConnLogoff()
        {
            conn.Logoff();
        }
    }
}