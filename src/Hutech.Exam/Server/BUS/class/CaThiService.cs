﻿using Hutech.Exam.Server.DAL.Repositories;
using Hutech.Exam.Shared.Models;
using System.Data;

namespace Hutech.Exam.Server.BUS
{
    public class CaThiService
    {
        private readonly ICaThiRepository _caThiRepository;
        public CaThiService(ICaThiRepository caThiRepository)
        {
            _caThiRepository = caThiRepository;
        }
        private CaThi getProperty(IDataReader dataReader)
        {
            CaThi caThi = new CaThi();
            caThi.MaCaThi = dataReader.GetInt32(0);
            caThi.TenCaThi = dataReader.IsDBNull(1) ? null : dataReader.GetString(1);
            caThi.MaChiTietDotThi = dataReader.GetInt32(2);
            caThi.ThoiGianBatDau = dataReader.GetDateTime(3);
            caThi.MaDeThi = dataReader.GetInt32(4);
            caThi.IsActivated = dataReader.GetBoolean(5);
            caThi.ActivatedDate = dataReader.IsDBNull(6) ? null : dataReader.GetDateTime(6);
            caThi.ThoiGianThi = dataReader.GetInt32(7);
            caThi.KetThuc = dataReader.GetBoolean(8);
            caThi.ThoiDiemKetThuc = dataReader.IsDBNull(9) ? null : dataReader.GetDateTime(9);
            caThi.MatMa =  dataReader.IsDBNull(10) ? null : dataReader.GetString(10);
            caThi.Approved = dataReader.GetBoolean(11);
            caThi.ApprovedDate = dataReader.IsDBNull(12) ? null : dataReader.GetDateTime(12);
            caThi.ApprovedComments = dataReader.IsDBNull(13) ? null : dataReader.GetString(13);
            return caThi;
        }
        public List<CaThi> SelectBy_ma_chi_tiet_dot_thi(int ma_chi_tiet_dot_thi)
        {
            List<CaThi> list = new List<CaThi>();
            using(IDataReader dataReader = _caThiRepository.SelectBy_ma_chi_tiet_dot_thi(ma_chi_tiet_dot_thi))
            {
                while (dataReader.Read())
                {
                    CaThi caThi = getProperty(dataReader);
                    list.Add(caThi);
                }
            }
            return list;
        }
        public CaThi SelectOne(int ma_ca_thi)
        {
            CaThi caThi = new CaThi();
            using(IDataReader dataReader = _caThiRepository.SelectOne(ma_ca_thi))
            {
                if (dataReader.Read())
                {
                    caThi = getProperty(dataReader);
                }
            }
            return caThi;
        }
        public List<CaThi> ca_thi_GetAll()
        {
            List<CaThi> list = new List<CaThi>();
            using (IDataReader dataReader = _caThiRepository.ca_thi_GetAll())
            {
                while (dataReader.Read())
                {
                    CaThi caThi = getProperty(dataReader);
                    list.Add(caThi);
                }
            }
            return list;
        }
        public void ca_thi_Activate(int ma_ca_thi, bool IsActivated)
        {
            try
            {
                _caThiRepository.ca_thi_Activate(ma_ca_thi, IsActivated);
            } catch(Exception ex)
            {
                throw new Exception("Không thể kích hoạt hoặc hủy kích hoạt ca thi " + ex.Message);
            }
        }
        public void ca_thi_Ketthuc(int ma_ca_thi)
        {
            try
            {
                _caThiRepository.ca_thi_Ketthuc(ma_ca_thi);
            }
            catch (Exception ex)
            {
                throw new Exception("Không thể kết thúc ca thi " + ex.Message);
            }
        }
    }
}
