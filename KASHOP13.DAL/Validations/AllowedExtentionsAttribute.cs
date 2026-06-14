using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KASHOP13.DAL.Validations
{
    public class AllowedExtentionsAttribute : ValidationAttribute
    {
        string[] _extentions = { ".png", ".jpg", ".webp" };
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is IFormFile file)
            {
                var extenstion = Path.GetExtension(file.FileName).ToLower();
                if (!_extentions.Contains(extenstion))
                {
                    return new ValidationResult($"Allowed Extention : {string.Join(",", _extentions)}");
                }
                
            }
            return ValidationResult.Success;
        }
    }
}
