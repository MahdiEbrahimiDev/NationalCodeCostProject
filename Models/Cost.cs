using System.ComponentModel.DataAnnotations;

namespace NationalCodeCostProject.Models;

public class Cost
{
       public int Id { get; set; }

       [Required(ErrorMessage = "کد ملی الزامی است")]
[RegularExpression(@"^\d{10}$", ErrorMessage = "کد ملی باید دقیقاً ۱۰ رقم باشد")]

    public string NationalCode { get; set; }
    public decimal Amount { get; set; }
}
