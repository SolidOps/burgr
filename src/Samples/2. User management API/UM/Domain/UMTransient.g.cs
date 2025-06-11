using SolidOps.UM.Shared.Domain.Entities;
// using SolidOps.UM.Shared.Core.CrossCutting.Extension; //necessary for Serialize
using System.Linq; //necessary for Cast
using System; //necessary for Cast
using System.Collections.Generic;
namespace SolidOps.UM.Domain.Transients;
// Object [TR]
public partial class InviteResult : BaseInviteResult
{

    // Property [S][NP]
    public System.String Email { get; set; }

    public System.String Creator { get; set; }

    public System.String Message { get; set; }

    public virtual void CopyValues(InviteResult copy)
    {
        // Property [S][NO][NP][PUO][NAR]
        Email = copy.Email;

        Creator = copy.Creator;

        Message = copy.Message;

    }
}
public partial class LoginRequest : BaseLoginRequest
{

    // Property [S][NP]
    public System.String Login { get; set; }

    public System.String Password { get; set; }

    public virtual void CopyValues(LoginRequest copy)
    {
        // Property [S][NO][NP][PUO][NAR]
        Login = copy.Login;

        Password = copy.Password;

    }
}
public partial class SelfUserCreationRequest : BaseSelfUserCreationRequest
{

    // Property [S][NP]
    public System.String Email { get; set; }

    public System.String Password { get; set; }

    public virtual void CopyValues(SelfUserCreationRequest copy)
    {
        // Property [S][NO][NP][PUO][NAR]
        Email = copy.Email;

        Password = copy.Password;

    }
}
public partial class UserCreationInfo : BaseUserCreationInfo
{

    // Property [S][NP]
    public System.String UserEmail { get; set; }

    public System.String Password { get; set; }

    // Property [M][NP][AR]
    public List<Entities.UserRight> Rights { get; set; }

    public virtual void CopyValues(UserCreationInfo copy)
    {
        // Property [S][NO][NP][PUO][NAR]
        UserEmail = copy.UserEmail;

        Password = copy.Password;

        // Property [M][R][NO][NP][NA][PUO][AR][EN][AG]
        Rights = copy.Rights;

    }
}
// Object [TR]
public abstract class BaseInviteResult : ITransientEntity 
{

}
public abstract class BaseLoginRequest : ITransientEntity 
{

}
public abstract class BaseSelfUserCreationRequest : ITransientEntity 
{

}
public abstract class BaseUserCreationInfo : ITransientEntity 
{

}