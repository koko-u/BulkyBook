using System.ComponentModel.DataAnnotations;
using BulkyBook.Persistence.Data;

namespace BulkyBook.Validations;

public class IsUniqueAttribute : ValidationAttribute
{
    public string? GetErrorMessage()
    {
        return string.IsNullOrEmpty(ErrorMessage) ? "The value must be unique." : ErrorMessage;
    }


    public Type? ModelType { get; set; }


    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        ArgumentNullException.ThrowIfNull(ModelType);

        if (validationContext.GetService(typeof(BulkyBookDbContext)) is BulkyBookDbContext dbContext)
        {
            var targetInstance = validationContext.ObjectInstance;
            var memberName = validationContext.MemberName;
            if (string.IsNullOrEmpty(memberName))
            {
                throw new Exception("target member name is empty.");
            }

            var entity = dbContext.Model.GetEntityTypes()
                                  .FirstOrDefault(
                                      entity => string.Equals(entity.Name, ModelType.FullName));
            if (entity == null)
            {
                throw new Exception($"{ModelType} is not found in the DbContext");
            }

            var dbSet = dbContext.GetType()
                                 .GetMethod("Set", Type.EmptyTypes)?
                                 .MakeGenericMethod(entity.ClrType)
                                 .Invoke(dbContext, null);
            if (dbSet is IQueryable<object> collection)
            {
                var list = collection.ToList();
                if (list.Any(entry => Equals(GetPropertyValueByName(entry, memberName), value)))
                {
                    return new ValidationResult(GetErrorMessage());
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
        }

        throw new Exception("BulkyBookDbContext cannot cast");
    }

    private static object? GetPropertyValueByName(object? obj, string propName)
    {
        if (obj == null)
        {
            return null;
        }

        var prop = obj.GetType().GetProperty(propName);
        if (prop == null)
        {
            return null;
        }

        return prop.GetValue(obj);
    }
}