using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using THT.JMS.Domain.Common;

namespace THT.JMS.Domain.Entities
{
    public class Users : BaseEntity
    {
        [Required]
        [Description("Mã người dùng")]
        public string Code { get; set; }

        [Required]
        [StringLength(50)]
        [Description("Tên đăng nhập")]
        public string UserName { get; set; }

        [Description("Tên")]
        [StringLength(200, ErrorMessage = "Tên phải nhỏ hơn 200 kí tự")]
        public string FirstName { get; set; }

        [Description("Họ")]
        [StringLength(300, ErrorMessage = "Tên phải nhỏ hơn 300 kí tự")]
        public string LastName { get; set; }

        [Description("Tên đầy đủ")]
        [StringLength(500)]
        public string FullName { get; set; }

        [Description("Số điện thoại")]
        [StringLength(20)]
        public string Phone { get; set; }

        [Description("Email")]
        [StringLength(50)]
        public string Email { get; set; }

        [Description("Địa chỉ")]
        [StringLength(1000)]
        public string Address { get; set; }

        [Description("Trạng thái")]
        public int? Status { get; set; }

        [Description("Ngày sinh")]
        public DateTime? Birthday { get; set; }

        [Description("Chứng minh nhân dân")]
        [StringLength(50)]
        public string IdentityCard { get; set; }

        [Description("Ngày cấp chứng minh nhân dân")]
        public DateTime? IdentityCardDate { get; set; }

        [Description("Nơi cấp chứng minh nhân dân")]
        [StringLength(1000, ErrorMessage = "Tên phải nhỏ hơn 300 kí tự")]
        public string IdentityCardAddress { get; set; }

        [Description("Mật khẩu")]
        [StringLength(4000)]
        public string Password { get; set; }

        /// <summary>
        /// Giới tính
        /// 0 => Khác
        /// 1 => Nam
        /// 2 => Nữ
        /// </summary>
        [Description("Giới tính")]
        public int? Gender { get; set; }

        [Description("Cờ Admin")]
        public bool? IsAdmin { get; set; }

        [Description("Doanh thu mua hàng")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal PurchaseRevenue { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        [NotMapped]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalProfit { get; set; }
        public ICollection<Roles> Roles { get; set; }
    }
}
