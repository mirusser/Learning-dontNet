using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandsService.DTOs
{
    public class CommandCreateDto
    {
        [Required]
        public string HowTo { get; set; } = null!;

        [Required]
        public string CommandLine { get; set; } = null!;
    }
}