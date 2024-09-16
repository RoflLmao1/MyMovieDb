namespace MyMovieDb;

public static class MyMovieDbDomainErrorCodes
{
    public const string Namespace = nameof(MyMovieDb);

    /* You can add your business exception error codes here, as constants */

    /// <summary>
    /// Movie related
    /// </summary>
    public static class Movie
    {
        public const string Namespace = $"{MyMovieDbDomainErrorCodes.Namespace}.{nameof(Movie)}";

        /// <summary>
        /// Invalid movie
        /// </summary>
        public static class InvalidMovie
        {
            /// <summary>
            /// Localization key
            /// </summary>
            public const string Code = $"{Movie.Namespace}:{nameof(InvalidMovie)}";

            /// <summary>
            /// String formats to replace with actual values
            /// </summary>
            public static class Formats
            {
                /// <summary>
                /// Id
                /// </summary>
                public const string Id = "Id";
            }
        }
    }
}
