using Case_2___Zealand_Lokale_Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Text;
using System.Xml.Linq;


namespace Case_2___Zealand_Lokale_Booking.Pages.BookingPages
{
    public class BookingModel : PageModel
    {
        [BindProperty]
        public Booking Book { get; set; } = new Booking();


        public SelectList DagListe { get; private set; }
        public SelectList LokaleListe { get; private set; }
        public SelectList LokationsListe { get; private set; }
        public SelectList TidListe { get; private set; }
        public string ErrorMessage { get; set; }


        public void OnGet()
        {
            using BookngServiceContext context = new BookngServiceContext();


            List<Dag> DageFraDB = context.Dags.ToList();
            List<Lokale> LokalerFraDB = context.Lokales.ToList();
            List<Lokation> LokationerFraDB = context.Lokations.Include(l => l.AdresseNavigation).ToList();
            List<Tid> TiderFraDB = context.Tids.ToList();


            DagListe = new SelectList(DageFraDB, nameof(Dag.DagId), nameof(Dag.ValgteDag));
            LokaleListe = new SelectList(LokalerFraDB, nameof(Lokale.LokaleId), nameof(Lokale.LokaleNavnOgType));
            LokationsListe = new SelectList(LokationerFraDB, nameof(Lokation.LokationId), nameof(Lokation.ByNavn));
            TidListe = new SelectList(TiderFraDB, nameof(Tid.TidId), nameof(Tid.ValgteTid));


        }


        public IActionResult OnPostSubmit() 
        {
            using BookngServiceContext context = new BookngServiceContext();

            int BookingCounter = context.Bookings.Where(elm => elm.BrugerId == IndexModel.CurrentUser.BrugerId).Include(elm => elm.Tid).Include(elm => elm.Dag).Include(elm => elm.Lokale).Include(elm => elm.Lokation).ThenInclude(elm => elm.AdresseNavigation).ToList().Count();


            if (BookingCounter >= 6)
            {
                ErrorMessage = "Du kan maks have 6 bookings ad gangen!";
                return Page();
            }


            // Tjek om det indtastede data er validt
            if (!ModelState.IsValid)
            {
                return Page();
            }

            // Send data videre til repository
            Book.BrugerId = IndexModel.CurrentUser.BrugerId;
            context.Bookings.Add(Book);
            context.SaveChanges();

            // Vend tilbage til oversigen
            return RedirectToPage("BookingSucess");
        }







    }
}
