﻿using System.ComponentModel.DataAnnotations;

namespace OnlineGameStore.BLL.DTOs;

public class GenreCreateDto
{

    [Required]
    public required string Name { get; set; }

    [Required]
    public string Description { get; set; } = string.Empty;

    public Guid? ParentId { get; set; } = null;

    [Required]
    public ICollection<Guid> GamesIds { get; set; } = new List<Guid>();
}
