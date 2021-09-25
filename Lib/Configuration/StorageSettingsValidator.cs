using FluentValidation;

namespace Lib.Configuration
{
    public class StorageSettingsValidator : AbstractValidator<StorageSettings>, IStorageSettingsValidator
    {
        public StorageSettingsValidator()
        {
            RuleFor(x => x.Endpoint).NotEmpty();
            RuleFor(x => x.AccessKey).NotEmpty();
            RuleFor(x => x.SecretKey).NotEmpty();
        }
    }

    public interface IStorageSettingsValidator : IValidator<StorageSettings>
    {
    }
}