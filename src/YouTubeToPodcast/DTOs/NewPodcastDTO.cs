using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YouTubeToPodcast.DTOs
{
    public class NewPodcastDTO
    {
        [Required]
        [RegularExpression(@"(https?:\/\/)?(www\.)?(m\.)?youtube\.com\/(channel|user)\/[^\/?\s]+.*", ErrorMessage = "Enter a valid channel or user url.")]
        [Display(Prompt = "https://www.youtube.com/channel/id")]
        public string Url { get; set; }

        [Display(Prompt = "What's the title?")]
        public string Contains { get; set; }

        [Required]
        public int Duration { get; set; }

        public static List<SelectListItem> Durations { get; } = new List<SelectListItem>
        {
            new SelectListItem { Value = "0", Text = "Any" },
            new SelectListItem { Value = "5", Text = "Five minutes" },
            new SelectListItem { Value = "10", Text = "Ten minutes" },
            new SelectListItem { Value = "30", Text = "Half an hour" },
            new SelectListItem { Value = "60", Text = "An hour" }
        };
    }
}