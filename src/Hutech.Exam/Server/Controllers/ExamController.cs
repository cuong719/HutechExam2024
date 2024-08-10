using Hutech.Exam.Server.Attributes;
using Hutech.Exam.Server.BUS;
using Hutech.Exam.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text.Json;
using System.Xml.Linq;

namespace Hutech.Exam.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ExamController : Controller
    {
        private readonly ChiTietDeThiHoanViService _chiTietDeThiHoanViService;
        private readonly ChiTietBaiThiService _chiTietBaiThiService;
        private readonly AudioListenedService _audioListenedService;
        private readonly CustomDeThiService _customDeThiService;
        private readonly HttpClient _httpClient;
        public ExamController(ChiTietDeThiHoanViService chiTietDeThiHoanViService, ChiTietBaiThiService chiTietBaiThiService, 
            AudioListenedService audioListenedService, CustomDeThiService customDeThiService, HttpClient httpClient)
        {
            _chiTietDeThiHoanViService = chiTietDeThiHoanViService;
            _chiTietBaiThiService = chiTietBaiThiService;
            _audioListenedService = audioListenedService;
            _customDeThiService = customDeThiService;
            _httpClient = httpClient;
        }
        [HttpGet("GetDeThi")]
        [Cache]
        public ActionResult<List<CustomDeThi>> GetDeThi([FromQuery] long ma_de_thi_hoan_vi)
        {
            List<CustomDeThi> result = _customDeThiService.handleDeThi(ma_de_thi_hoan_vi);
            return Ok(result);
        }

        // Insert (có trả về list bài thi) giúp cho sinh viên tiếp tục thi trong trường hợp treo máy

        [HttpGet("InsertChiTietBaiThi")]
        public ActionResult<List<ChiTietBaiThi>> InsertChiTietBaiThi([FromQuery] int ma_chi_tiet_ca_thi ,[FromQuery] long ma_de_thi_hoan_vi)
        {
            List<CustomDeThi>? customDeThis = this.GetDeThi(ma_de_thi_hoan_vi).Value;
            _chiTietBaiThiService.insertChiTietBaiThis_SelectByChiTietDeThiHV(customDeThis, ma_chi_tiet_ca_thi, ma_de_thi_hoan_vi);

            // tránh trường hợp lấy đề của những môn khác
            List<ChiTietBaiThi> chiTietBaiThis = _chiTietBaiThiService.SelectBy_ma_chi_tiet_ca_thi(ma_chi_tiet_ca_thi);
            return chiTietBaiThis.Where(p => p.MaDeHv == ma_de_thi_hoan_vi).ToList();
        }

        [HttpPost("UpdateChiTietBaiThi")]
        public ActionResult UpdateChiTietBaiThi([FromBody] List<ChiTietBaiThi> chiTietBaiThis)
        {
            _chiTietBaiThiService.updateChiTietBaiThis(chiTietBaiThis);
            return Ok();
        }
        //private List<ChiTietBaiThi> handleDungSai(List<ChiTietBaiThi> chiTietBaiThis)
        //{
        //    List<int>? listDapAn = getListDapAn(chiTietBaiThis[0].MaDeHv).Result;
        //    foreach(var item in chiTietBaiThis)
        //        if(item.CauTraLoi != null && listDapAn?.Contains(item.CauTraLoi))
        //}
        private async Task<List<int>?> getListDapAn(long ma_de_hoan_vi)
        {
            HttpResponseMessage? response = null;
            List<int>? result = new List<int>();
            if (_httpClient != null)
                response = await _httpClient.GetAsync($"api/Result/GetListDapAn?ma_de_thi_hoan_vi={ma_de_hoan_vi}");
            if (response != null && response.IsSuccessStatusCode)
            {
                var resultString = await response.Content.ReadAsStringAsync();
                result = JsonSerializer.Deserialize<List<int>>(resultString, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });
            }
            return result;
        }

        [HttpGet("AudioListendCount")]
        public ActionResult<int> AudioListendCount([FromQuery] int ma_chi_tiet_ca_thi, [FromQuery] string filename)
        {
            return _audioListenedService.SelectOne(ma_chi_tiet_ca_thi, filename);
        }

    }
}
