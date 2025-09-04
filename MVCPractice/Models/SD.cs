namespace MVCPractice.Models
{
    public static class SD   // SD = Static Details / Status Details
    {
        // Order Status
        public const string StatusPending = "Pending";
        public const string StatusConfirmed = "Confirmed";
        public const string StatusShipped = "Shipped";
        public const string StatusDelivered = "Delivered";

        // Payment Status
        public const string PaymentPending = "Pending";
        public const string PaymentApproved = "Approved";
        public const string PaymentRejected = "Rejected";

        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employe";
    }
}
