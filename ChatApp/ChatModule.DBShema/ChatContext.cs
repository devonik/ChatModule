// ***********************************************************************
// Assembly         : ChatModule.DBShema
// Author           : grieger
// Created          : 03-09-2017
//
// Last Modified By : grieger
// Last Modified On : 03-06-2017
// ***********************************************************************
// <copyright file="ChatContext.cs" company="Berenberg">
//     Copyright © Berenberg 2016
// </copyright>
// <summary></summary>
// ***********************************************************************
using ChatModule.DBShema.Models;
using System.Data.Entity;

namespace ChatModule.DBShema
{
    // represents a session with the database, allowing us to query and save data.We define a context that derives from System.Data.Entity.DbContext and exposes a typed DbSet<TEntity> for each class in our model.
    /// <summary>
    /// Class ChatContext.
    /// </summary>
    /// <seealso cref="System.Data.Entity.DbContext" />
    public class ChatContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChatContext"/> class.
        /// </summary>
        public ChatContext() : base("ChatContext")
        {
        }

        /// <summary>
        /// Gets or sets the user.
        /// </summary>
        /// <value>The user.</value>
        public DbSet<User> User { get; set; }
        /// <summary>
        /// Gets or sets the chatlog.
        /// </summary>
        /// <value>The chatlog.</value>
        public DbSet<Chatlog> Chatlog { get; set; }
        /// <summary>
        /// Gets or sets the support group.
        /// </summary>
        /// <value>The support group.</value>
        public DbSet<SupportGroup> SupportGroup { get; set; }
        /// <summary>
        /// Gets or sets the user2 support group.
        /// </summary>
        /// <value>The user2 support group.</value>
        public DbSet<User2SupportGroup> User2SupportGroup { get; set; }
    }
}
