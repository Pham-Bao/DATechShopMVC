﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DATechShop.Areas.Admin.Content;
using DATechShop.Models;
using PagedList;
using static DATechShop.Areas.Admin.Content.AuthAttribute;

namespace DATechShop.Areas.Admin.Controllers
{
    public class NguoiDungController : Controller

    {
		// GET: Admin/NguoiDung
		DATotNghiepEntities db = new DATotNghiepEntities();
		[AdminAuth]
		public ActionResult DanhSachNguoiDung(int? page)
		{
			mapNguoiDung map = new mapNguoiDung();
			var data = map.DanhSachNguoiDung().OrderByDescending(x => x.id_NguoiDung); 
			int pageSize = 6; 
			int pageNumber = (page ?? 1);

			
			var pagedList = data.ToPagedList(pageNumber, pageSize);

			return View(pagedList);
		}
		public ActionResult DangKyTaiKhoan()
		{

			return View();

		}
		[HttpPost]
		public ActionResult DangKyTaiKhoan(NguoiDung model, string matKhau, string mk)
		{
			var existingPhone = db.NguoiDungs.FirstOrDefault(u => u.sdt == model.sdt);
			var existingEmail = db.NguoiDungs.FirstOrDefault(u => u.email == model.email);
			if (existingPhone != null)
			{
				ViewBag.phone = "(*)Số điện thoại đã được sử dụng!";
				return View();
			}

			if (existingEmail != null)
			{
				ViewBag.phone = "(*)Email đã được sử dụng!";
				return View();
			}

			if (matKhau != mk)
			{
				ViewBag.error = "Mật Khẩu không khớp";
				return View();
			}

			
			
			string hashedPassword = HashingHelper.HashPassword(matKhau);
			model.matKhau = hashedPassword;

			var emailAddress = model.email;
			try
			{
				// Kiểm tra địa chỉ email có hợp lệ không
				if (!IsValidEmail(emailAddress))
				{
					ViewBag.error = "Địa chỉ email không hợp lệ.";
					return View("DangKyTaiKhoan");
				}

				// Tạo mã OTP ngẫu nhiên
				string otp = GenerateOTP();

				// Gửi mã OTP đến địa chỉ email của người dùng
				SendEmail(emailAddress, "Mã OTP", $"Mã OTP của bạn là: {otp}");
				db.NguoiDungs.Add(model);
				db.SaveChanges();

				// Chuyển hướng đến trang nhập mã OTP để người dùng nhập
				return RedirectToAction("Verify", new { emailAddress = emailAddress, otp = otp });
			}
			catch (Exception ex)
			{
				// Xử lý lỗi nếu có
				ViewBag.error = "Đã xảy ra lỗi khi gửi mã OTP. Vui lòng thử lại sau.";
				return View("DangKyTaiKhoan");
			}
			
			
		}

		public ActionResult Verify(string emailAddress, string otp)
		{
			ViewBag.EmailAddress = emailAddress;
			ViewBag.OTP = otp;
			return View();
		}

		// POST: OTP/Verify
		[HttpPost]
		public ActionResult Verify(string emailAddress, string otpEntered, string otp)
		{
			
			if (otpEntered == otp)
			{
				var user = db.NguoiDungs.FirstOrDefault(u => u.email == emailAddress);

				if (user != null)
				{
					user.TrangThaiXoa = false; // Cập nhật trạng thái trangthaiXoa thành false
					db.SaveChanges(); // Lưu thay đổi vào cơ sở dữ liệu
					ViewBag.Success = "Xác thực thành công! Tài khoản của bạn đã được kích hoạt.";
				}
			}
			else
			{
				// Mã OTP nhập không đúng
				ViewBag.Error = "Mã OTP không đúng. Vui lòng thử lại.";
			}

			ViewBag.EmailAddress = emailAddress;
			ViewBag.OTP = otp;
			return View();
		}





		public ActionResult DangNhap()
		{

			return View();

		}

		[HttpPost]
		public ActionResult DangNhap(string sdt, string mk)
		{
			
			var user = db.NguoiDungs.FirstOrDefault(u => u.sdt == sdt);


			if (sdt.ToLower() == "admin" & mk.ToLower() == "1")
			{
				Session["tk"] = "AdminTechShop";
				return RedirectToAction("ThongKe", "SanPham");
			}
			if (user != null && HashingHelper.VerifyPassword(mk, user.matKhau))
			{
				// Đăng nhập thành công
				Session["id_NguoiDung"] = user.id_NguoiDung;
				Session["TenNguoiDung"] = user.ten;
				Session["SoDienThoai"] = user.sdt;
				Session["DiaChi"] = user.diaChi;

				ViewBag.Success = "Đăng Nhập thành công";
				TempData["SuccessMessage"] = "Thêm sản phẩm thành công";

				// Gọi hàm JavaScript để hiển thị thông báo
				ViewBag.NotificationMessage = TempData["SuccessMessage"];
				return View();
			}
			else
			{
				// Đăng nhập không thành công
				ViewBag.Error = "Số điện thoại hoặc mật khẩu không đúng.";
				return View();
			}
		}


		public ActionResult DangXuat()
		{
			Session.Remove("id_NguoiDung");
			Session.Remove("TenNguoiDung");
			Session.Remove("SoDienThoai");
			Session.Remove("DiaChi");
			Session.Remove("tk");

			return RedirectToAction("DangNhap", "NguoiDung");
		}

		private string GenerateOTP()
		{
			Random rand = new Random();
			return rand.Next(100000, 999999).ToString(); // Mã OTP là một số có 6 chữ số
		}
		private DateTime GetExpiryTime()
		{
			return DateTime.Now.AddMinutes(1); // Mã OTP chỉ có hiệu lực trong 1 phút
		}

		// Phương thức để gửi email chứa mã OTP đến địa chỉ email của người dùng
		private void SendEmail(string toAddress, string subject, string body)
		{
			var fromAddress = new MailAddress(ConfigurationManager.AppSettings["FromEmailAddress"]);
			var toAddr = new MailAddress(toAddress);
			string smtpServer = ConfigurationManager.AppSettings["SMTPServer"];
			int smtpPort = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);
			string username = ConfigurationManager.AppSettings["SMTPUsername"];
			string password = ConfigurationManager.AppSettings["SMTPPassword"];

			var smtp = new SmtpClient
			{
				Host = smtpServer,
				Port = smtpPort,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(username, password)
			};

			using (var message = new MailMessage(fromAddress, toAddr)
			{
				Subject = subject,
				Body = body
			})
			{
				smtp.Send(message);
			}
		}

		// Phương thức để kiểm tra địa chỉ email có hợp lệ không
		private bool IsValidEmail(string email)
		{
			try
			{
				var addr = new MailAddress(email);
				return addr.Address == email;
			}
			catch
			{
				return false;
			}
		}
	}
}