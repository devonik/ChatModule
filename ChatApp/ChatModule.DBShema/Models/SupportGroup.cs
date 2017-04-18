// ***********************************************************************
// Assembly         : ChatModule.DBShema
// Author           : grieger
// Created          : 03-09-2017
//
// Last Modified By : grieger
// Last Modified On : 03-10-2017
// ***********************************************************************
// <copyright file="SupportGroup.cs" company="Berenberg">
//     Copyright © Berenberg 2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatModule.DBShema.Models
{
    /// <summary>
    /// Class SupportGroup.
    /// </summary>
    public class SupportGroup
    {
        /// <summary>
        /// Gets or sets the supportgroup identifier.
        /// </summary>
        /// <value>The supportgroup identifier.</value>
        [Key]
        public int supportgroup_id { get; set; }
        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>The subject.</value>
        [Required]
        public string subject { get; set; }
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>The email.</value>
        public string email { get; set; }
    }
}
