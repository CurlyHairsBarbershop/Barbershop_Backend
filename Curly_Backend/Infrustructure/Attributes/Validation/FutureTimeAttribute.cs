using System.ComponentModel.DataAnnotations;

namespace Infrustructure.Attributes.Validation;

public class FutureTimeAttribute : ValidationAttribute
{
    public override bool IsValid(object? o)
    {
        if (o is DateTime dateTime)
        {
            return dateTime > DateTime.Now;
        }

        return false;
    }
}