using System.ComponentModel.DataAnnotations;

namespace MediQuickFinal.Models
{
    public class Medicine
    {

        [Key]
        public int  MediId { get; set; }

        [Required(ErrorMessage = "Medicine name is required.")]

        public string MediName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Medicine price is required.")]
        public int MediPrice { get; set; }

        [Required(ErrorMessage = "Medicine category is required.")]

        public MediCategory MediCategory { get; set; }
        public MediType MediType { get; set; }

        public byte[]? MediLogo { get; set; }
        public byte[]? MediBanner { get; set; }
        public byte[]? MediBackground { get; set; }

        public string? MediDescription { get; set; }
        public MediUser? MediManager { get; set; }

    }

    public enum MediCategory

    {
        Pharmacy,
        Personal_Care,
        Health_and_Nutrition,
        Baby_Needs,
        Vitamins_and_Supplements,
        Household_Needs,
        Ayurvedic,
        Elderly_Care,
        MediQuick_Plus
    }

    public enum MediType
    {
        Prescription,
        Pharmacy,
        Health_and_Nutrition,
        Lifestyle,
        Baby_and_Elderly
    }

}
