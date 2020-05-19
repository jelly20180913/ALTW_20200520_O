using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using SAP.Middleware.Connector;

namespace WebApi.ThirdParty.SAP
{
    public class SapDestinationConfig : IDestinationConfiguration
    {
        /// <summary>
        /// when WebAPI server start up will initial  RFC config setting by web.config 
        /// accoding to server ip to change QAS or PRD
        /// </summary>
        /// <param name="destinationName"></param>
        /// <returns></returns>
        public RfcConfigParameters GetParameters(String destinationName)
        {
            string _ParameterValue = ConfigurationManager.AppSettings[destinationName];
            string[] _ArrayParameterValue = _ParameterValue.Split(';');
            string _NAME = _ArrayParameterValue[0].Split('=')[1];
            string _SAP_USERNAME = _ArrayParameterValue[1].Split('=')[1];
            string _SAP_PASSWORD = _ArrayParameterValue[2].Split('=')[1];
            string _SAP_CLIENT = _ArrayParameterValue[3].Split('=')[1];
            string _SAP_APPSERVERHOST = _ArrayParameterValue[4].Split('=')[1];
            string _SAP_SYSTEMNUM = _ArrayParameterValue[5].Split('=')[1];
            string _SAP_LANGUAGE = _ArrayParameterValue[6].Split('=')[1];
            string _SAP_POOLSIZE = _ArrayParameterValue[7].Split('=')[1];
            RfcConfigParameters parms = new RfcConfigParameters();
            parms.Add(RfcConfigParameters.Name, _NAME);
            parms.Add(RfcConfigParameters.AppServerHost, _SAP_APPSERVERHOST);
            parms.Add(RfcConfigParameters.SystemNumber, _SAP_SYSTEMNUM);
            parms.Add(RfcConfigParameters.SystemID, _SAP_CLIENT);
            parms.Add(RfcConfigParameters.User, _SAP_USERNAME);
            parms.Add(RfcConfigParameters.Password, _SAP_PASSWORD);
            parms.Add(RfcConfigParameters.Client, _SAP_CLIENT);
            parms.Add(RfcConfigParameters.Language, _SAP_LANGUAGE);
            parms.Add(RfcConfigParameters.PoolSize, _SAP_POOLSIZE);
            return parms;
        }
        public bool ChangeEventsSupported()
        {
            return false;
        }
        public event RfcDestinationManager.ConfigurationChangeHandler ConfigurationChanged;
    }
}