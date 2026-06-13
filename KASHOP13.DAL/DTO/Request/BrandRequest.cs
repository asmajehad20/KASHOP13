using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.DAL.DTO.Request
{
    public class BrandRequest
    {
        public IFormFile Logo { get; set; }
        public List<BrandTranslationRequest> Translations { get; set; }
    }
}
