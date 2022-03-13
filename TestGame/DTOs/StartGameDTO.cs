using System.ComponentModel.DataAnnotations;

namespace TestGame.DTOs
{
    public class StartGameDTO
    {
        [Required]
        public int SecondClientId { get; set; }
    }
}
