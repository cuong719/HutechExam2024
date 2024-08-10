using System.Data;

namespace Hutech.Exam.Server.DAL.Repositories
{
    public interface IUserRepository
    {
        public IDataReader SelectOne(Guid userId);
        public IDataReader SelectByLoginName(string loginName);
    }
}
