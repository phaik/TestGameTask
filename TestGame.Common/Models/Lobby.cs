using System;

namespace TestGame.Common.Models
{
    public class Lobby
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
