using System.ComponentModel.DataAnnotations;

namespace Benday.Presidents.Api.Models;

public class Term
{
    public Term()
    {

    }

    public int Id { get; set; }

    public bool IsDeleted { get; set; }

    [Required]
    public string Role { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = false)]
    public DateTime Start { get; set; }

    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = false)]
    public DateTime End { get; set; }
    public int Number { get; set; }
}