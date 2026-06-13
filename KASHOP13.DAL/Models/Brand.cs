using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.Models
{
    public class Brand : AuditableEntity
    {
        public int Id { get; set; }
        public string Logo { get; set; }
        
        public List<BrandTranslation> Translations { get; set; }
        public List<Product> Products { get; set; }
    }
}
