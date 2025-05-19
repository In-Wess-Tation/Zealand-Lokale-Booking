using Case_2___Zealand_Lokale_Booking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Case_2___Zealand_Lokale_Booking.Pages.BookingPages
{
    public class SeeBookingsModel : PageModel
    {
        [BindProperty]
/*        public Booking Book { get; set; } = new Booking();
*/        public List<Booking> Book { get; private set; }


/*        public SelectList BookingListe { get; private set; }
*/

        public void OnGet()
        {
            using BookngServiceContext context = new BookngServiceContext();


            Book = context.Bookings.Where(elm => elm.BrugerId == IndexModel.CurrentUser.BrugerId).Include(elm => elm.Tid).Include(elm => elm.Dag).Include(elm => elm.Lokale).Include(elm => elm.Lokation).ThenInclude(elm => elm.AdresseNavigation).ToList();



/*            BookingListe = new SelectList(BookingsFraDB, nameof(Booking.BookingId), nameof(Booking.Lokale), nameof(Booking.Lokation), nameof(Booking.Dag), nameof(Booking.Tid));
*/


        }

        public IActionResult OnPostSubmit(int id)
        {
            using BookngServiceContext context = new BookngServiceContext();


            // Send data videre til repository
            Booking b = context.Bookings.Find(id);
            context.Bookings.Remove(b);
            context.SaveChanges();

            // Vend tilbage til oversigen
            return RedirectToPage("SeeBookings");
        }






    }
}
