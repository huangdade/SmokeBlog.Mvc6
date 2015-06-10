using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using SmokeBlog.Core.Extensions;

namespace SmokeBlog.Core.Models.Category
{
    public class DeleteCategoryRequest
    {
        [Required]
        public string ID { get; set; }

        public int[] CategoryIDList
        {
            get
            {
                return this.ID.ToIntArray();
            }
        }
    }
}
