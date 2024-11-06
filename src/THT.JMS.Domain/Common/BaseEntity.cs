using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace THT.JMS.Domain.Common
{
    public class BaseEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Description("ID dữ liệu")]
        public Guid Id { get; set; }

        [Description("Thời gian tạo")]
        public DateTime? Created { get; set; }

        [Description("Người tạo")]
        public Guid? CreatedBy { get; set; }

        [Description("Thời gian cập nhật")]
        public DateTime? Updated { get; set; }

        [Description("Người cập nhật")]
        public Guid? UpdatedBy { get; set; }

        [Description("Cờ xóa")]
        public bool? Deleted { get; set; } = false;

        [Description("Cờ active")]
        public bool? IsActive { get; set; } = true;

        [NotMapped]
        public string CreatedByName { get; set; }
        [NotMapped]
        public string CreatedByCode { get; set; }
        [NotMapped]
        public int TotalItem { get; set; }
    }
}
