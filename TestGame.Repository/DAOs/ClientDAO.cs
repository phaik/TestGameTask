using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestGame.Repository.DAOs
{
    public class ClientDAO
    {
        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        [InverseProperty(nameof(LobbyDAO.Host))]
        public virtual ICollection<LobbyDAO> LobbyHostIdNavigations { get; set; }

        [InverseProperty(nameof(LobbyDAO.SecondClient))]
        public virtual ICollection<LobbyDAO> LobbySecondClientIdNavigations { get; set; }

        [InverseProperty(nameof(LobbyDAO.Winner))]
        public virtual ICollection<LobbyDAO> LobbyWinnerIdNavigations { get; set; }
    }
}
