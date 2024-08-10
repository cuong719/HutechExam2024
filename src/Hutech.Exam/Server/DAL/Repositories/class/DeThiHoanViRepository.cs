using Hutech.Exam.Server.DAL.DataReader;
using Hutech.Exam.Shared.Models;
using System.Data;

namespace Hutech.Exam.Server.DAL.Repositories
{
    public class DeThiHoanViRepository : IDeThiHoanViRepository
    {
        public IDataReader SelectOne(long ma_de_hoan_vi)
        {
            DatabaseReader sql = new DatabaseReader("tbl_DeThiHoanVi_SelectOne");
            sql.SqlParams("@MaDeHV", SqlDbType.BigInt, ma_de_hoan_vi);
            return sql.ExcuteReader();
        }
    }
}
