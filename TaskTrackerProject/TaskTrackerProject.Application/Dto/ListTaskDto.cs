using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using TaskTrackerProject.Application.Infrastructure;
using TaskTrackerProject.Application.Model;

namespace TaskTrackerProject.Application.Dto
{
    public record ListTaskDto(
        Guid Guid,

        [StringLength(20, MinimumLength = 3, ErrorMessage = "name must contain at least 3 letters")]
        string Name,
        Status Status,
        Priority Priority,
        DateTime Date,
        Guid ListGuid) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var db = validationContext.GetRequiredService<TaskTrackerContext>();
            if (!db.Lists.Any(a => a.Guid == ListGuid))
            {
                yield return new ValidationResult("List does not exist", new[] { nameof(ListGuid) });
            }
        }
    }
}
