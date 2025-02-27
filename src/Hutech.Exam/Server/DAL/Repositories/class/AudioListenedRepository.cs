﻿using Hutech.Exam.Server.DAL.DataReader;
using Hutech.Exam.Shared.Models;
using System.Data;

namespace Hutech.Exam.Server.DAL.Repositories
{
    public class AudioListenedRepository : IAudioListenedRepository
    {
        public IDataReader SelectOne(int ma_chi_tiet_ca_thi, string filename)
        {
            DatabaseReader sql = new DatabaseReader("tbl_AudioListened_SelectOne");
            sql.SqlParams("@MaChiTietCaThi", SqlDbType.Int, ma_chi_tiet_ca_thi);
            sql.SqlParams("@FileName", SqlDbType.NVarChar, filename);
            return sql.ExcuteReader();
        }
    }
}
