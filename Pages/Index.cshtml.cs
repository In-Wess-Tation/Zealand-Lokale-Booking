using Case_2___Zealand_Lokale_Booking.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Case_2___Zealand_Lokale_Booking.Pages
{
    public class IndexModel : PageModel
    {

        public static Brugere? CurrentUser { get; set; }

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        [BindProperty]
        public string BrugerNavn { get; set; }

        [BindProperty, DataType(DataType.Password)]
        public string Kodeord { get; set; }

        public string ErrorMessage { get; set; }


        public void OnGet()
        {
            if (IndexModel.CurrentUser == null) // Force Signout on startup
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
        }


        public async Task<IActionResult> OnPostSubmit()
        {

            CurrentUser = VerifyBrugere(BrugerNavn, Kodeord);

            if (CurrentUser == null)
            {
                ErrorMessage = "Kunne ikke logge ind";
                return Page();

            }

            // Log ind
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                BuildClaimsPrincipal(CurrentUser));

            return RedirectToPage("BookingPages/SeeBookings");

        }


        private Brugere? VerifyBrugere(string providedBrugerNavn, string providedKodeord)
        {
            List<Brugere> b = new List<Brugere>();

            using BookngServiceContext context = new BookngServiceContext();

            b = context.Brugeres.ToList();

            Brugere? bruger = b.FirstOrDefault(bs => bs.BrugerNavn == providedBrugerNavn && bs.Kodeord == providedKodeord);

            return bruger;

        }


        private ClaimsPrincipal BuildClaimsPrincipal(Brugere bruger)
        {
            // Opbyg Claims-liste
            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, bruger.BrugerNavn));

            // Opret ClaimsIdentity (claims plus Authentication-strategi)
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Opret endeligt ClaimsPrincipal-objekt
            return new ClaimsPrincipal(claimsIdentity);
        }







    }
}
