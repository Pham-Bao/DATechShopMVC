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
    
    public partial class ChitietSP
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ChitietSP()
        {
            this.ChiTietHoaDons = new HashSet<ChiTietHoaDon>();
        }
    
        public int id_chiTietSP { get; set; }
        public Nullable<int> id_sanPham { get; set; }
        public Nullable<int> id_tuyChon { get; set; }
        public Nullable<int> id_Mau { get; set; }
        public Nullable<double> giaSP { get; set; }
        public string anhSP { get; set; }
        public Nullable<bool> TrangThaiXoa { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public virtual MauSac MauSac { get; set; }
        public virtual SanPham SanPham { get; set; }
        public virtual TuyChon TuyChon { get; set; }
    }
}
