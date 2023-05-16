using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using TerritorialHQ.Helpers;
using TerritorialHQ.Services;

namespace TerritorialHQ.Areas.Administration.Pages.Clans
{
    [Authorize(Roles = "Administrator, Staff")]
    public class EditModel : PageModel
    {

        private readonly IMapper _mapper;
        private readonly LoggerService _logger;
        private readonly ClanService _service;
        private readonly IWebHostEnvironment _env;

        public EditModel(IMapper mapper, LoggerService logger, ClanService service, IWebHostEnvironment env)
        {
            _mapper = mapper;
            _logger = logger;
            _service = service;
            _env = env;
        }


        [BindProperty]
        public string Id { get; set; }
        [BindProperty]
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [BindProperty]
        [Display(Name = "Discord Guild ID")]
        public ulong? GuildId { get; set; }
        [BindProperty]
        [Display(Name = "Logo file")]
        public string? LogoFile { get; set; }
        [BindProperty]
        [Display(Name = "Banner file")]
        public string? BannerFile { get; set; }
        [BindProperty]
        [Display(Name = "Discord server link")]
        public string? DiscordLink { get; set; }
        [BindProperty]
        [Display(Name = "Description")]
        public string? Description { get; set; }


        [BindProperty]
        public bool RemoveLogo { get; set; }
        [BindProperty]
        public bool RemoveBanner { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _service.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }

            _mapper.Map(item, this);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile? fileLogo, IFormFile? fileBanner)
        {
            if (!ModelState.IsValid)
            {
                IEnumerable<ModelError> allErrors = ModelState.Values.SelectMany(v => v.Errors);

                return Page();
            }

            var item = await _service.FindAsync(this.Id);
            _mapper.Map(this, item);

            if (fileLogo != null)
            {
                item.LogoFile = await ImageHelper.ProcessImage(fileLogo, _env.WebRootPath + "/Data/Uploads/System/", true, item.LogoFile, false);
            }
            else if (RemoveLogo && !string.IsNullOrEmpty(item.LogoFile))
            {
                var oldFilePath = Path.Combine(_env.WebRootPath + "/Data/Uploads/System/", item.LogoFile);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                item.LogoFile = null;
                RemoveLogo = false;
            }

            if (fileBanner != null)
            {
                item.BannerFile = await ImageHelper.ProcessImage(fileBanner, _env.WebRootPath + "/Data/Uploads/System/", true, item.BannerFile, false);
            }
            else if (RemoveBanner && !string.IsNullOrEmpty(item.BannerFile))
            {
                var oldFilePath = Path.Combine(_env.WebRootPath + "/Data/Uploads/System/", item.BannerFile);
                if (System.IO.File.Exists(oldFilePath))
                    System.IO.File.Delete(oldFilePath);

                item.BannerFile = null;
                RemoveBanner = false;
            }

            _service.Update(item);

            try
            {
                await _service.SaveChangesAsync(User);
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await ClanExists(Id);
                if (!exists)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private async Task<bool> ClanExists(string id)
        {
            return await _service.ExistsAsync(id);
        }
    }
}
