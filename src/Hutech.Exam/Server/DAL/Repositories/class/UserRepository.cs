﻿using Hutech.Exam.Server.DAL.DataReader;
using Hutech.Exam.Shared.Models;
using System.Data;

namespace Hutech.Exam.Server.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        public IDataReader SelectOne(Guid userId)
        {
            DatabaseReader sql = new DatabaseReader("User_SelectOne");
            sql.SqlParams("@UserId", SqlDbType.UniqueIdentifier, userId);
            return sql.ExcuteReader();
        }
        public IDataReader SelectByLoginName(string loginName)
        {
            DatabaseReader sql = new DatabaseReader("User_SelectByLoginName");
            sql.SqlParams("@LoginName", SqlDbType.NVarChar, loginName);
            return sql.ExcuteReader();
        }
    }
}
