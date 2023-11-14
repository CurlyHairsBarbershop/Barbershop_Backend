using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Infrustructure.Attributes.Validation;

public class NotEmptyCollectionAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is IEnumerable collection && collection.GetEnumerator().MoveNext())
        {
            return true;
        }
        return false;
    }
}