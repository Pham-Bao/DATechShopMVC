//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DATechShop.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class HoaDon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HoaDon()
        {
            this.ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }
    
        public int id_HoaDon { get; set; }
        public Nullable<int> id_NguoiDung { get; set; }
        public Nullable<int> id_KhuyenMai { get; set; }
        public Nullable<System.DateTime> ngayTao { get; set; }
        public Nullable<int> trangThai { get; set; }
        public string sdt { get; set; }
        public string diaChi { get; set; }
        public Nullable<double> tongTien { get; set; }
        public Nullable<double> giamGia { get; set; }
        public Nullable<bool> TrangThaiXoa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public virtual KhuyenMai KhuyenMai { get; set; }
        public virtual NguoiDung NguoiDung { get; set; }
    }
}
