using System.ComponentModel.DataAnnotations;
using DAL.Services;

namespace DAL.Attributes;

public class ExistsAttribute<T> : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is int id)
        {
            // Resolve the Service from the DI container
            var service = (IEntityService<T>?)validationContext.GetService(typeof(IEntityService<T>));
            if (service == null)
                return new ValidationResult($"{typeof(T).Name} service not found.");

            if (!service.Exists(id))
                return new ValidationResult(ErrorMessage ?? $"{typeof(T).Name} with Id {id} does not exist.");
        }

        return ValidationResult.Success;
    }
}