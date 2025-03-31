using Microsoft.AspNetCore.Mvc.Rendering;

namespace MediQuickFinal.Models.AdminViewModel
{
    public class MediViewModel
    {
        public int MediId { get; set; }
        public string? MediName { get; set; }
        public int MediPrice { get; set; }
        public string? MediDescription { get; set; }
        public MediUser? MediManager { get; internal set; }
        public MediCategory MediCategory { get; set; }
        public MediType MediType { get; set; }
        public byte[]? MediBackground { get; set; }

        public string? MediManagerEmail { get; set; }
        public List<SelectListItem> MediCategories
        {
            get
            {
                List<SelectListItem> selectListItem = Enum.GetValues<MediCategory>()
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString()
                    }).ToList();
                return selectListItem;
            }
        }
        public List<SelectListItem> MediTypes
        {
            get
            {
                List<SelectListItem> selectListItem = Enum.GetValues<MediType>()
                    .Select(x => new SelectListItem
                    {
                        Text = x.ToString(),
                        Value = x.ToString()
                    }).ToList();
                return selectListItem;
            }
        }
    }
}
