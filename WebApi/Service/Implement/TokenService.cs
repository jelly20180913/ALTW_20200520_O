using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Models.CustomModel;
using Jose;
using System.Text;
using WebApi.Models.JWT;
using System.IO;
using Newtonsoft.Json;
using WebApi.Service.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement
{
    public class TokenService : ITokenService
    {
        private ILoginService _loginService;
        private Interface.Common.ICommonService _commonService;
        public TokenService(ILoginService loginService, Interface.Common.ICommonService commonService)
        {
            this._loginService = loginService;
            this._commonService = commonService;
        }
        // POST api/values
        /// <summary>
        /// p.s the expiration not work
        /// </summary>
        /// <param name="loginData"></param>
        /// <returns></returns>
        public object SetToken(LoginData loginData)
        {
            Token _Token = getToken(loginData.Origin);
            var secret = _Token.Secret; 
            // if ( loginData.Username ==_Token.Username && loginData.Password == _Token.Password ) 
            Login _Login = _loginService.GetByAccount(loginData.Username, loginData.Password);
            if (_Login!=null)
            {
                this._commonService.InsetButtonLog(_Login.Id, "Login","", "Login.html");
                //every day has diffrent token
                var payload = new JwtAuthObject()
                {
                    Id =  _Login.Id,
                    exp = DateTime.Now.Ticks,
                    iat = DateTime.Now.AddSeconds(10).Ticks
                };
                return new
                {
                    token = Jose.JWT.Encode(payload, Encoding.UTF8.GetBytes(secret), JwsAlgorithm.HS256),
                    loginId = _Login.Id,
                    indexPage = _Login.IndexPage
                };
            }
            else
            {
                throw new UnauthorizedAccessException("IDPASSWORDERROR:ID:"+ loginData.Username+" PWD:" + loginData.Password);
            }
        }
        private Token getToken(string orign)  
        {
            Token _Token = new Token();
            string _Json = getFileJson();
            List<Token> _TokenList = JsonConvert.DeserializeObject<List<Token>>(_Json);
            if (_TokenList.Where(x => x.Origin == orign).Count() > 0) _Token = _TokenList.Where(x => x.Origin == orign).First();
            else _Token = _TokenList.First();
            return _Token;
        }
        /// <summary>
        /// get json file 
        /// </summary>
        /// <returns></returns>
        private string getFileJson()
        {
            string json = string.Empty;
            using (FileStream fs = new FileStream(System.Web.Hosting.HostingEnvironment.MapPath("~/Setting.json"), FileMode.Open, System.IO.FileAccess.Read, FileShare.ReadWrite))
            {
                using (StreamReader sr = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                {
                    json = sr.ReadToEnd().ToString();
                }
            }
            return json;
        }
       
    }
}