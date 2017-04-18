// ***********************************************************************
// Assembly         : ChatModule.DBShema
// Author           : grieger
// Created          : 03-09-2017
//
// Last Modified By : grieger
// Last Modified On : 03-10-2017
// ***********************************************************************
// <copyright file="User2SupportGroup.cs" company="Berenberg">
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
    /// Class User2SupportGroup.
    /// </summary>
    public class User2SupportGroup
    {
        /// <summary>
        /// Gets or sets the user2supportgroup identifier.
        /// </summary>
        /// <value>The user2supportgroup identifier.</value>
        [Key]
        public int user2supportgroup_id { get; set; }
        /// <summary>
        /// Gets or sets the supportgroup identifier.
        /// </summary>
        /// <value>The supportgroup identifier.</value>
        [Required]
        public int supportgroup_id { get; set; }
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [Required]
        public int user_id { get; set; }
        /// <summary>
        /// Gets or sets the support group.
        /// </summary>
        /// <value>The support group.</value>
        public virtual SupportGroup SupportGroup { get; set; }
        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public virtual User User { get; set; }
    }
}
