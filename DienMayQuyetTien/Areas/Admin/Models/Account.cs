﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
namespace DienMayQuyetTien.Areas.Admin.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Account
    {
        [Required(ErrorMessage = "Bạn phải nhập username")]
        [StringLength(20)]
        [Display(Name = "Username: ")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Bạn phải nhập password")]
        [DataType(DataType.Password)]
        [StringLength(150, MinimumLength = 4)]
        [Display(Name = "Password: ")]
        public string Password { get; set; }

        [StringLength(150)]
        [Display(Name = "Full Name: ")]
        public string Fullname { get; set; }
    }
}
