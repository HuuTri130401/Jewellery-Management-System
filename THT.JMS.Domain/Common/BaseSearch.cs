using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace THT.JMS.Domain.Common
{
    public class BaseSearch
    {
        /// <summary>
        /// Trang hiện tại
        /// </summary>
        [DefaultValue(1)]
        public int PageIndex { set; get; }

        /// <summary>
        /// Số lượng item trên 1 trang
        /// </summary>
        [DefaultValue(20)]
        public int PageSize { set; get; }

        /// <summary>
        /// Nội dung tìm kiếm chung
        /// </summary>
        [StringLength(1000, ErrorMessage = "Nội dung không vượt quá 1000 kí tự")]
        public string? SearchContent { set; get; }

        /// <summary>
        /// Không dùng
        /// </summary>
        [DefaultValue(0)]
        public virtual int OrderBy { set; get; }

        [DefaultValue(true)]
        public bool? IsPaged { set; get; }
    }
}
