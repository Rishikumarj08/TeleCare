using System.ComponentModel.DataAnnotations;

namespace TeleCare.Model
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }
        public required string RoleName { get; set; }
    }
}