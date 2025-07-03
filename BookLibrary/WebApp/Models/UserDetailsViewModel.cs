using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WebApp.Models
{
    public class UserDetailsViewModel
    {
        [ValidateNever]
        public UserViewModel User { get; set; } = null!;

        [ValidateNever]
        public IEnumerable<UserReservationViewModel> Reservations { get; set; } = new List<UserReservationViewModel>();

        [ValidateNever]
        public IEnumerable<UserReviewViewModel> Reviews { get; set; } = new List<UserReviewViewModel>();
    }
}
