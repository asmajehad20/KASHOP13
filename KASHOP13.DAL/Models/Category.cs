using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.Models
{
    public class Category : AuditableEntity
    {
        public int Id { get; set; }

        public List<CategoryTranslation> Translations { get; set; }
    }
}
