using System.ComponentModel.DataAnnotations;

namespace MusicSemesterTask.Domain.Enums;

public enum Country
{
    [Display(Name = "Usa")]
    Usa,
    [Display(Name = "Russia")]
    Russia,
    [Display(Name = "China")]
    China,
    [Display(Name = "France")]
    France,
    [Display(Name = "Uk")]
    Uk
    
}