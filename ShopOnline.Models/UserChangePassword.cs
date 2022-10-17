namespace ShopOnline.Models
{
    public class UserChangePassword
    {
        [Required, StringLength(20, MinimumLength = 6)]
        public string Password { get; set; } = string.Empty;

        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
