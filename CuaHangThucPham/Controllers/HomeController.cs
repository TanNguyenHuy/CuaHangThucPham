using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangThucPham.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;

namespace CuaHangThucPham.Controllers
{
    public class HomeController : Controller
    {
        QuanLyEntities db = new QuanLyEntities();
        public ActionResult Index()
        {
            //tao viewbag de lay san pham moi tu co so du lieu
            var lstSPM = db.SanPhams.Where(n => n.Moi == 1 && n.DaXoa == false);
            //gan vao viewbag
            ViewBag.ListSPM = lstSPM;
            return View();
        }
        public ActionResult MenuPartial()
        {
            var lstSP = db.SanPhams;
            return PartialView(lstSP);
        }
        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(ThanhVien tv)
        {
            ViewBag.CauHoi = new SelectList(LoadCauHoi());
            //kiểm tra captcha
            if (this.IsCaptchaValid("captcha is not valid")) {
                if (ModelState.IsValid) {
                    if (CheckTaiKhoan(tv.TaiKhoan)) {
                        ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                    }
                    else if (CheckEmail(tv.Email)) {
                        ModelState.AddModelError("", "Email đã tồn tại");
                    }
                    else if (CheckSDT(tv.SoDienThoai)) {
                        ModelState.AddModelError("", "Số điện thoại đã tồn tại");
                    }
                    else {
                        ViewBag.ThongBao = "Tạo tài khoản thành công";
                        //thêm khách hàng vào csdl
                        db.ThanhViens.Add(tv);
                        db.SaveChanges();
                    }
                    
                }
                else {
                    ViewBag.ThongBao = "Tạo tài khoản thất bại";
                }
                return View();
            }
            ViewBag.ThongBao = "Sai mã Captcha";
            return View();
        }
        public List<string> LoadCauHoi()
        {
            List<string> lstCauHoi = new List<string>();
            lstCauHoi.Add("Con vật mà bạn yêu thích?");
            lstCauHoi.Add("Ca sỹ mà bạn yêu thích");
            lstCauHoi.Add("Hiện tại bạn đang làm công việc gì?");
            return lstCauHoi;
        }
        public ActionResult DangNhap()
        {
            return View();
        }
        //xay dung action dang nhap
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            //kiem tra ten dang nhap va mat khau
            string sTaiKhoan = f["txtTenDangNhap"].ToString();
            string sMatKhau = f["txtMatKhau"].ToString();
            ThanhVien tv = db.ThanhViens.SingleOrDefault(n => n.TaiKhoan == sTaiKhoan && n.MatKhau == sMatKhau);
            if (tv != null) {
                Session["TaiKhoan"] = tv;
                return RedirectToAction("Index");
            }
            ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng!";
            return View();
        }
        public ActionResult DangXuat()
        {
            Session["TaiKhoan"] = null;
            return RedirectToAction("Index");
        }
        public bool CheckTaiKhoan (string taiKhoan)
        {
            return db.ThanhViens.Count(n => n.TaiKhoan == taiKhoan) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.ThanhViens.Count(n => n.Email == email) > 0;
        }
        public bool CheckSDT(string sdt)
        {
            return db.ThanhViens.Count(n => n.SoDienThoai == sdt) > 0;
        }
    }

}