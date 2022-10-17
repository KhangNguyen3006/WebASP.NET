using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsiteBH.Models
{
    public partial class OrderMasterData
    {
        public int id { get; set; }
        [Display(Name = "Tên đơn hàng")]
        public string Name { get; set; }
        [Display(Name = "Người mua hàng")]
        public Nullable<int> UserId { get; set; }
        [Display(Name = "Tổng giá trị đơn hàng")]
        public Nullable<double> Price { get; set; }
        [Display(Name = "Trạng thái đơn hàng")]
        public Nullable<int> Status { get; set; }
        [Display(Name = "Thời gian tạo đơn hàng")]
        public Nullable<System.DateTime> CreatedOnUtc { get; set; }
    }
}