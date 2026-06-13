using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.DTO.Response
{
    public class CategoryResponse
    {
        public int Category_Id { get; set; }
        public string User {  get; set; }
        //public List<CategoryTranslationResponse> Translations { get; set; }

        public string Name { get; set; }
    }
}
