using System.Collections.Generic;
using System; // needed for common types
// Enum 
namespace SolidOps.TODO.Contracts.Enums
{
    public enum ItemStatusEnum
    {
        Todo = 0,
InProgress = 1,
Done = 2
    }
}
namespace SolidOps.TODO.Contracts.DTO
{
    // Object [EN][AG][EXP]
    public partial class ItemDTO
    {
        // Property [S][NO][NP][CA][PUO]
        public System.String Name { get; set; }

        public System.DateTime DueDate { get; set; }

        public System.Int32 RemainingDays { get; set; }

        // Property [E][NO][NP][CA][PUO][NAR]
        public SolidOps.TODO.Contracts.Enums.ItemStatusEnum Status { get; set; }

        public string Id { get; set; }

        // relations

    }
    public partial class ItemWriteDTO
    {
        public ItemWriteDTO()
        {
        }
        public ItemWriteDTO(ItemDTO source)
        {
            // Property [S][NO][NP][PUO]
            // write
            this.Name = source.Name;

            // write
            this.DueDate = source.DueDate;

            // Property [E][NO][NP][PUO][NAR]
            // write
            this.Status = source.Status;

            // relations

        }
        // Property [S][NO][NP][PUO]
        // write
        public System.String Name { get; set; }

        // write
        public System.DateTime DueDate { get; set; }

        // Property [E][NO][NP][PUO][NAR]
        // write
        public SolidOps.TODO.Contracts.Enums.ItemStatusEnum Status { get; set; }

        // relations

    }
    public partial class ItemPatchDTO
    {
        // Property [S][NO][NP][PUO]
        // patch
        public System.String Name { get; set; }

        // patch
        public System.DateTime? DueDate { get; set; }

        // Property [E][NO][NP][PUO][NAR]
        // patch
        public SolidOps.TODO.Contracts.Enums.ItemStatusEnum Status { get; set; }

        // relations

    }

}