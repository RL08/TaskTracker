using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskTrackerProject.Application.Model;

namespace TaskTrackerProject.Application.Dto
{
    public record ListTaskDto(
        Guid Guid,

        [StringLength(20, MinimumLength = 3, ErrorMessage = "name must contain at least 3 letters")]
        string Name,
        string Status,
        string Priority,
        UserList List,
        DateTime Date);
}
