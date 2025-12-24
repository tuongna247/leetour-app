using Repository.Pattern.Ef6;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Vinaday.Data.Models
{
    public class Customer : Entity
    {
        public int? AddressId { get; set; }

        public string ANSWER1 { get; set; }
        public string ANSWER2 { get; set; }

        public string BANKNAME { get; set; }

        public string BANKNUMBER { get; set; }

        public virtual ICollection<Booking> BOOKINGs { get; set; }

        public string CARDHOLDER { get; set; }

        public int? CardId { get; set; }

        public virtual ICollection<COMMENT> COMMENTs { get; set; }

        public int CustomerId { get; set; }

        public string CVCODE { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public virtual ICollection<DAYTRIPBOOKING> DAYTRIPBOOKINGs { get; set; }

        public string District { get; set; }

        public string Email { get; set; }

        public DateTime? EXPIRYDATE { get; set; }

        public string Firstname { get; set; }

        public string IdCard { get; set; }

        public bool? ISBANK { get; set; }
        public bool? IsCall { get; set; }

        public bool? ISSENDMAIL { get; set; }

        public string Lastname { get; set; }

        public string MemberId { get; set; }

        public int? MEMBERTYPE { get; set; }

        public int? NationalId { get; set; }
        public bool? IsThirdPerson { get; set; }
        public string ThirdPersonGender { get; set; }
        public string Gender { get; set; }
        public string ThirdPersonFirstname { get; set; }
        public string ThirdPersonLastname { get; set; }
        public bool? IsGiftVoucher { get; set; }
        public string Gift_type { get; set; }
        public string PersonalMessage { get; set; }

        public virtual Vinaday.Data.Models.Nationality Nationality { get; set; }

        public string Nickname { get; set; }

        public string Password { get; set; }
      //  public bool VNSite { get; set; }

        public virtual ICollection<PasswordResetRequest> PasswordResetRequests { get; set; }

        public string PhoneNumber { get; set; }

        public string PhoneNumber2 { get; set; }

        //public string PromotionCode
        //{
        //    get;
        //    set;
        //}

        //public decimal DiscountValue
        //{
        //    get;
        //    set;
        //}

        public int? Priority { get; set; }

        public string SECRETQUESTION1 { get; set; }

        public string SECRETQUESTION2 { get; set; }

        public int? Status { get; set; }

        public string Street { get; set; }

        public virtual ICollection<TOURBOOKING> TOURBOOKINGs { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public string Ward { get; set; }

        public Customer()
        {
            this.BOOKINGs = new List<Booking>();
            this.COMMENTs = new List<COMMENT>();
            this.DAYTRIPBOOKINGs = new List<DAYTRIPBOOKING>();
            this.PasswordResetRequests = new List<PasswordResetRequest>();
            this.TOURBOOKINGs = new List<TOURBOOKING>();
        }
    }
}