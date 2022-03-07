using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CuaHangThucPham.Models;


namespace CuaHangThucPham.Controllers
{
    public class QuanLyLoaiThanhVienController : Controller
    {
        QuanLyEntities db = new QuanLyEntities();
        // GET: QuanLyLoaiThanhVien
        public ActionResult Index()
        {
            var lstLTV = db.LoaiThanhViens;
            return View(lstLTV);
        }
        [HttpGet]
        public ActionResult TaoMoi()
        {
            return View();
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult TaoMoi(LoaiThanhVien ltv)
        {
            db.LoaiThanhViens.Add(ltv);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult ChinhSua(int? id)
        {
            //lay ncc can chinh sua
            if (id == null) {
                Response.StatusCode = 404;
                return null;
            }
            LoaiThanhVien ltv = db.LoaiThanhViens.SingleOrDefault(n => n.MaLoaiTV == id);
            if (ltv == null) {
                return HttpNotFound();
            }
            return View(ltv);
        }
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult ChinhSua(LoaiThanhVien ltv)
        {
            db.Entry(ltv).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Xoa(int? id)
        {
            //lay ncc can chinh sua
            if (id == null) {
                Response.StatusCode = 404;
                return null;
            }
            LoaiThanhVien ltv = db.LoaiThanhViens.SingleOrDefault(n => n.MaLoaiTV == id);
            if (ltv == null) {
                return HttpNotFound();
            }
            return View(ltv);
        }
        [HttpPost]
        public ActionResult Xoa(int id)
        {
            if (id == null) {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            LoaiThanhVien ltv = db.LoaiThanhViens.SingleOrDefault(n => n.MaLoaiTV == id);
            if (ltv == null) {
                return HttpNotFound();
            }
            db.LoaiThanhViens.Remove(ltv);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}