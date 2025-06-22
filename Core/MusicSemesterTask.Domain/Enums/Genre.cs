using System.ComponentModel.DataAnnotations;

namespace MusicSemesterTask.Domain.Enums;

public enum Genre
{
    [Display(Name = "Pop")]
    Pop,
    [Display(Name = "Metall")]
    Metall,
    [Display(Name = "Rock")]
    Rock,
    [Display(Name = "HardRock")]
    HardRock,
    [Display(Name = "HipHop")]
    HipHop
    
}