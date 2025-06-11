using SolidOps.UM.Shared.Contracts.DTO;
using System.Collections.Generic;
using System; // needed for common types
namespace SolidOps.UM.Contracts
{
    public static class AssemblyReference
    {
    }
}
// Enum 
namespace SolidOps.UM.Contracts.Enums
{
    public enum InviteStatusEnum
    {
        Sent = 0,
Used = 1,
Declined = 2
    }
}
namespace SolidOps.UM.Contracts.DTO
{
    // Object [EN][AG][EXP]
    public partial class UserDTO
    {
        // Property [S][NO][NP][CA][PUO]
        public System.String Email { get; set; }

        public System.String Provider { get; set; }

        public System.Boolean TechnicalUser { get; set; }

        public List<System.String> Rights { get; set; }

        public string Id { get; set; }

        // relations

        // Property [M][NO][NP][CA][NA][PUO][AR]
        public List<SolidOps.UM.Contracts.DTO.UserRightDTO> UserRights { get; set; }

    }
    public partial class UserWriteDTO
    {
        public UserWriteDTO()
        {
        }
        public UserWriteDTO(UserDTO source)
        {
            // Property [S][NO][NP][PUO]
            // write
            this.Email = source.Email;

            // write
            this.Provider = source.Provider;

            // write
            this.TechnicalUser = source.TechnicalUser;

            // relations

            // Property [M][NO][NA][PUO][AR][EN][AG]
            // write
            this.UserRights = new List<SolidOps.UM.Contracts.DTO.UserRightWriteDTO>();
            if (source.UserRights != null)
            {
                foreach (var item in source.UserRights)
                {
                    this.UserRights.Add(new SolidOps.UM.Contracts.DTO.UserRightWriteDTO(item));
                }
            }

        }
        // Property [S][NO][NP][PUO]
        // write
        public System.String Email { get; set; }

        // write
        public System.String Provider { get; set; }

        // write
        public System.Boolean TechnicalUser { get; set; }

        // relations

        // Property [M][NO][NA][PUO][AR][EN][AG]
        // write
        public List<SolidOps.UM.Contracts.DTO.UserRightWriteDTO> UserRights { get; set; }

    }
    public partial class UserPatchDTO
    {
        // Property [S][NO][NP][PUO]
        // patch
        public System.String Email { get; set; }

        // patch
        public System.String Provider { get; set; }

        // patch
        public System.Boolean? TechnicalUser { get; set; }

        // relations

        // Property [M][NO][NA][PUO][AR][EN][AG]
        // patch
        public List<SolidOps.UM.Contracts.DTO.UserRightPatchDTO> UserRights { get; set; }

    }

    public partial class UserRightDTO
    {

        // Property [M][NO][PUO][EN][AG]
        public string UserId { get; set; }

        public string RightId { get; set; }

        public string Id { get; set; }

        // relations
        // Property [M][NO][NP][CA][NA][PUO][NAR]
        public SolidOps.UM.Contracts.DTO.UserDTO User { get; set; }

        public SolidOps.UM.Contracts.DTO.RightDTO Right { get; set; }

    }
    public partial class UserRightWriteDTO
    {
        public UserRightWriteDTO()
        {
        }
        public UserRightWriteDTO(UserRightDTO source)
        {

            // Property [M][NO][PUO][EN][AG]
            // write
            this.UserId = source.UserId;

            // write
            this.RightId = source.RightId;

            // relations
            // Property [M][NO][NA][PUO][NAR][EN][AG]
            // write
            this.User = new SolidOps.UM.Contracts.DTO.UserWriteDTO(source.User);

            // write
            this.Right = new SolidOps.UM.Contracts.DTO.RightWriteDTO(source.Right);

        }

        // Property [M][NO][PUO][EN][AG]
        // write
        public string UserId { get; set; }

        // write
        public string RightId { get; set; }

        // relations
        // Property [M][NO][NA][PUO][NAR][EN][AG]
        // write
        public SolidOps.UM.Contracts.DTO.UserWriteDTO User { get; set; }

        // write
        public SolidOps.UM.Contracts.DTO.RightWriteDTO Right { get; set; }

    }
    public partial class UserRightPatchDTO
    {

        // Property [M][NO][PUO][EN][AG]
        // patch
        public string UserId { get; set; }

        // patch
        public string RightId { get; set; }

        // relations
        // Property [M][NO][NA][PUO][NAR][EN][AG]
        // patch
        public SolidOps.UM.Contracts.DTO.UserPatchDTO User { get; set; }

        // patch
        public SolidOps.UM.Contracts.DTO.RightPatchDTO Right { get; set; }

    }

    public partial class RightDTO
    {
        // Property [S][NO][NP][CA][PUO]
        public System.String Name { get; set; }

        public string Id { get; set; }

        // relations

    }
    public partial class RightWriteDTO
    {
        public RightWriteDTO()
        {
        }
        public RightWriteDTO(RightDTO source)
        {
            // Property [S][NO][NP][PUO]
            // write
            this.Name = source.Name;

            // relations

        }
        // Property [S][NO][NP][PUO]
        // write
        public System.String Name { get; set; }

        // relations

    }
    public partial class RightPatchDTO
    {
        // Property [S][NO][NP][PUO]
        // patch
        public System.String Name { get; set; }

