using Microsoft.AspNetCore.Mvc;
using Hutech.Exam.Server.BUS;
using Hutech.Exam.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Hutech.Exam.Server.Attributes;


namespace Hutech.Exam.Server.Controllers;

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]        
    public class ResultController : Controller
    {
    private readonly SinhVienService _sinhVienService;
    private readonly CaThiService _caThiService;
    private readonly ChiTietDeThiHoanViService _chiTietDeThiHoanViService;
    private readonly CauTraLoiService _cauTraLoiService;
    private readonly ChiTietCaThiService _chiTietCaThiService;
    public ResultController(SinhVienService sinhVienService, ChiTietDeThiHoanViService chiTietDeThiHoanViService, CaThiService caThiService,
        CauTraLoiService cauTraLoiService, ChiTietCaThiService chiTietCaThiService)
    {
        _sinhVienService = sinhVienService;
        _caThiService = caThiService;
        _chiTietDeThiHoanViService = chiTietDeThiHoanViService;
        _cauTraLoiService = cauTraLoiService;
        _chiTietCaThiService = chiTietCaThiService;
    }
    [HttpGet("GetThongTinSinhVien")]
    public ActionResult<SinhVien> GetThongTinSinhVien([FromQuery] long ma_sinh_vien)
    {
        return _sinhVienService.SelectOne(ma_sinh_vien);
    }
    [HttpGet("GetThongTinCaThi")]
    public ActionResult<CaThi> GetThongTinCaThi([FromQuery] int ma_ca_thi)
    {
        return _caThiService.SelectOne(ma_ca_thi);
    }
    [HttpGet("GetChiTietCaThiSelectBy_SinhVien")]
    // lấy chi tiết các thông tin của 1 sinh viên thi vào 1 ca giờ cụ thể (đề thi hoán vị)
    public ActionResult<ChiTietCaThi> GetChiTietCaThiSelectBy_SinhVien([FromQuery] int ma_ca_thi, [FromQuery] long ma_sinh_vien)
    {
        return _chiTietCaThiService.SelectBy_MaCaThi_MaSinhVien(ma_ca_thi, ma_sinh_vien);
    }
    [HttpGet("GetListDapAn")]
    [Cache(120)]
    public ActionResult<List<int>> GetListDapAn([FromQuery] long ma_de_thi_hoan_vi)
    {
        List<TblChiTietDeThiHoanVi> chiTietDeThiHoanVis = _chiTietDeThiHoanViService.SelectBy_MaDeHV(ma_de_thi_hoan_vi);
        List<int> listDapAn = new List<int>();
        foreach(var item in chiTietDeThiHoanVis)
        {
            if(item.DapAn != null)
                listDapAn.Add((int)item.DapAn);
        }
        return Ok(listDapAn);
    }
    [HttpPost("GetSoCauDung")]
    public ActionResult<int> GetSoCauDung([FromQuery] long ma_de_thi_hoan_vi, [FromBody] List<int> listKhoanh)
    {
        int tong_so_cau_dung = 0;
        List<int>? listDapAn = this.GetListDapAn(ma_de_thi_hoan_vi).Value;
        // Không tìm thấy đáp án đề hoặc thí sinh bỏ giấy trắng
        if(listDapAn == null || listKhoanh == null)
            return Ok(0);
        foreach (var item in listKhoanh)
            if (listDapAn.Contains(item))
                tong_so_cau_dung++;
        return Ok(tong_so_cau_dung);
    }
    [HttpPost("UpdateKetThuc")]
    public ActionResult UpdateKetThuc([FromBody] ChiTietCaThi chiTietCaThi)
    {
        _chiTietCaThiService.UpdateKetThuc(chiTietCaThi);
        return Ok();
    }
}

