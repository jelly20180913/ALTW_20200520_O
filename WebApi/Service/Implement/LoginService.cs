using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Service.Interface;
using WebApi.Models.Repository.EDI.Interface;
using WebApi.Models;
namespace WebApi.Service.Implement
{
    public class LoginService:ILoginService
    {
        private IRepository<Login> _repository;
        public LoginService(IRepository<Login> repository)
        {
            this._repository = repository;
        }
        public void Create(Login instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            this._repository.Create(instance);
        }

        public void Update(Login instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException();
            }
            this._repository.Update(instance);
        }

        public void Delete(int Id)
        {
            var instance = this.GetByID(Id);
            this._repository.Delete(instance);
        }

        public bool IsExists(int Id)
        {
            return this._repository.GetAll().Any(x => x.Id == Id);
        }

        public Login GetByID(int Id)
        {
            return this._repository.Get(x => x.Id == Id);
        }

        public IEnumerable<Login> GetAll()
        {
            return this._repository.GetAll();
        }
        /// <summary>
        /// Verify this account 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool VerifyAccount(string  account,string password)
        {
            return this._repository.GetAll().Any(x => x.Account == account&&x.Password==password);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Login GetByAccount(string account, string password)
        {
            return this._repository.Get(x => x.Account == account && x.Password == password&&x.Approve==true);
        }
    }
}