        // relations

    }

    public partial class InviteDTO
    {
        // Property [S][NO][NP][CA][PUO]
        public System.String Email { get; set; }

        public System.String CreatorName { get; set; }

        public System.String CreatorMessage { get; set; }

        // Property [E][NO][NP][CA][PUO][NAR]
        public SolidOps.UM.Contracts.Enums.InviteStatusEnum Status { get; set; }

        // Property [M][NO][PUO][EN][AG]
        public string CreatorId { get; set; }

        public string Id { get; set; }

        // relations
        // Property [M][NO][NP][CA][NA][PUO][NAR]
        public SolidOps.UM.Contracts.DTO.UserDTO Creator { get; set; }

    }
    public partial class InviteWriteDTO
    {
        public InviteWriteDTO()
        {
        }
        public InviteWriteDTO(InviteDTO source)
        {
            // Property [S][NO][NP][PUO]
            // write
            this.Email = source.Email;

            // write
            this.CreatorName = source.CreatorName;

            // write
            this.CreatorMessage = source.CreatorMessage;

            // Property [E][NO][NP][PUO][NAR]
            // write
            this.Status = source.Status;

            // Property [M][NO][PUO][EN][AG]
            // write
            this.CreatorId = source.CreatorId;

            // relations
            // Property [M][NO][NA][PUO][NAR][EN][AG]
            // write
            this.Creator = new SolidOps.UM.Contracts.DTO.UserWriteDTO(source.Creator);

        }
        // Property [S][NO][NP][PUO]
        // write
        public System.String Email { get; set; }

        // write
        public System.String CreatorName { get; set; }

        // write
        public System.String CreatorMessage { get; set; }

        // Property [E][NO][NP][PUO][NAR]
        // write
        public SolidOps.UM.Contracts.Enums.InviteStatusEnum Status { get; set; }

        // Property [M][NO][PUO][EN][AG]
        // write
        public string CreatorId { get; set; }

        // relations
        // Property [M][NO][NA][PUO][NAR][EN][AG]
        // write
        public SolidOps.UM.Contracts.DTO.UserWriteDTO Creator { get; set; }

    }
    public partial class InvitePatchDTO
    {
        // Property [S][NO][NP][PUO]
        // patch
        public System.String Email { get; set; }

        // patch
        public System.String CreatorName { get; set; }

        // patch
        public System.String CreatorMessage { get; set; }

        // Property [E][NO][NP][PUO][NAR]
        // patch
        public SolidOps.UM.Contracts.Enums.InviteStatusEnum Status { get; set; }

        // Property [M][NO][PUO][EN][AG]
        // patch
        public string CreatorId { get; set; }

        // relations
        // Property [M][NO][NA][PUO][NAR][EN][AG]
        // patch
        public SolidOps.UM.Contracts.DTO.UserPatchDTO Creator { get; set; }

    }

    // Object [TR]
    public partial class InviteResultDTO
    {

        // Property [S][NO][NP][CA][PUO]
        public System.String Email { get; set; }

        public System.String Creator { get; set; }

        public System.String Message { get; set; }

    }

    public partial class LoginRequestDTO
    {

        // Property [S][NO][NP][CA][PUO]
        public System.String Login { get; set; }

        public System.String Password { get; set; }

    }

    public partial class SelfUserCreationRequestDTO
    {

        // Property [S][NO][NP][CA][PUO]
        public System.String Email { get; set; }

        public System.String Password { get; set; }

    }

    public partial class UserCreationInfoDTO
    {

        // Property [S][NO][NP][CA][PUO]
        public System.String UserEmail { get; set; }

        public System.String Password { get; set; }

        // Property [M][NO][NP][PUO][EN][AG]
        public string RightsId { get; set; }

        // Property [M][NO][NP][CA][NA][PUO][AR]
        public List<SolidOps.UM.Contracts.DTO.UserRightDTO> Rights { get; set; }

    }

    // Object [EN][AG]
    public partial class LocalUserQueryFilterDTO : IQueryFilterDTO
    {
        public string Filter { get; set; }
        public List<OrderByClause> OrderBy { get; set; }
        public int? MaxResults { get; set; }
        public string ContinuationId { get; set; }
    }

    public partial class UserQueryFilterDTO : IQueryFilterDTO
    {
        public string Filter { get; set; }
        public List<OrderByClause> OrderBy { get; set; }
        public int? MaxResults { get; set; }
        public string ContinuationId { get; set; }
    }

    public partial class UserRightQueryFilterDTO : IQueryFilterDTO
    {
        public string Filter { get; set; }
        public List<OrderByClause> OrderBy { get; set; }
        public int? MaxResults { get; set; }
        public string ContinuationId { get; set; }
    }

    public partial class RightQueryFilterDTO : IQueryFilterDTO
    {
        public string Filter { get; set; }
        public List<OrderByClause> OrderBy { get; set; }
        public int? MaxResults { get; set; }
        public string ContinuationId { get; set; }
    }

    public partial class InviteQueryFilterDTO : IQueryFilterDTO
    {
        public string Filter { get; set; }
        public List<OrderByClause> OrderBy { get; set; }
        public int? MaxResults { get; set; }
        public string ContinuationId { get; set; }
    }

}