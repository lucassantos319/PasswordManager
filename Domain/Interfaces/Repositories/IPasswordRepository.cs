using Domain.Models;

namespace Domain.Interfaces.Repositories
{
    public interface IPasswordRepository
    {
        void Add(PasswordModel passwordObj);

        PasswordModel GetById(int id);

        IEnumerable<PasswordModel> GetByType(int typeId);
    }
}