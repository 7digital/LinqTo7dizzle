using LinqTo7Dizzle.OAuth;

namespace LinqTo7Dizzle
{
    public class SevenDizzleExecute : IExecute
    {
        private readonly IAuthorize _authorize;

        public SevenDizzleExecute(IAuthorize authorize)
        {
            _authorize = authorize;
        }
    }
}