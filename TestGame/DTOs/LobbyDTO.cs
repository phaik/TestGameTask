using System;

namespace TestGame.DTOs
{
    public class LobbyDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int HostId { get; set; }

        public int? SecondClientId { get; set; }

        public DateTime CreateDate { get; set; }

        public int HostHealth { get; set; }

        public int SecondClientHealth { get; set; }

        public DateTime? StartDate { get; set; }

        public int MovesCount { get; set; }

        public int? WinnerId { get; set; }
    }
}
