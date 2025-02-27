﻿using Hutech.Exam.Client.DAL;
using Hutech.Exam.Shared.Models;
using Microsoft.AspNetCore.Components;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Hutech.Exam.Client.Authentication;
using System.Net.Http.Headers;
namespace Hutech.Exam.Client.Pages;

public partial class Result
{
    [Inject]
    HttpClient? httpClient { get; set; }
    [Inject]
    ApplicationDataService? myData { get; set; }
    [Inject]
    AuthenticationStateProvider? authenticationStateProvider { get; set; }
    [Inject]
    NavigationManager? navManager { get; set; }
    [Inject]
    IJSRuntime? js { get; set; }
    private SinhVien? sinhVien { get; set; }
    private CaThi? caThi { get; set; }
    private ChiTietCaThi? chiTietCaThi { get; set; }
    private List<CustomDeThi>? customDeThis { get; set; }
    private List<int>? listDapAn { get; set; }
    private List<bool>? ketQuaDapAn { get; set; }
    private double diem { get; set; }
    private int so_cau_dung { get; set; }
    private async Task checkPage()
    {
        if ((myData == null || myData.chiTietCaThi == null || myData.sinhVien == null) && js != null)
        {
            await js.InvokeVoidAsync("alert", "Cách hoạt động trang trang web không hợp lệ. Vui lòng quay lại");
            navManager?.NavigateTo("/info");
            return;
        }
        if (myData != null && myData.chiTietCaThi != null)
        {
            khoiTaoBanDau();
            chiTietCaThi = myData.chiTietCaThi;
            caThi = myData.chiTietCaThi.MaCaThiNavigation;
            sinhVien = myData.sinhVien;
            await Start();
        }
    }
    protected override async Task OnInitializedAsync()
    {
        //xác thực người dùng
        var customAuthStateProvider = (authenticationStateProvider != null) ? (CustomAuthenticationStateProvider)authenticationStateProvider : null;
        var token = (customAuthStateProvider != null) ? await customAuthStateProvider.GetToken() : null;
        if (!string.IsNullOrWhiteSpace(token) && httpClient != null)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
        }
        else
        {
            navManager?.NavigateTo("/");
        }
        await checkPage();
        await base.OnInitializedAsync();
    }

    private async Task HandleUpdateKetThuc()
    {
        if(chiTietCaThi != null && httpClient != null && myData != null && customDeThis != null)
        {
            chiTietCaThi.ThoiGianKetThuc = DateTime.Now;
            chiTietCaThi.Diem = diem;
            chiTietCaThi.SoCauDung = so_cau_dung;
            chiTietCaThi.TongSoCau = customDeThis.Count;
            var jsonString = JsonSerializer.Serialize(chiTietCaThi);
            await httpClient.PostAsync("api/Result/UpdateKetThuc", new StringContent(jsonString, Encoding.UTF8, "application/json"));
        }
    }
    private void tinhDiemSo()
    {
        diem = so_cau_dung = 0;
        double diem_tung_cau = 0;
        if(customDeThis != null)
            diem_tung_cau = (10.0 / customDeThis.Count);
        int length = 0;
        if(myData != null && myData.listDapAnKhoanh != null)
            length = myData.listDapAnKhoanh.Count;
        if(myData != null && myData.listDapAnKhoanh != null && myData.listDapAnGoc != null && ketQuaDapAn != null)
        {
            foreach (var item in myData.listDapAnGoc)
            {
                if (myData.listDapAnKhoanh.Contains(item))
                {
                    diem += diem_tung_cau;
                    so_cau_dung++;
                    ketQuaDapAn.Add(true);
                }
                else
                    ketQuaDapAn.Add(false);
            }
        }


        diem = quyDoiDiem(diem);
    }
    private double quyDoiDiem(double diem)
    {
        double so_phay = diem % 1;
        if (so_phay > 0 && so_phay <= 0.25)
            return Math.Floor(diem) + 0.3;
        if (so_phay > 0.25 && so_phay <= 0.5)
            return Math.Floor(diem) + 0.5;
        if (so_phay > 0.5 && so_phay <= 0.75)
            return Math.Floor(diem) + 0.8;
        if(so_phay > 0.75)
            return Math.Ceiling(diem);
        return Math.Floor(diem);
    }
    private async Task onClickDangXuatAsync()
    {
        bool result = (js != null) && await js.InvokeAsync<bool>("confirm", "Bạn có chắc chắn muốn đăng xuất?");
        if (result)
        {
            await UpdateLogout();
            var customAuthStateProvider = (authenticationStateProvider!=null) ? (CustomAuthenticationStateProvider)authenticationStateProvider : null;
            if(customAuthStateProvider != null)
                await customAuthStateProvider.UpdateAuthenticationState(null);
            navManager?.NavigateTo("/", true);
        }
    }
    private async Task UpdateLogout()
    {
        if(httpClient != null && sinhVien != null)
            await httpClient.GetAsync($"api/User/UpdateLogout?ma_sinh_vien={sinhVien.MaSinhVien}");
    }
    private void khoiTaoBanDau()
    {
        sinhVien = new SinhVien();
        caThi = new CaThi();
        chiTietCaThi = new ChiTietCaThi();
        if(myData != null)
        {
            listDapAn = myData.listDapAnGoc;
            customDeThis = myData.customDeThis;
        }
        ketQuaDapAn = new List<bool>();
    }

    private async Task Start()
    {
        tinhDiemSo();
        await HandleUpdateKetThuc();
    }
}