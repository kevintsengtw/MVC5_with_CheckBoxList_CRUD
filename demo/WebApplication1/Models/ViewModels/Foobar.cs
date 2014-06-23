using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    [MetadataType(typeof(FoobarMetadata))]
    public partial class Foobar
    {
        public class FoobarMetadata
        {
            [Display(Name = "編號")]
            [Required(ErrorMessage = "為必要欄位")]
            public Guid ID { get; set; }

            [Display(Name = "名稱")]
            [Required]
            [StringLength(100, ErrorMessage = "請輸入名稱")]
            public string Name { get; set; }

            [Display(Name = "分類")]
            [Required(ErrorMessage = "請至少選擇一個分類")]
            public string Categories { get; set; }

            [Display(Name = "建立日期")]
            [Required(ErrorMessage = "為必要欄位")]
            public DateTime CreateDate { get; set; }

            [Display(Name = "更新日期")]
            [Required(ErrorMessage = "為必要欄位")]
            public DateTime UpdateDate { get; set; }

        }

        public string SelectedCategories { get; set; }

    }
}