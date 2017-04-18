// ***********************************************************************
// Assembly         : ChatModule.DBShema
// Author           : grieger
// Created          : 03-09-2017
//
// Last Modified By : grieger
// Last Modified On : 03-29-2017
// ***********************************************************************
// <copyright file="User.cs" company="Berenberg">
//     Copyright © Berenberg 2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ChatModule.DBShema.Models
{
    //Struktur einer Tabelle
    /// <summary>
    /// Class User.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Key]
        public int user_id { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>The first name.</value>
        [Required]
        public string first_name { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>The last name.</value>
        [Required]
        public string last_name { get; set; }

        /// <summary>
        /// Gets or sets the avatarlink.
        /// </summary>
        /// <value>The avatarlink.</value>
        public string avatarlink { get; set; }

        /// <summary>
        /// Gets or sets the last logout.
        /// </summary>
        /// <value>The last logout.</value>
        public DateTime? last_logout { get; set; }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        [DefaultValue("offline")]
        public string status { get; set; }
    }
}
