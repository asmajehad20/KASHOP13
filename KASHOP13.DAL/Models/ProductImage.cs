using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
