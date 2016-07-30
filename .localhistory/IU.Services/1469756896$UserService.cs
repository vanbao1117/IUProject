using IU.Domain;
using IU.Services.Repositories;
using IU.Services.Utilities;
using IU.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IU.Services
{
    public class UserService : IDisposable
    {
        private IRepository<AspNetUser> AccountRepository;
        private IRepository<Client> ClientRepository;

        public UserService()
        {
            AccountRepository = new Repository<AspNetUser>();
            ClientRepository = new Repository<Client>();
        }

        public IRepository<AspNetUser> GetUserRespoitory()
        {
            return AccountRepository;
        }

        public AspNetUserViewModel GetUserInfo(string username)
        {
            var user = AccountRepository.FindOneBy(u => u.UserName == username);
            if (user == null)
            {
                return null;
            }

            var userModelView = new AspNetUserViewModel()
            {
                Id = user.Id,
                AccessFailedCount = user.AccessFailedCount,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEndDateUtc = user.LockoutEndDateUtc,
                PasswordHash = user.PasswordHash,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                SecurityStamp = user.SecurityStamp,
                TwoFactorEnabled = user.TwoFactorEnabled,
                UserName = user.UserName
            };


            using (var context = new IUContext())
            {
                //Check if user is student
                var student = context.StudentTBLs.Where(s => s.UserID == user.Id).FirstOrDefault();
                if (student != null)
                {
                    userModelView.AccountType = "STUDENT";
                    return userModelView;
                }
                //Check if user is Lecturer
                var lecturer = context.LecturerTBLs.Where(s => s.UserID == user.Id).FirstOrDefault();
                if (lecturer != null)
                {
                    userModelView.AccountType = "LECTURER";
                    userModelView.FullName = lecturer.LecturerName;
                   
                    return userModelView;
                }

                //Check if user is ADMIN
                var admin = context.AdminTBLs.Where(s => s.UserID == user.Id).FirstOrDefault();
                if (admin != null)
                {
                    userModelView.AccountType = "Admin";
                    return userModelView;
                }
            }


            return null;

        }

        private AspNetUserViewModel FindUser(string username, string password)
        {
            string passwordHash = Helper.GetHash(password);
            var user = AccountRepository.FindOneBy(u => u.UserName == username && u.PasswordHash == passwordHash);
            if (user==null)
            {
                return null;
            }
            return new AspNetUserViewModel() { Id = user.Id, AccessFailedCount = user.AccessFailedCount, 
                Email = user.Email, EmailConfirmed = user.EmailConfirmed, LockoutEnabled = user.LockoutEnabled, 
                LockoutEndDateUtc = user.LockoutEndDateUtc, PasswordHash = user.PasswordHash, PhoneNumber = user.PhoneNumber, 
                PhoneNumberConfirmed = user.PhoneNumberConfirmed, SecurityStamp = user.SecurityStamp, TwoFactorEnabled = user.TwoFactorEnabled, UserName = user.UserName };
        }

        private AspNetUserViewModel FindUser(string username)
        {
            var user = AccountRepository.FindOneBy(u => u.UserName == username);
            if (user == null)
            {
                return null;
            }
            return new AspNetUserViewModel()
            {
                Id = user.Id,
                AccessFailedCount = user.AccessFailedCount,
                Email = user.Email,
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnabled = user.LockoutEnabled,
                LockoutEndDateUtc = user.LockoutEndDateUtc,
                PasswordHash = user.PasswordHash,
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                SecurityStamp = user.SecurityStamp,
                TwoFactorEnabled = user.TwoFactorEnabled,
                UserName = user.UserName
            };
        }

        public async Task<AspNetUserViewModel> FindUserSync(string username, string password)
        {
            return await Task.Run(() => FindUser(username, password));
        }

        public async Task<AspNetUserViewModel> FindUserSync(string username)
        {
            return await Task.Run(() => FindUser(username));
        }

        public async Task<string> checkEmailSync(string username)
        {
            var checkuser =  AccountRepository.FindOneBy(u => u.UserName == username);
            if (checkuser==null)
            {
                return null;
            }
            return checkuser.UserName;
        }

        public async Task<bool> InsertAsync(AspNetUser user)
        {
            try
            {
                await AccountRepository.SaveAsync(user);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Insert(AspNetUser user)
        {
            try
            {
                AccountRepository.Save(user);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public ClientViewModel FindClient(string clientId)
        {
            var client = ClientRepository.FindOneBy(c => c.Id == clientId);
            return new ClientViewModel() { Id = client.Id, Active = client.Active, AllowedOrigin = client.AllowedOrigin, ApplicationType = client.ApplicationType, Name = client.Name, RefreshTokenLifeTime = client.RefreshTokenLifeTime, Secret = client.Secret };
        }
        

        public bool CheckExistUser(LoginViewModel _viewmModel)
        {
            string passwordHash = Helper.GetHash(_viewmModel.Password);
            var user = AccountRepository.FindAllBy(a => a.UserName == _viewmModel.UserName && a.PasswordHash == passwordHash);
            if (user != null)
                return user.Any();
            else
                return false;
        }


        #region Dispose
        ~UserService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            // This method will remove current object from garbage collector's queue 
            // and stop calling finilize method twice 
        }    

        public void Dispose(bool disposer)
        {
            if (disposer)
            {
                // dispose the managed objects
                AccountRepository.Dispose();
                AccountRepository = null;
            }
            // dispose the unmanaged objects
        }

        #endregion
    }
}
