using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestGame.Repository.DAOs
{
    public class LobbyDAO
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int HostId { get; set; }

        public int? SecondClientId { get; set; }

        public DateTime CreateDate { get; set; }

        [DefaultValue(10)]
        public int HostHealth { get; set; }

        [DefaultValue(10)]
        public int SecondClientHealth { get; set; }

        public DateTime? StartDate { get; set; }

        public int MovesCount { get; set; }

        public int? WinnerId { get; set; }

        [ForeignKey(nameof(HostId))]
        [InverseProperty(nameof(ClientDAO.LobbyHostIdNavigations))]
        public virtual ClientDAO Host { get; set; }

        [ForeignKey(nameof(SecondClientId))]
        [InverseProperty(nameof(ClientDAO.LobbySecondClientIdNavigations))]
        public virtual ClientDAO SecondClient { get; set; }

        [ForeignKey(nameof(WinnerId))]
        [InverseProperty(nameof(ClientDAO.LobbyWinnerIdNavigations))]
        public virtual ClientDAO Winner { get; set; }
    }
}
