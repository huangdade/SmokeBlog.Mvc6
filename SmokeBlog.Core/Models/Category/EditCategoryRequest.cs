using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SmokeBlog.Core.Models.Category
{
    public class EditCategoryRequest : AddCategoryRequest
    {
        [Required]
        public int? ID { get; set; }
    }
}
