using Domain.Interfaces.Repositories;
using Domain.Models;
using Infra.Data;
using Infra.Repositories.DB;
using System.Xml.Linq;

namespace Application.Services
{
    public class PasswordApplication
    {
        public static string key = "b14ca5898a4e4133bbce2ea2315a1916";
        private readonly PasswordRepository _repository;

        public PasswordApplication()
        {
        }

        public PasswordApplication(string connectionString)
        {
            _repository = new PasswordRepository(connectionString);
        }

        public int AddPassword(string password, string name, int typeId)
        {
            try
            {
                if (string.IsNullOrEmpty(password))
                    throw new NullReferenceException(nameof(password));

                var existPasswordName = GetByName(name);

                if (existPasswordName != null)
                    throw new Exception("Senha já existe");

                var id = _repository.AddReturnId(new PasswordModel
                {
                    Name = name,
                    Password = EncryptService.Encrypt(key, password),
                    TypeId = typeId
                });

                return id;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}\n\nStack Trace: {ex.StackTrace}\n\nInner Exception: {ex.InnerException}");
                throw;
            }
        }

        public string DecryptPassword(int typeId, int passwordId)
        {
            try
            {
                var dbPasswords = _repository.GetByType(typeId);
                var password = dbPasswords.FirstOrDefault(x => x.Id == passwordId);

                if (password != null)
                    return EncryptService.Decrypt(key, password.Password);

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}\n\nStack Trace: {ex.StackTrace}\n\nInner Exception: {ex.InnerException}");
            }

            return null;
        }

        public void DeletePassword(int passwordId)
        {
            try
            {
                _repository.Delete(passwordId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}\n\nStack Trace: {ex.StackTrace}\n\nInner Exception: {ex.InnerException}");
                throw;
            }
        }

        public IEnumerable<PasswordModel> GetPasswordsFromType(int typeId)
        {
            try
            {
                return _repository.GetByType(typeId);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}\n\nStack Trace: {ex.StackTrace}\n\nInner Exception: {ex.InnerException}");
            }

            return Enumerable.Empty<PasswordModel>();
        }

        private PasswordModel GetByName(string name)
        {
            try
            {
                return _repository.GetByName(name);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Message: {ex.Message}\n\nStack Trace: {ex.StackTrace}\n\nInner Exception: {ex.InnerException}");
                throw;
            }
        }
    }
}