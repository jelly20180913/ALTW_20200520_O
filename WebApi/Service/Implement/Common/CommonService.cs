using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Service.Interface.Common;
using WebApi.Models;
using WebApi.Service.Interface;
namespace WebApi.Service.Implement.Common
{
    public class CommonService : ICommonService
    {
        private ILoginService _loginService;
        private IButtonLogService _buttonLogService;
        public CommonService(ILoginService loginService, IButtonLogService buttonLogService)
        {
            this._loginService = loginService;
            this._buttonLogService = buttonLogService;
        }
        public void UpdatePassword(int loginId, string password)
        {
            Login _Login = this._loginService.GetByID(loginId);
            _Login.Password = password;
            this._loginService.Update(_Login);
        }
        /// <summary>
        /// user trace
        /// </summary>
        /// <param name="loginId"></param>
        /// <param name="buttonName"></param>
        /// <param name="remark">filter information</param>
        /// <param name="page"></param>
        public void InsetButtonLog(int loginId,string buttonName,string remark,string page)
        {
            ButtonLog _ButtonLog = new ButtonLog();
            _ButtonLog.FK_LoginId = loginId;
            _ButtonLog.Button = buttonName;
            _ButtonLog.ClickTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
            _ButtonLog.Remark = remark;
            _ButtonLog.Page = page;
            _buttonLogService.Create(_ButtonLog);
        }
    }
}