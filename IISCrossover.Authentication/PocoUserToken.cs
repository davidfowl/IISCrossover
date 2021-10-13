using System;

namespace IISCrossover.Authentication
{
    /// <summary>
    /// Entity type for a user's token
    /// </summary>
    public class PocoUserToken : PocoUserToken<string> { }

    /// <summary>
    /// Entity type for a user's token
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public class PocoUserToken<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        ///     The login provider for the login (i.e. facebook, google)
        /// </summary>
        public virtual string LoginProvider { get; set; }

        /// <summary>
        ///     Key representing the login for the provider
        /// </summary>
        public virtual string TokenName { get; set; }

        /// <summary>
        ///     Display name for the login
        /// </summary>
        public virtual string TokenValue { get; set; }

        /// <summary>
        ///     User Id for the user who owns this login
        /// </summary>
        public virtual TKey UserId { get; set; }
    }
}
