using System;
using System.ComponentModel.DataAnnotations;

public class FutureDateAttribute : ValidationAttribute
{
    private readonly int _daysInFuture;

    public FutureDateAttribute(int daysInFuture)
    {
        _daysInFuture = daysInFuture;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        DateTime expiryDate = (DateTime)value;
        DateTime sevenDaysLater = DateTime.Now.AddDays(_daysInFuture);

        if (expiryDate < sevenDaysLater)
        {
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}