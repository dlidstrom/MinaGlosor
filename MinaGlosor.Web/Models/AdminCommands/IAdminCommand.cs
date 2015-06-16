using System.ComponentModel.DataAnnotations;

namespace MinaGlosor.Web.Models.AdminCommands
{
    public interface IAdminCommand
    {
        [Required]
        string RequestUsername { get; }

        [Required]
        string RequestPassword { get; }
    }
}