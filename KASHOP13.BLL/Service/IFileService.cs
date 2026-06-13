using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace KASHOP13.BLL.Service
{
    public interface IFileService
    {
        Task<string?> UploadAsync(IFormFile file);
        void Delete(string fileName);
    }
}
