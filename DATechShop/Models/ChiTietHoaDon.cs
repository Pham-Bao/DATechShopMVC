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
    
    public partial class ChiTietHoaDon
    {
        public int id_chiTietHD { get; set; }
        public Nullable<int> id_HoaDon { get; set; }
        public Nullable<int> id_chiTietSP { get; set; }
        public Nullable<int> soLuong { get; set; }
        public Nullable<double> gia { get; set; }
    
        public virtual ChitietSP ChitietSP { get; set; }
        public virtual HoaDon HoaDon { get; set; }
    }
}
