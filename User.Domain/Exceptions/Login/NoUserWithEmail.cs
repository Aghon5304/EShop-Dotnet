namespace User.Domain.Exceptions.Login
{
    public class NoUserWithEmail : Exception
    {
        public NoUserWithEmail() : base("no account with selected email")
        { }
    }
}
