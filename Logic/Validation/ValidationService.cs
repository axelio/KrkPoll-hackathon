using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using KrkPoll.Utils.Result;

namespace KrkPoll.Logic.Validation
{
    public interface IValidationService
    {
        Task<Result<T>> ValidateRulesAsync<T>(IValidator<T> validator, T obj);
    }

    public class ValidationService : IValidationService
    {
        public async Task<Result<T>> ValidateRulesAsync<T>(IValidator<T> validator, T obj)
        {
            if (obj == null) return Result.Fail<T>("The sent object cannot be null");
            ValidationResult results = await validator.ValidateAsync(obj);
                
            return !results.IsValid ? Result.Fail<T>(GetErrorMessages(results.Errors)) : Result.Ok(obj);
        }

        private string GetErrorMessages(IList<ValidationFailure> failrues)
        {
            StringBuilder errorMessageBuilder = new StringBuilder();
            foreach (var errorMessage in failrues)
            {
                errorMessageBuilder.Append(errorMessage.ErrorMessage);
            }

            return errorMessageBuilder.ToString();
        }
    }
}
