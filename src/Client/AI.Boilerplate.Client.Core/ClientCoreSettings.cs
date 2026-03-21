
namespace AI.Boilerplate.Client.Core;

public partial class ClientCoreSettings : SharedSettings
{
    /// <summary>
    /// If you're running AI.Boilerplate.Server.Web project, then you can also use relative urls such as / for Blazor Server and WebAssembly
    /// </summary>
    [Required]
    public string ServerAddress { get; set; } = default!;

    [Required]
    public string GoogleRecaptchaSiteKey { get; set; } = default!;

    /// <summary>
    /// The advertisement's unit path of the google ads from the Google Ad Manager panel.
    /// </summary>
    [Required]
    public string AdUnitPath { get; set; } = default!;

    public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        var validationResults = base.Validate(validationContext).ToList();

        if (AppEnvironment.IsDevelopment() is false && GoogleRecaptchaSiteKey is "6LdMKr4pAAAAAKMyuEPn3IHNf04EtULXA8uTIVRw")
        {
            validationResults.Add(new ValidationResult("Please set your own GoogleRecaptchaSiteKey in Client.Core's appsettings.json"));
        }

        return validationResults;
    }
}
