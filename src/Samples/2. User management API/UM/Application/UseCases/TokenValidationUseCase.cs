using SolidOps.UM.Shared.Application;
using SolidOps.UM.Shared.Contracts.Results;
using SolidOps.SubZero;

namespace SolidOps.UM.Application.UseCases;

public partial class TokenValidationUseCase
{
    public override async Task<IOpsResult<string>> Validate()
    {
        await Task.CompletedTask;
        var dto = new ValidateTokenResultDTO()
        {
            UserId = executionContext.UserId,
            Rights = executionContext.Rights
        };

        var serialization = Serializer.Serialize(dto);

        return IOpsResult.Ok(serialization);
    }
}
