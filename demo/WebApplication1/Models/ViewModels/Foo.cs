using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models.ViewModels
{
    public class Foo
    {
        [Display(Name = "名稱")]
        [Required]
        [StringLength(100, ErrorMessage = "請輸入名稱")]
        public string Name { get; set; }

        [Display(Name = "分類")]
        [Required(ErrorMessage = "請至少選擇一個分類")]
        public string Categories { get; set; }

    }
}