using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Case_2___Zealand_Lokale_Booking.Pages
{
    public class LogOutModel : PageModel
    {
        public async Task<IActionResult> OnGet()
        {
            IndexModel.CurrentUser = null;

            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToPage("/Index");
        }






    }
}
