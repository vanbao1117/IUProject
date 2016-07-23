using IU.Web.Models;
using IU.Domain;
using IU.Services.Repositories;
using IU.Services.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IU.Web;

namespace IU.Services
{
    public class RefreshTokenService : IDisposable
    {
        private IRepository<RefreshToken> RefreshTokenRepository;
        public RefreshTokenService()
        {
            RefreshTokenRepository = new Repository<RefreshToken>();
        }

        public async Task<bool> Delete(string userName, string clientId)
        {
            try
            {
               
                var client = await RefreshTokenRepository.FindAllByAsync(t=>t.Subject == userName && t.ClientId == clientId);
                if (client != null)
                {
                    foreach(RefreshToken _refreshToken in client.ToArray() ){
                        await RefreshTokenRepository.DeleteAsync(_refreshToken);
                    }
                    
                    return true;
                }
                    
            }
            catch (Exception ex)
            {
            }
            return false;
        }

        public async Task<RefreshTokenViewModel> Create(RefreshTokenViewModel model)
        {
            try
            {
                await RefreshTokenRepository.SaveAsync(new RefreshToken() { Id = model.Id, ClientId = model.ClientId, Subject = model.Subject, ExpiresUtc = model.ExpiresUtc, IssuedUtc = model.IssuedUtc, ProtectedTicket = model.ProtectedTicket });
                
                return model;
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        public async Task<RefreshTokenViewModel> GetList(string userName, string clientId)
        {
            try
            {
                var client = await RefreshTokenRepository.FindOneByAsync(t=>t.Subject == userName && t.ClientId == clientId);
                if (client != null)
                {
                    return new RefreshTokenViewModel(){Id = client.Id, ClientId = client.ClientId, Subject = client.Subject, ExpiresUtc = client.ExpiresUtc, IssuedUtc = client.IssuedUtc, ProtectedTicket = client.ProtectedTicket};
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

         public async Task<RefreshTokenViewModel> FindRefreshToken(string hashedTokenId)
        {
            try
            {
                var client = await RefreshTokenRepository.FindOneByAsync(t=>t.Id == hashedTokenId);
                if (client != null)
                {
                    return new RefreshTokenViewModel(){Id = client.Id, ClientId = client.ClientId, Subject = client.Subject, ExpiresUtc = client.ExpiresUtc, IssuedUtc = client.IssuedUtc, ProtectedTicket = client.ProtectedTicket};
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        public async Task<RefreshTokenViewModel> RemoveRefreshToken(string hashedTokenId)
        {
            try
            {
                var client = await RefreshTokenRepository.FindOneByAsync(t=>t.Id == hashedTokenId);
                if (client != null)
                {
                    await RefreshTokenRepository.DeleteAsync(client);
                    return new RefreshTokenViewModel(){Id = client.Id, ClientId = client.ClientId, Subject = client.Subject, ExpiresUtc = client.ExpiresUtc, IssuedUtc = client.IssuedUtc, ProtectedTicket = client.ProtectedTicket};
                }
            }
            catch (Exception ex)
            {
            }
            return null;
        }

        #region Dispose
        ~RefreshTokenService()
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
                RefreshTokenRepository.Dispose();
                RefreshTokenRepository = null;
            }
            // dispose the unmanaged objects
        }

        #endregion
    }
}
