using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineGameStore.BLL.DTOs.Platforms;

public class PlatformBasicDto
{
    [Required]
    public required Guid Id { get; set; }

    [Required]
    public required string Name { get; set; }
}

