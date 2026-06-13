using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.DTO.Request
{
    public class ProductTranslationRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Language {  get; set; }
    }
}
