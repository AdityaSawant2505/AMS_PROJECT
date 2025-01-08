using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models.Users
{
    public class GetUsers
    {
        public long UserId { get; set; } // Maps to user_id
        public string UserHash { get; set; } // Maps to user_hash
        public string UserName { get; set; } // Maps to user_name
        public string UserEmail { get; set; } // Maps to user_email
        public string Department { get; set; } // Maps to department
        public string Designation { get; set; } // Maps to designation
        public long? ManagerId { get; set; } // Maps to manager_id
        public string PasswordHash { get; set; } // Maps to password_hash
        public string PasswordSalt { get; set; } // Maps to password_salt
        public bool IsDeleted { get; set; } // Maps to is_deleted
        public long CreatedBy { get; set; } // Maps to created_by
        public DateTime CreatedOn { get; set; } // Maps to created_on
        public long UpdatedBy { get; set; } // Maps to updated_by
        public DateTime UpdatedOn { get; set; } // Maps to updated_on
        public string RefreshToken { get; set; } // Maps to refresh_token
        public string ResetToken { get; set; } // Maps to reset_token
        public DateTime? ResetTokenExpiry { get; set; } // Maps to reset_token_expiry
        public string PhoneNumber { get; set; } // Maps to phone_number
        public string Otp { get; set; } // Maps to Otp
        public DateTime? OtpCreateTime { get; set; }
    }
}
