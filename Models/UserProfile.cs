using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Infinterest.Models
{
public class UserProfile : User
    {   
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
            // soon to be required
        [DataType(DataType.Text)]
        public string Website { get; set; }

    }   
}