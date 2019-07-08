using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Infinterest.Models
{
public class User : BaseEntity
    {   
        // Database info
        [Key]
        public int UserId {get; set;}
        public string UserType {get; set;}
        public int AffiliateCode {get; set;}

        // User-input
            // required
        [DataType(DataType.Text)]
        [Display(Name = "First Name:")]
        [RegularExpression("^([a-zA-Z ]*)$"), StringLength(45, MinimumLength = 2)]
        [Required(ErrorMessage = "Your first name is required")]
        public string FirstName { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Last Name:")]
        [Required(ErrorMessage = "Your last name is required")]
        [MinLength(2, ErrorMessage = "Your last name has to be at least 2 letters")]
        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email:")]
        [Required(ErrorMessage = "Your email is required")]
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password:")]
        [Required(ErrorMessage = "A password is required")]
        [RegularExpression("^(?=.*[a-zA-Z])(?=.*[^a-zA-Z])(.{8,15})$", ErrorMessage = "Your password must contain an upper case letter, lower case letter, and special character or number")]
        [MinLength(8, ErrorMessage = "Your password must be at least 8 characters long")]
        public string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm your password:")]
        [Required(ErrorMessage = "Please Confirm Password")]
        [Compare(nameof(Password), ErrorMessage = "Your passwords need to match")]        
        public string PasswordConfirm { get; set; }
            // soon to be required
        [Required(ErrorMessage = "Please Add Image")]
        // [RegularExpression("^(https?:)?//?[^\'\"<>]+?.(jpg|jpeg|gif|png)$", ErrorMessage = "Right-click on an image online and select 'Copy Image Address' and paste it here")]
        [DataType(DataType.Text)]
        public string ImgUrl { get; set; }
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Add Contact Information")]

        public string Contact {get; set;}
        [DataType(DataType.Text)]

        public string Bio {get; set;}
        [DataType(DataType.Text)]
        [Required(ErrorMessage = "Please Add Company Name")]

        public string Company { get; set; }
        [DataType(DataType.Text)]

        public string Website { get; set; }

    }   
}