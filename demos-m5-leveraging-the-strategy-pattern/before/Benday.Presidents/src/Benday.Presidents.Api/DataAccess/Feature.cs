using Benday.Presidents.Api.Models;
using System.ComponentModel.DataAnnotations;

namespace Benday.Presidents.Api.DataAccess;
public class Feature : Int32Identity
{
    [Display(Name = "Feature Name")]
    [Required]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Name { get; set; }

    [Display(Name = "Is Enabled")]
    public bool IsEnabled { get; set; }

    [Display(Name = "For Username")]
    [DisplayFormat(ConvertEmptyStringToNull = false)]
    public string Username { get; set; }
}