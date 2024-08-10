﻿using Hutech.Exam.Server.DAL.DataReader;
using System.Data;

namespace Hutech.Exam.Server.DAL.Repositories
{
    public class ChiTietDeThiHoanViRepository : IChiTietDeThiHoanViRepository
    {
        public IDataReader SelectBy_MaDeHV_MaNhom_MaChiTietCaThi_v3(long maDeHV, int maNhom, int maChiTietCaThi)
        {
            DatabaseReader sql = new DatabaseReader("tbl_ChiTietDeThiHoanVi_SelectBy_MaDeHV_MaNhom_MaChiTietCaThi_v3");
            sql.SqlParams("@MaDeHV", SqlDbType.BigInt, maDeHV);
            sql.SqlParams("@MaNhom", SqlDbType.Int, maNhom);
            sql.SqlParams("@MaChiTietCaThi", SqlDbType.Int, maChiTietCaThi);
            return sql.ExcuteReader();
        }
        public IDataReader SelectBy_MaDeHV(long maDeHV)
        {
            DatabaseReader sql = new DatabaseReader("tbl_ChiTietDeThiHoanVi_SelectBy_MaDeHV");
            sql.SqlParams("@MaDeHV", SqlDbType.BigInt, maDeHV);
            return sql.ExcuteReader();
        }
        public IDataReader SelectBy_MaDeHV_MaNhom(long ma_de_hoan_vi, int ma_nhom)
        {
            DatabaseReader sql = new DatabaseReader("tbl_ChiTietDeThiHoanVi_SelectBy_MaDeHV_MaNhom");
            sql.SqlParams("@MaDeHV", SqlDbType.BigInt, ma_de_hoan_vi);
            sql.SqlParams("@MaNhom", SqlDbType.Int, ma_nhom);
            return sql.ExcuteReader();
        }
    }
}
