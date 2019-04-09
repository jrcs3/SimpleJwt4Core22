using System.ComponentModel.DataAnnotations;

namespace SimpleJwt4Core22.ViewModels
{
    public class MakeTokenViewModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Role { get; set; }
        // To siumulate login or other failure:
        public bool Fail { get; set; }
    }
}
