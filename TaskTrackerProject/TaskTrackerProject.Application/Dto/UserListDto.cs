using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTrackerProject.Application.Infrastructure;
using TaskTrackerProject.Application.Model;

namespace TaskTrackerProject.Application.Dto
{
    public record UserListDto(
        Guid Guid,

        [StringLength(20, MinimumLength = 3, ErrorMessage = "Name must contain at least 3 letters")]
        string Name,
        Guid UserGuid) : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var db = validationContext.GetRequiredService<TaskTrackerContext>();
            if (!db.Users.Any(a => a.Guid == UserGuid))
            {
                yield return new ValidationResult("User does not exist", new[] { nameof(UserGuid) });
            }
        }
    }
}
