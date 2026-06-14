using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace KASHOP13.DAL.Validations
{
    public class MaxFileSizeAttribute : ValidationAttribute
    {
        private readonly int _maxSizeInMb;
        public MaxFileSizeAttribute(int maxSizeInMb)
        { 
            _maxSizeInMb = maxSizeInMb;
        }
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if(value is IFormFile file)
            {
                var sizeInMb = file.Length / (1024 * 1024);
                if(sizeInMb > _maxSizeInMb)
                {
                    return new ValidationResult($"Max file size is : {_maxSizeInMb}Mb");
                }
            }
            return ValidationResult.Success;
            
        }
    }
}
