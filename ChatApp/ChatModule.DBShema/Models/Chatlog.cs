// ***********************************************************************
// Assembly         : ChatModule.DBShema
// Author           : grieger
// Created          : 03-09-2017
//
// Last Modified By : grieger
// Last Modified On : 03-10-2017
// ***********************************************************************
// <copyright file="Chatlog.cs" company="Berenberg">
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
    /// <summary>
    /// Class Chatlog.
    /// </summary>
    public class Chatlog
    {
        /// <summary>
        /// Gets or sets the chatlog identifier.
        /// </summary>
        /// <value>The chatlog identifier.</value>
        [Key]
        public int chatlog_id { get; set; }
        //[Required]
        /// <summary>
        /// Gets or sets the sender identifier.
        /// </summary>
        /// <value>The sender identifier.</value>
        public int sender_id { get; set; }
        /// <summary>
        /// Gets or sets the empfaenger identifier.
        /// </summary>
        /// <value>The empfaenger identifier.</value>
        [Required]
        public int empfaenger_id { get; set; }
        //[Required]
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string message { get; set; }
        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        [Required]
        public DateTime timestamp { get; set; }
        /// <summary>
        /// Gets or sets the support group.
        /// </summary>
        /// <value>The support group.</value>
        public virtual ICollection<SupportGroup> SupportGroup { get; set; }
    }
}
