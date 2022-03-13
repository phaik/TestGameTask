using System.ComponentModel.DataAnnotations;

namespace TestGame.DTOs
{
    public class CreateLobbyDTO
    {
        [Required]
        public int HostId { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
