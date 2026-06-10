using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.DTO.Response
{
    public class CategoryResponse
    {
        public int Id { get; set; }
        public List<CategoryTranslationResponse> Translations { get; set; }
    }
}
