using SolidOps.Burgr.Shared.Application;
using SolidOps.Burgr.Shared.Contracts.Results;
using SolidOps.SubZero;

namespace SolidOps.UM.Application.Services;

public partial class TokenValidationService
{
    public override async Task<IOpsResult<string>> Validate()
    {
        await Task.CompletedTask;
        var dto = new ValidateTokenResultDTO()
        {
            UserId = executionScope.UserId,
            Applications = executionScope.Applications,
            Rights = executionScope.Rights
        };

        var serialization = Serializer.Serialize(dto);

        return IOpsResult.Ok(serialization);
    }
}
